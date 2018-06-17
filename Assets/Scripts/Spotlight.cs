using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlight : MonoBehaviour 
{
    private Light spotlight;
    private float max_intensity = 5f;
    private float min_intensity = 2f;
    private float delta_intensity = 1.5f;
    private float widen_speed = 120f;
    private float currentDelta;
    private Light directionalLight;
    public bool hasPlayer;
    private bool retracting = false;
    public GameObject movingBlock;
    public Collider player;

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

            if(directionalLight.intensity > 0f)
                directionalLight.intensity -= Time.deltaTime * 5;
        }
        else if(retracting)
        {
            if(spotlight.spotAngle > 18f)
                spotlight.spotAngle -= Time.deltaTime * widen_speed;
            else
                retracting = false;

            if(directionalLight.intensity < 1f)
                directionalLight.intensity += Time.deltaTime * 5;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player" || other.tag == "Party")
        {
            hasPlayer = true;
            player = other;
            Skillbar.SilenceAllExcept(other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(player == other)
        {
            hasPlayer = false;
            retracting = true;
            player = null;
            Skillbar.RestoreAllVolume();
        }
    }
}
