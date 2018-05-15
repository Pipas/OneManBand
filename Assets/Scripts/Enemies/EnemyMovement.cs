using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    public Transform[] points;
    private float speed = 1.5f;
    private int currentPoint = 0;

	// Use this for initialization
	void Start () {
        GetComponent<Animator>().SetBool("RunToggle", true);
	}
	
	// Update is called once per frame
	void Update () {
        if (currentPoint < points.Length) // this is to check if it's less than the length of the points
        {
            float dist = Vector3.Distance(transform.position, points[currentPoint].position);

            if (dist < 0.1f)
            {
                currentPoint++;
            }
            else
            {
                transform.LookAt(points[currentPoint].position);
                transform.position = Vector3.Lerp(this.transform.position, points[currentPoint].position, Time.deltaTime * speed);
            }
        }
        else
        {
            currentPoint = 0; //this is to loop it back to zero again
        }
	}


}
