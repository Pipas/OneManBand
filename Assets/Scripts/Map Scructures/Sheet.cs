using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheet : MonoBehaviour 
{
    public float rotationSpeed;
    public SheetBar sheetBar;
    public bool caught = false;

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
        caught = true;
        Destroy(transform.parent.gameObject);
        sheetBar.catchSheet();
        BGM.FoundSheet();
    }

    public bool isCaught() {
        return caught;
    }
}
