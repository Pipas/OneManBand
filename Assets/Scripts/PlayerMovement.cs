using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement 
{	
	private Rigidbody playerRB;
	private Vector3 startPosition;
	void Start ()
	{
		playerRB = GetComponent<Rigidbody>();
		startPosition = transform.position;
	}

	void Update ()
	{
		if(playerRB.velocity == Vector3.zero)
		{
			if(Input.GetKeyDown(KeyCode.W))
				Move(Vector3.forward);
			else if(Input.GetKeyDown(KeyCode.S))
				Move(Vector3.back);
			else if(Input.GetKeyDown(KeyCode.A))
				Move(Vector3.left);
			else if(Input.GetKeyDown(KeyCode.D))
				Move(Vector3.right);
		}

		if(transform.position.y < -10)
		{
			ResetPlayer();
		}
	}

	void ResetPlayer()
	{
		playerRB.velocity = Vector3.zero;
		transform.position = startPosition;
	}
}
