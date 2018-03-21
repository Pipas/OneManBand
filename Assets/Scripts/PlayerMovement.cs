using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : Movement 
{	
	void Start ()
	{
		savedPosition = transform.position;
	}
	void Update ()
	{	
		HandleInput(); // Handles the user input, duh

		HandleMovement(); // Updates movement everyframe

		HandlePartyMovement(); // Updates the rest of the party movement, this way it's sequential after the player
	}

	private void HandleInput()
	{
		if(!isMoving) // So you can hold down the button and it works
		{
			if(Input.GetButton("Up"))
				QueueInput(Vector3.forward);
			if(Input.GetButton("Down"))
				QueueInput(Vector3.back);
			if(Input.GetButton("Left"))
				QueueInput(Vector3.left);
			if(Input.GetButton("Right"))
				QueueInput(Vector3.right);
		}
		else // So if you click during an animation it still works
		{
			if(Input.GetButtonDown("Up"))
				QueueInput(Vector3.forward);
			if(Input.GetButtonDown("Down"))
				QueueInput(Vector3.back);
			if(Input.GetButtonDown("Left"))
				QueueInput(Vector3.left);
			if(Input.GetButtonDown("Right"))
				QueueInput(Vector3.right);
		}

		if(Input.GetButtonDown("Interact"))
			checkSurroundings();

		if(Input.GetButtonDown("Ditch"))
			DitchPartyMember();
	}

	private void checkSurroundings() // Checks all surrounding blocks and adds party members to party
	{
		Vector3[] directions = {Vector3.forward, Vector3.back, Vector3.left, Vector3.right};
		RaycastHit hit;
        GameObject obstacle = null;

        foreach(Vector3 direction in directions)
		{
			if (Physics.Raycast(transform.position, direction, out hit, 1))
			{
				obstacle = hit.transform.gameObject;
				if(obstacle.tag == "Party")
				{
					if(!party.Contains(obstacle))
					{
						party.Add(obstacle);
						if(party.Count == 1)
						{
							nextInParty = obstacle;
						}
						else
							party[party.IndexOf(obstacle) - 1].GetComponent<PartyMovement>().nextInParty = obstacle;	
					}
				}
			}
		}
	}

	private void HandlePartyMovement()
	{
		foreach(GameObject member in party)
			member.GetComponent<PartyMovement>().HandleMovement();
	}

	private void DitchPartyMember()
	{
		if(party.Count > 1)
		{
			party[party.Count - 2].GetComponent<PartyMovement>().nextInParty = null;
			party.RemoveAt(party.Count - 1);
		}
		else if(party.Count == 1)
		{
			nextInParty = null;
			party.RemoveAt(party.Count - 1);
		}
	}
}