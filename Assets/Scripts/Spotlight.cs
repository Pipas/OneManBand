using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlight : MonoBehaviour 
{
    private Light spotlight;
    public float max_intensity;
    public float min_intensity;
    public float delta_intensity;
    private float currentDelta;

	// Use this for initialization
	void Start () 
    {
		spotlight = GetComponent<Light>();
        spotlight.intensity = min_intensity;
        currentDelta = delta_intensity;
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
    }
}
