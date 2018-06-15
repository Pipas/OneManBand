using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour 
{
    public enum State { falling, moving, still, climbing }
    public State state;
    public static List<GameObject> party = new List<GameObject>();
    public GameObject nextInParty;
    private AnimationItem currentAnimation;
    public Vector3 savedPosition;
    private float movementPercentageElapsed = 1;
    public float baseSpeed = 6;
    public bool isMoving = false;
    private Queue<AnimationItem> animationQueue = new Queue<AnimationItem>();
    private Queue<Vector3> userInputQueue = new Queue<Vector3>();
    
    public void HandleMovement()
    {
        if(movementPercentageElapsed >= 1) // If no animation is playing
        {
            foreach(GameObject member in party)
                if(transform.tag == "Player" && member.GetComponent<PartyMovement>().isMoving) // Waits for all animations to end
                    return;

            if(animationQueue.Count == 0 && userInputQueue.Count == 0)
                CheckHoldInput();

            if(animationQueue.Count == 0 && userInputQueue.Count != 0) // If there is no animation and an input Validates the input, it's up here so it starts in the same frame it validates
                ValidateInput(userInputQueue.Dequeue());
            
            if(animationQueue.Count != 0) // If there is an animation queued
            {
                currentAnimation = animationQueue.Dequeue();

                changeState(currentAnimation);

                if(currentAnimation.SavePosition()) // if it's supposed to save its position, used for the next in party
                    SavePosition();

                if(currentAnimation.TriggerParty()) // Triggers next in party to start animation with new saved animations
                    TriggerNextInParty();

                movementPercentageElapsed = 0;
                isMoving = true;
            }
        }
        if(movementPercentageElapsed < 1) // Handles the actual animation frame by frame
        {            
            float delta = Time.deltaTime * currentAnimation.GetSpeed();
            float deltaPercentage = delta / currentAnimation.GetVector().magnitude;
            if(movementPercentageElapsed + deltaPercentage > 1)
                deltaPercentage = (1 - movementPercentageElapsed);   
            transform.Translate(currentAnimation.GetVector() * deltaPercentage, Space.World);
            movementPercentageElapsed += deltaPercentage;

            if(movementPercentageElapsed == 1 && animationQueue.Count == 0 && userInputQueue.Count == 0) // if there is no more animations and it's the end toggles isMoving, saves a frame
            {
                isMoving = false;
                state = State.still;
                CheckIfDitched(); // At the end of an animation checks to see if party members were ditched
            } 
        }
    }

    private void changeState(AnimationItem currentAnimation)
    {
        if(currentAnimation.GetVector().y > 0)
            state = State.climbing;
        else if(currentAnimation.GetVector().y == 0)
        {
            if(animationQueue.Count != 0 && animationQueue.Peek().GetVector().y != 0)
                state = State.falling;
            else
                state = State.moving;
        }
    }

    /* Enqueues an animation */
    public void QueueAnimation(AnimationItem anim)
    {
        animationQueue.Enqueue(anim);
    }

    /* Enqueues palyer input */
    public void QueueInput(Vector3 direction)
    {
        if(userInputQueue.Count < 3) // To prevent people from spamming the button and adding new inputs
            userInputQueue.Enqueue(direction);  
    }

    /* Validates the input and adds animation accordingly */
	protected void ValidateInput(Vector3 direction)
    {
        foreach(GameObject member in party) {
            member.GetComponent<PartyMovement>().setPlayerDirection(direction);
        }

        if (direction == Vector3.forward)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        }
        else if (direction == Vector3.back)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        }
        else if (direction == Vector3.right)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        }
        else if (direction == Vector3.left)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -90, transform.eulerAngles.z);
        }
        
        RaycastHit hit;
        GameObject obstacle = null;

        var layerMask = (1 << 8);
        layerMask = ~layerMask;

        var distance = 1f;

        if (Physics.Raycast(transform.position, direction, out hit, distance, layerMask)) // Checks block where you want to move
        {
            obstacle = hit.transform.gameObject;

            if(obstacle.tag == "Ladder")
                HandleLadder(obstacle, direction); // If there is a ladder
            else if(obstacle.tag == "Sheet")
            {
                HandleSheet(obstacle); // If there is a Sheet
                HandleNoObstacle(direction);
            }
            else if(obstacle.tag == "Bridge")
            {
                QueueAnimation(new AnimationItem(direction/5f, baseSpeed/2, false, false)); // If can't move enqueues small animation to display that you can't move
                QueueAnimation(new AnimationItem(-direction/5f, baseSpeed/2, false, false)); // Enqueues reverse animation
            }
            /*else
            {
                QueueAnimation(new AnimationItem(direction/10f, baseSpeed/2, false, false)); // If can't move enqueues small animation to display that you can't move
                QueueAnimation(new AnimationItem(-direction/10f, baseSpeed/2, false, false)); // Enqueues reverse animation
            }*/
        }
        else
            HandleNoObstacle(direction); // If there is no obstacle checks if there is a floor
    }

    private void HandleNoObstacle(Vector3 direction)
    {
        RaycastHit hit;
        GameObject obstacle = null;

        var layerMask = (1 << 8);
        layerMask = ~layerMask;
        var distAhead = transform.position + 1.2f*direction;

        if (Physics.Raycast(distAhead, Vector3.down, out hit, 10, layerMask)) // Checks below the block is moving to
        {
            obstacle = hit.transform.gameObject;
            if(obstacle.tag != "Party")
            {
                if(obstacle.tag == "Sheet")
                    HandleSheet(obstacle);

                if(hit.distance > 0.4f) // If there is a fall enqueues 2 animations move and fall
                {
                    QueueAnimation(new AnimationItem(direction, baseSpeed, true, false));
                    QueueAnimation(new AnimationItem(Vector3.down * (hit.distance - 0.4f), baseSpeed * 3, false, true));
                }
                else // If not just moves
                    QueueAnimation(new AnimationItem(direction, baseSpeed, true, true));
                return;
            }
        }

        /*QueueAnimation(new AnimationItem(direction/3f, baseSpeed/1.5f, false, false)); // If can't move enqueues small animation to display that you can't move
        QueueAnimation(new AnimationItem(-direction/3f, baseSpeed/1.5f, false, false)); // Enqueues reverse animation*/
    }

    public void HandleLadder(GameObject ladder, Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up * ladder.GetComponent<Renderer>().bounds.size.y, direction, out hit, 1)) // If a block is above the ladder
            return;
        else // Enqueues the ladder climbing animations
        {
            QueueAnimation(new AnimationItem(Vector3.up * ladder.GetComponent<Renderer>().bounds.size.y, baseSpeed, true, false));
            QueueAnimation(new AnimationItem(direction, baseSpeed, false, true));
        }
    }

    public void HandleSheet(GameObject sheet)
    {
        sheet.GetComponent<Sheet>().RemovePage();
        int Level1Pages = PlayerPrefs.GetInt("Level1Pages");
        PlayerPrefs.SetInt("Level1Pages", Level1Pages + 1); 
    }

    private void SavePosition() // Saves self and next in party position to calculate the movement
    {
        if(nextInParty != null)
        {
            savedPosition = transform.position;

            Vector3 savedPos = new Vector3(nextInParty.transform.position.x, nextInParty.transform.position.y + 0.4f, nextInParty.transform.position.z);
            nextInParty.GetComponent<PartyMovement>().savedPosition = savedPos;
        }
    }

    private void TriggerNextInParty() // Triggers the next in party to move
	{
        if(nextInParty != null)
        {
            PartyMovement nextMovement = nextInParty.GetComponent<PartyMovement>();
            Vector3 diffHPositions = new Vector3(savedPosition.x - nextMovement.savedPosition.x, 0, savedPosition.z - nextMovement.savedPosition.z); // Horizontal Vector Difference
            Vector3 diffVPositions = new Vector3(0, savedPosition.y - nextMovement.savedPosition.y, 0); // Vertical Vector Difference

            if(diffHPositions.magnitude < 1.1) // Only move if adjacent (prevents oblique movement), sometimes is 1.000001 so can't do == 0
            {
                if(diffVPositions.y > 0) // If there was a vertical change
                {
                    nextMovement.QueueAnimation(new AnimationItem(diffVPositions, baseSpeed, true, false));
                    nextMovement.QueueAnimation(new AnimationItem(diffHPositions, baseSpeed, false, true));
                }	
                else if(diffVPositions.y < 0) // If there was a vertical change
                {
                    nextMovement.QueueAnimation(new AnimationItem(diffHPositions, baseSpeed, true, false));
                    nextMovement.QueueAnimation(new AnimationItem(diffVPositions, baseSpeed * 3, false, true));
                }
                else
                    nextMovement.QueueAnimation(new AnimationItem(diffHPositions, baseSpeed, true, true));
            }
        }	
	}

    protected virtual void CheckHoldInput() {}
    public virtual void CheckIfDitched() {}
}
