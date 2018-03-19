using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement 
{	
	private List<GameObject> party = new List<GameObject>();
	void Update ()
	{
		handleMovement();
		
			if(Input.GetKeyDown(KeyCode.UpArrow))
				Move(Vector3.forward);
			if(Input.GetKeyDown(KeyCode.DownArrow))
				Move(Vector3.back);
			if(Input.GetKeyDown(KeyCode.LeftArrow))
				Move(Vector3.left);
			if(Input.GetKeyDown(KeyCode.RightArrow))
				Move(Vector3.right);
		
	}
}
