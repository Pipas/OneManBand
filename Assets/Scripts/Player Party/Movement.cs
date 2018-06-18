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
    public float baseSpeed;
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
                updateDirection(currentAnimation.GetVector());

                if(currentAnimation.SavePosition()) // if it's supposed to save its position, used for the next in party
                    SavePosition();

                if(currentAnimation.TriggerParty()) // Triggers next in party to start animation with new saved animations
                    TriggerNextInParty(currentAnimation.getPartySwap());

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
        updateDirection(direction);

        RaycastHit hit;
        GameObject obstacle = null;

        var distance = 1f;

        if (Physics.Raycast(transform.position, direction, out hit, distance)) // Checks block where you want to move
        {
            obstacle = hit.transform.gameObject;
            Debug.DrawRay(transform.position, direction * hit.distance, Color.red);

            if(obstacle.tag == "Sheet")
            {
                HandleSheet(obstacle); // If there is a Sheet
            }
            else if(obstacle.tag == "Party")
            {
                HandlePartyMember(direction, obstacle);
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

    private void updateDirection(Vector3 direction)
    {
        if (direction.z > 0.5)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        }
        else if (direction.z < -0.5)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        }
        else if (direction.x > 0.5)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        }
        else if (direction.x < -0.5)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -90, transform.eulerAngles.z);
        }
    }

    private void HandleNoObstacle(Vector3 direction)
    {
        RaycastHit hit;
        GameObject obstacle = null;

        var distAhead = transform.position + 1.2f*direction;

        if (Physics.Raycast(distAhead, Vector3.down, out hit, 10)) // Checks below the block is moving to
        {
            obstacle = hit.transform.gameObject;
            if(obstacle.tag != "Party")
            {

                if(hit.distance > 0.55f) // If there is a fall enqueues 2 animations move and fall
                {
                    QueueAnimation(new AnimationItem(direction, baseSpeed, true, false));
                    QueueAnimation(new AnimationItem(Vector3.down * (hit.distance - 0.5f), baseSpeed * 3, false, true));
                }
                else // If not just moves
                    QueueAnimation(new AnimationItem(direction, baseSpeed, true, true));
                return;
            }
        }
    }

    private void HandlePartyMember(Vector3 direction, GameObject partyMember)
    {
        if(party.Contains(partyMember))
        {
            QueueAnimation(new AnimationItem(direction, baseSpeed, true, true, partyMember));
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

            float party_position_y = nextInParty.transform.position.y;

            if (this.gameObject.tag == "Player")
                party_position_y += 0.4f;

            Vector3 savedPos = new Vector3(nextInParty.transform.position.x, party_position_y, nextInParty.transform.position.z);
            nextInParty.GetComponent<PartyMovement>().savedPosition = savedPos;
        }
    }

    private void TriggerNextInParty(GameObject partySwap) // Triggers the next in party to move
	{
        if(nextInParty != null)
        {
            if(partySwap != null)
                if(party.IndexOf(partySwap) < party.IndexOf(nextInParty))
                    return;

            PartyMovement nextMovement = nextInParty.GetComponent<PartyMovement>();
            Vector3 diffHPositions = new Vector3(savedPosition.x - nextMovement.savedPosition.x, 0, savedPosition.z - nextMovement.savedPosition.z); // Horizontal Vector Difference
            Vector3 diffVPositions = new Vector3(0, savedPosition.y - nextMovement.savedPosition.y, 0); // Vertical Vector Difference

            if(diffHPositions.magnitude < 1.1) // Only move if adjacent (prevents oblique movement), sometimes is 1.000001 so can't do == 0
            {
                if(diffVPositions.y < 0) // If there was a vertical change
                {
                    nextMovement.QueueAnimation(new AnimationItem(diffHPositions, baseSpeed, true, false, partySwap));
                    nextMovement.QueueAnimation(new AnimationItem(diffVPositions, baseSpeed * 3, false, true, partySwap));
                }
                else
                    nextMovement.QueueAnimation(new AnimationItem(diffHPositions, baseSpeed, true, true, partySwap));
            }
            else
            {
                if(diffVPositions.y < 0) // If there was a vertical change
                {
                    if(Mathf.Floor(this.gameObject.transform.position.x) == Mathf.Floor(GameObject.Find("PlayerPivot").transform.position.x))
                    {
                        nextMovement.QueueAnimation(new AnimationItem(new Vector3(diffHPositions.x, 0, 0), baseSpeed, true, false, partySwap));
                        nextMovement.QueueAnimation(new AnimationItem(new Vector3(0, 0, diffHPositions.z), baseSpeed, false, false, partySwap));
                    }
                    else
                    {
                        nextMovement.QueueAnimation(new AnimationItem(new Vector3(0, 0, diffHPositions.z), baseSpeed, true, false, partySwap));
                        nextMovement.QueueAnimation(new AnimationItem(new Vector3(diffHPositions.x, 0, 0), baseSpeed, false, false, partySwap));
                    }
                    nextMovement.QueueAnimation(new AnimationItem(diffVPositions, baseSpeed * 3, false, true, partySwap));
                }
                else
                {
                    if(Mathf.Floor(this.gameObject.transform.position.x) == Mathf.Floor(GameObject.Find("PlayerPivot").transform.position.x))
                    {
                        nextMovement.QueueAnimation(new AnimationItem(new Vector3(diffHPositions.x, 0, 0), baseSpeed, true, false, partySwap));
                        nextMovement.QueueAnimation(new AnimationItem(new Vector3(0, 0, diffHPositions.z), baseSpeed, false, true, partySwap));
                    }
                    else
                    {
                        nextMovement.QueueAnimation(new AnimationItem(new Vector3(0, 0, diffHPositions.z), baseSpeed, true, false, partySwap));
                        nextMovement.QueueAnimation(new AnimationItem(new Vector3(diffHPositions.x, 0, 0), baseSpeed, false, true, partySwap));
                    }
                }
            }
        }
	}

    protected virtual void CheckHoldInput() {}
    public virtual void CheckIfDitched() {}
}
