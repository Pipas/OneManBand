using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour 
{
	private Tile GetTile(Vector3 direction)
    {
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(transform.position, direction, out hit, 1))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

	public void Move(Vector3 direction)
	{
		if(GetTile(direction) == null)
		{
			transform.Translate(direction);
		}
	}
}
