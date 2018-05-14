using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour 
{
    public bool isMoving = false;
    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;
    private Vector3 initPosition, endPosition;

    void Update()
    {
        if(isMoving)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(initPosition, endPosition, fracJourney);
            if(fracJourney >= 1)
                isMoving = false;
        }
    }

    public void MoveCamera(Vector3 deltaMove)
    {
        startTime = Time.time;
        initPosition = transform.position;
        endPosition = initPosition + deltaMove;
        journeyLength = Vector3.Distance(initPosition, endPosition);
        isMoving = true;
    }
}
