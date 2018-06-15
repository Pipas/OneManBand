using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlight : MonoBehaviour 
{
    private Light spotlight;
    private float max_intensity = 0.6f;
    private float min_intensity = 0.3f;
    private float delta_intensity = 0.15f;
    private float widen_speed = 25f;
    private float currentDelta;
    private Light directionalLight;
    public bool hasPlayer, retracting = false;
    public GameObject movingBlock;

	// Use this for initialization
	void Start () 
    {
		spotlight = GetComponent<Light>();
        spotlight.intensity = min_intensity;
        currentDelta = delta_intensity;
        directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () 
    {
		IdleDimming();
	}

    private void IdleDimming()
    {
        if(currentDelta > 0)
        {
            if(spotlight.intensity < max_intensity)
                spotlight.intensity += currentDelta * Time.deltaTime;
            else
                currentDelta = -currentDelta;
        }
        else
        {
           if(spotlight.intensity > min_intensity)
                spotlight.intensity += currentDelta * Time.deltaTime;
            else
                currentDelta = -currentDelta; 
        }

        if(hasPlayer)
        {
            if(spotlight.spotAngle < 40f)
                spotlight.spotAngle += Time.deltaTime * widen_speed;

            if(directionalLight.intensity > 0.1f)
                directionalLight.intensity -= Time.deltaTime * 15;
        }
        else if(retracting)
        {
            if(spotlight.spotAngle > 29f)
                spotlight.spotAngle -= Time.deltaTime * widen_speed;
            else
                retracting = false;

            if(directionalLight.intensity < 1f)
                directionalLight.intensity += Time.deltaTime * 15;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            min_intensity = 0.6f;
            max_intensity = 2f;
            hasPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.tag == "Player")
        {
            min_intensity = 0.3f;
            max_intensity = 0.6f;
            hasPlayer = false;
            retracting = true;
        }
    }
}
