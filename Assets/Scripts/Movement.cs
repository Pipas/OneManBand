using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour 
{
    private Vector3 hVector;
    private float hElapsed;
    public float speed = 15f;
    private bool isMoving = false;
    private List<Vector3> bufferMovements = new List<Vector3>();

    public void handleMovement()
    {
        if(hElapsed < 1)
        {
            float move = hVector.magnitude * Time.deltaTime * speed;
            if(hElapsed + move > 1)
                move = 1 - hElapsed;
            transform.Translate(hVector * move);
            hElapsed += move;
        }
        if(hElapsed >= 1)
        {
            if(bufferMovements.Count != 0)
            {
                hVector = bufferMovements[0];
                bufferMovements.RemoveAt(0);
                hElapsed = 0;
            }
            else
                isMoving = false;
        }
    }

    public void Move(Vector3 direction)
    {
        if(!isMoving)
        {
            hVector = direction;
            hElapsed = 0;
            isMoving = true;
        }
        else
        {
            bufferMovements.Add(direction);
        }
    }

	public GameObject GetObstacle(Vector3 direction)
    {
        RaycastHit hit;
        GameObject obstacle = null;

        if (Physics.Raycast(transform.position, direction, out hit, 1))
        {
            obstacle = hit.transform.gameObject;
        }

        return obstacle;
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
