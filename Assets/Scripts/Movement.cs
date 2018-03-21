using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour 
{
    private MovementItem movement;
    public Vector3 savedPosition;
    private float movementPercentageElapsed = 1;
    private float baseSpeed = 10;
    private float speed;
    public bool isMoving = false;
    private List<MovementItem> movementList = new List<MovementItem>();

    public void HandleMovement()
    {
        if(movementPercentageElapsed >= 1) // If no animation is playing
        {
            if(movementList.Count != 0) // If there is an animation already queued
            {
                movement = movementList[0];
                movementList.RemoveAt(0);

                if(movement.SavePosition())
                    savedPosition = transform.position;

                if(!movement.CheckForward())
                {
                    movementPercentageElapsed = 0; // Restarts animation
                    isMoving = true;
                    if(movement.TriggerParty())
                        AddMovementToParty();
                }  
                else if(HandleObstacle(movement.GetVector()))
                {
                    movementPercentageElapsed = 0;
                    isMoving = true;
                    if(movement.TriggerParty())
                        AddMovementToParty();
                }
                
                if(movement.GetVector().y < 0) // Faster when falling to imitate gravity
                    speed = baseSpeed * 3;
                else
                    speed = baseSpeed;
            }
            else
                isMoving = false; // Reset state if no more movements
        }
        if(movementPercentageElapsed < 1)
        {
            float delta = Time.deltaTime * speed;
            float deltaPercentage = delta / movement.GetVector().magnitude;
            if(movementPercentageElapsed + deltaPercentage > 1)
                deltaPercentage = (1 - movementPercentageElapsed);
            transform.Translate(movement.GetVector() * deltaPercentage);
            movementPercentageElapsed += deltaPercentage;
        }
    }

    public void Move(MovementItem movement)
    {
        if(movementList.Count < 3)  // To prevent people from spamming the button and adding new movements
            movementList.Add(movement);
    }

	protected virtual bool HandleObstacle(Vector3 direction)
    {
        RaycastHit hit;
        GameObject obstacle = null;

        if(Physics.Raycast(transform.position, direction, out hit, 1)) // Checks block where you want to move
        {
            obstacle = hit.transform.gameObject;
            if(obstacle.tag == "Ladder")
                return HandleLadder(obstacle, direction);
            else
                return false;
        }
        else
            return HandleNoObstacle(direction); // If there is no obstacle checks if there is a floor
    }

    private bool HandleNoObstacle(Vector3 direction)
    {
        RaycastHit hit;
        GameObject obstacle = null;

        if(Physics.Raycast(transform.position + direction, Vector3.down, out hit, 10)) // Checks below the block is moving to
        {
            obstacle = hit.transform.gameObject;
            if(obstacle.tag != "Party")
            {
                if((int) hit.distance > 0)
                    movementList.Insert(0, new MovementItem(Vector3.down * (int) hit.distance, false, true, false));
                return true;
            }
        }

        movementPercentageElapsed = 0; // Small animation starts imidiatly
        isMoving = true;
        movement = new MovementItem(direction/2f, false, false, false);
        movementList.Insert(0, new MovementItem(-direction/2f, false, false, false));  // Adds reverse to animation queue 
        return false;
    }

    public bool HandleLadder(GameObject ladder, Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up * ladder.GetComponent<Renderer>().bounds.size.y, direction, out hit, 1))
            return false;
        else
        {
            movement = new MovementItem(Vector3.up * ladder.GetComponent<Renderer>().bounds.size.y, false, false, true);
            movementList.Insert(0, new MovementItem(direction, false, true, false));
            return true;
        }
    }

    protected virtual void AddMovementToParty()
    {

    }
}
