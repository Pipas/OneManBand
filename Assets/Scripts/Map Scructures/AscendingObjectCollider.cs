using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscendingObjectCollider : MonoBehaviour 
{
    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log(other.gameObject.name);

        transform.parent.GetComponent<AscendingObject>().AddObject(other);
    }

    private void OnTriggerExit(Collider other) 
    {
        transform.parent.GetComponent<AscendingObject>().RemoveObject(other);
    }
}
