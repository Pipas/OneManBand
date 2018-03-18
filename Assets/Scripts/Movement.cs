using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour 
{
	private GameObject GetObstacle(Vector3 direction)
    {
        RaycastHit hit;
        GameObject obstacle = null;

        if (Physics.Raycast(transform.position, direction, out hit, 1))
        {
            obstacle = hit.transform.gameObject;
        }

        return obstacle;
    }

    private int GetLadderLength(GameObject ladderBase, int currentLength)
    {
        RaycastHit hit;
        GameObject nextStep = null;

        if (Physics.Raycast(ladderBase.transform.position, Vector3.up, out hit, 1))
        {
            nextStep = hit.transform.gameObject;
        }

        if(nextStep == null)
            return currentLength;
        else
            return GetLadderLength(nextStep, currentLength + 1);
    }

	public void Move(Vector3 direction)
	{
        GameObject obstacle = GetObstacle(direction);
		if(obstacle == null)
		{
			transform.Translate(direction);
		}
        else if(obstacle.tag == "Ladder")
        {
            transform.Translate(direction + Vector3.up * GetLadderLength(obstacle, 1));
        }
	}
}
