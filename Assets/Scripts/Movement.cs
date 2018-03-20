using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour 
{
    private Vector3 movementVector;
    private float hElapsed = 1;
    public float baseSpeed = 5;
    private float speed;
    public bool isMoving = false;
    private List<Vector3> movementList = new List<Vector3>();

    public void handleMovement()
    {
        if(hElapsed >= 1)
        {
            if(movementList.Count != 0)
            {
                isMoving = true;
                movementVector = movementList[0];
                movementList.RemoveAt(0);
                if(movementVector.magnitude < 1 || movementVector.y != 0 || handleObstacle(movementVector))
                    hElapsed = 0;
                else
                    isMoving = false;
                
                if(movementVector.y < 0)
                    speed = baseSpeed * 3;
                else
                    speed = baseSpeed;
            }
            else
                isMoving = false;
        }
        if(hElapsed < 1)
        {
            float move = Time.deltaTime * speed;
            float movePercentage = move / movementVector.magnitude;
            if(hElapsed + movePercentage > 1)
                movePercentage = (1 - hElapsed);
            transform.Translate(movementVector * movePercentage);
            hElapsed += movePercentage;
        }
    }

    public void Move(Vector3 direction)
    {
        movementList.Add(direction);
    }

	private bool handleObstacle(Vector3 direction)
    {
        RaycastHit hit;
        GameObject obstacle = null;

        if(Physics.Raycast(transform.position, direction, out hit, 1))
        {
            obstacle = hit.transform.gameObject;
            if(obstacle.tag == "Ladder")
                return false;
            if(obstacle.tag == "Party")
                return false;
            else
                return false;
        }
        else
            return handleNoObstacle(direction);
    }

    private bool handleNoObstacle(Vector3 direction)
    {
        RaycastHit hit;
        GameObject obstacle = null;

        if(Physics.Raycast(transform.position + direction, Vector3.down, out hit, 10))
        {
            obstacle = hit.transform.gameObject;
            if(obstacle.tag == "Party")
                return false;
            else
            {
                movementList.Insert(0, Vector3.down * (int) hit.distance);
                return true;
            }
        }
        else
        {
            movementList.Insert(0, -direction/1.6f);
            movementList.Insert(0, direction/1.6f);
            return false;
        }
    }

    public GameObject GetObstaclAboveLadder(GameObject ladder, Vector3 direction)
    {
        RaycastHit hit;
        GameObject obstacle = null;

        if (Physics.Raycast(transform.position + Vector3.up * ladder.GetComponent<Renderer>().bounds.size.y, direction, out hit, 1))
        {
            obstacle = hit.transform.gameObject;
        }

        return obstacle;
    }
}
