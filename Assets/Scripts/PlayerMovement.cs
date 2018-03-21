using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : Movement 
{	
	private List<GameObject> party = new List<GameObject>();

	void Start ()
	{
		savedPosition = transform.position;
	}
	void Update ()
	{	
		HandleInput(); // Handles the user input, duh

		HandleMovement(); // Updates movement everyframe

		HandlePartyMovement();
	}

	private void HandleInput()
	{
		if(!isMoving) // So you can hold down the button and it works
		{
			if(Input.GetButton("Up"))
				Move(new MovementItem(Vector3.forward));
			if(Input.GetButton("Down"))
				Move(new MovementItem(Vector3.back));
			if(Input.GetButton("Left"))
				Move(new MovementItem(Vector3.left));
			if(Input.GetButton("Right"))
				Move(new MovementItem(Vector3.right));
		}
		else // So if you click during an animation it still works
		{
			if(Input.GetButtonDown("Up"))
				Move(new MovementItem(Vector3.forward));
			if(Input.GetButtonDown("Down"))
				Move(new MovementItem(Vector3.back));
			if(Input.GetButtonDown("Left"))
				Move(new MovementItem(Vector3.left));
			if(Input.GetButtonDown("Right"))
				Move(new MovementItem(Vector3.right));
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
						party.Add(obstacle);
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
		if(party.Count > 0)
			party.RemoveAt(party.Count - 1);
	}

	override protected void AddMovementToParty()
	{
		Vector3 previousPosition = savedPosition;
		foreach(GameObject member in party)
		{
			PartyMovement memberMovement = member.GetComponent<PartyMovement>();
			Vector3 diffHPositions = new Vector3(previousPosition.x - memberMovement.savedPosition.x, 0, previousPosition.z - memberMovement.savedPosition.z); // Horizontal Vector Difference
			Vector3 diffVPositions = new Vector3(0, previousPosition.y - memberMovement.savedPosition.y, 0); // Vertical Vector Difference

			if(diffHPositions.magnitude < 1.1) // Only move if adjacent (prevents oblique movement), sometimes is 1.000001
			{
				if(diffVPositions.y > 0) // If there was a vertical change
				{
					memberMovement.Move(new MovementItem(diffVPositions, true, true, false));
					memberMovement.Move(new MovementItem(diffHPositions, true, true, false));
				}	
				else if(diffVPositions.y < 0) // If there was a vertical change
				{
					memberMovement.Move(new MovementItem(diffHPositions, true, true, false));
					memberMovement.Move(new MovementItem(diffVPositions, true, true, false));
				}
				else
					memberMovement.Move(new MovementItem(diffHPositions, true, true, false));

				previousPosition = memberMovement.savedPosition; // EUREKA save the position vefore the animation so you can spaaaam
				memberMovement.savedPosition = memberMovement.savedPosition + diffHPositions + diffVPositions;

				/* HAS A BUG IF GOING DOWN FAST */
			}
		}
	}
}