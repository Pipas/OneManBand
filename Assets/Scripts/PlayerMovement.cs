using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement 
{	
	private Rigidbody playerRB;
	private List<GameObject> party = new List<GameObject>();
	void Start ()
	{
		init();
		playerRB = GetComponent<Rigidbody>();
	}

	void Update ()
	{
		if(CanMove())
		{
			if(Input.GetKeyDown(KeyCode.UpArrow))
				MovePlayer(Vector3.forward);
			else if(Input.GetKeyDown(KeyCode.DownArrow))
				MovePlayer(Vector3.back);
			else if(Input.GetKeyDown(KeyCode.LeftArrow))
				MovePlayer(Vector3.left);
			else if(Input.GetKeyDown(KeyCode.RightArrow))
				MovePlayer(Vector3.right);
		}

		if(transform.position.y < -10)
		{
			ResetSelf();
			foreach(GameObject member in party)
				member.GetComponent<PartyMovement>().ResetSelf();
			party.Clear();
		}
	}

	void MoveParty()
	{
		Vector3 lastPartyMove = lastPosition;

		foreach(GameObject member in party)
		{
			member.GetComponent<PartyMovement>().Move(new Vector3(lastPartyMove.x - member.transform.position.x, 0 ,lastPartyMove.z - member.transform.position.z));
			lastPartyMove = member.GetComponent<PartyMovement>().lastPosition;
		}
	}

	public void MovePlayer(Vector3 direction)
	{
		lastPosition = transform.position;

		GameObject obstacle = GetObstacle(direction);
		if(obstacle == null)
		{
			transform.Translate(direction);
		}
        else if(obstacle.tag == "Ladder")
        {
			GameObject objectAboveStairs = GetObstaclAboveLadder(obstacle, direction);
			if(objectAboveStairs == null)
			{
				transform.Translate(direction + Vector3.up * obstacle.GetComponent<Renderer>().bounds.size.y);
			}
			else if(objectAboveStairs.tag == "Party")
			{
				if(!party.Contains(objectAboveStairs))
					party.Add(objectAboveStairs);
				return;
			}
        }
		else if(obstacle.tag == "Party")
		{
			if(!party.Contains(obstacle))
				party.Add(obstacle);
			return;
		}

		MoveParty();
	}

	private bool CanMove()
	{
		bool canMove = true;
		if(playerRB.velocity != Vector3.zero)
			canMove = false;
		
		foreach(GameObject member in party)
		{
			if(member.GetComponent<Rigidbody>().velocity != Vector3.zero)
				canMove = false;
		}

		return canMove;
	}
}
