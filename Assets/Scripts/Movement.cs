using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour 
{
    public Vector3 lastPosition = Vector3.zero;
    private Vector3 startPosition;

    public Rigidbody selfRB;

    public void init()
    {
        startPosition = transform.position;
        selfRB = GetComponent<Rigidbody>();
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

	public void Move(Vector3 direction)
	{
        lastPosition = transform.position;

        GameObject obstacle = GetObstacle(direction);
		if(obstacle == null)
		{
			transform.Translate(direction);
		}
        else if(obstacle.tag == "Ladder")
        {
            transform.Translate(direction + Vector3.up * obstacle.GetComponent<Renderer>().bounds.size.y);
        }
	}

    public void ResetSelf()
	{
		selfRB.velocity = Vector3.zero;
		transform.position = startPosition;
	}
}
