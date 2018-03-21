using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour 
{
    protected static List<GameObject> party = new List<GameObject>();
    public GameObject nextInParty;
    private AnimationItem currentAnimation;
    public Vector3 savedPosition;
    private float movementPercentageElapsed = 1;
    private float baseSpeed = 10;
    private float speed;
    public bool isMoving = false;
    private Queue<AnimationItem> animationQueue = new Queue<AnimationItem>();
    private Queue<Vector3> userInputQueue = new Queue<Vector3>();

    public void HandleMovement()
    {
        if(movementPercentageElapsed >= 1) // If no animation is playing
        {
            if(nextInParty != null && nextInParty.GetComponent<PartyMovement>().isMoving) // Waits for all animations to end
                return;
            
            if(animationQueue.Count == 0 && userInputQueue.Count != 0) // If there is no animation and an input Validates the input, it's up here so it starts in the same frame it validates
                ValidateInput(userInputQueue.Dequeue());
            
            if(animationQueue.Count != 0) // If there is an animation queued
            {
                currentAnimation = animationQueue.Dequeue();

                if(currentAnimation.SavePosition()) // if it's supposed to save its position, used for the next in party
                    SavePosition();

                if(currentAnimation.TriggerParty()) // Triggers next in party to start animation with new saved animations
                        TriggerNextInParty();

                movementPercentageElapsed = 0;
                isMoving = true;
                
                if(currentAnimation.GetVector().y < 0) // Faster when falling to imitate gravity
                    speed = baseSpeed * 3;
                else
                    speed = baseSpeed;
            }
            else
                isMoving = false; // Reset state if no more movements
        }
        if(movementPercentageElapsed < 1) // Handles the actual animation frame by frame
        {
            float delta = Time.deltaTime * speed;
            float deltaPercentage = delta / currentAnimation.GetVector().magnitude;
            if(movementPercentageElapsed + deltaPercentage > 1)
                deltaPercentage = (1 - movementPercentageElapsed);   
            transform.Translate(currentAnimation.GetVector() * deltaPercentage);
            movementPercentageElapsed += deltaPercentage;
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
        if(userInputQueue.Count < 3)  // To prevent people from spamming the button and adding new inputs
            userInputQueue.Enqueue(direction);
    }

    /* Validates the input and adds animation accordingly */
	protected void ValidateInput(Vector3 direction)
    {
        RaycastHit hit;
        GameObject obstacle = null;

        if(Physics.Raycast(transform.position, direction, out hit, 1)) // Checks block where you want to move
        {
            obstacle = hit.transform.gameObject;
            if(obstacle.tag == "Ladder")
                HandleLadder(obstacle, direction); // If there is a ladder
        }
        else
            HandleNoObstacle(direction); // If there is no obstacle checks if there is a floor
    }

    private void HandleNoObstacle(Vector3 direction)
    {
        RaycastHit hit;
        GameObject obstacle = null;

        if(Physics.Raycast(transform.position + direction, Vector3.down, out hit, 10)) // Checks below the block is moving to
        {
            obstacle = hit.transform.gameObject;
            if(obstacle.tag != "Party")
            {
                if((int) hit.distance > 0) // If there is a fall enqueues 2 animations move and fall
                {
                    QueueAnimation(new AnimationItem(direction, true, false));
                    QueueAnimation(new AnimationItem(Vector3.down * (int) hit.distance, false, true));
                }
                else // If not just moves
                    QueueAnimation(new AnimationItem(direction, true, true));
                return;
            }
        }

        QueueAnimation(new AnimationItem(direction/2f, false, false)); // If can't move enqueues small animation to display that you can't move
        QueueAnimation(new AnimationItem(-direction/2f, false, false)); // Enqueues reverse animation
    }

    public void HandleLadder(GameObject ladder, Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up * ladder.GetComponent<Renderer>().bounds.size.y, direction, out hit, 1)) // If a block is above the ladder
            return;
        else // Enqueues the ladder climbing animations
        {
            QueueAnimation(new AnimationItem(Vector3.up * ladder.GetComponent<Renderer>().bounds.size.y, true, false));
            QueueAnimation(new AnimationItem(direction, false, true));
        }
    }

    private void SavePosition() // Saves self and next in party position to calculate the movement
    {
        if(nextInParty != null)
        {
            savedPosition = transform.position;
            nextInParty.GetComponent<PartyMovement>().savedPosition = nextInParty.transform.position;
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
                    nextMovement.QueueAnimation(new AnimationItem(diffVPositions, true, false));
                    nextMovement.QueueAnimation(new AnimationItem(diffHPositions, false, true));
                }	
                else if(diffVPositions.y < 0) // If there was a vertical change
                {
                    nextMovement.QueueAnimation(new AnimationItem(diffHPositions, true, false));
                    nextMovement.QueueAnimation(new AnimationItem(diffVPositions, false, true));
                }
                else
                    nextMovement.QueueAnimation(new AnimationItem(diffHPositions, true, true));
            }
        }	
	}
}
