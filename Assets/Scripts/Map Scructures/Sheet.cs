using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheet : MonoBehaviour 
{
    public float rotationSpeed;
    public SheetBar sheetBar;

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
		IdleRotation();
	}

    private void IdleRotation () 
    {
        transform.Rotate(0, Time.deltaTime * rotationSpeed, 0, Space.World);
    }

    public void RemovePage ()
    {
        sheetBar.catchSheet();
        Destroy(transform.parent.gameObject);
    }
}
