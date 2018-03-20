using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement 
{	
	private List<GameObject> party = new List<GameObject>();
	void Update ()
	{	
		if(!isMoving)
		{
			if(Input.GetKey(KeyCode.UpArrow))
				Move(Vector3.forward);
			if(Input.GetKey(KeyCode.DownArrow))
				Move(Vector3.back);
			if(Input.GetKey(KeyCode.LeftArrow))
				Move(Vector3.left);
			if(Input.GetKey(KeyCode.RightArrow))
				Move(Vector3.right);
		}

		handleMovement();	
	}
}
