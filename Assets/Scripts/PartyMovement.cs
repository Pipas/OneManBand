using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMovement : Movement
{
	void Start ()
	{
		savedPosition = transform.position;
	}
	
	void Update ()
	{

	}

	protected override bool HandleObstacle(Vector3 direction)
	{
		return true;
	}
}
