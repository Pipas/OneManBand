using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscendingObject : MonoBehaviour, Triggerable 
{
    public enum State { retracted, extending, retracting, extended }
    public State state;
    private Vector3 initPosition, endPosition;
    private float travelLength, startTime;
    private GameObject aboveCollider;
    private List<Collider> colliders;
    private bool hasPlayer = false;
    public Vector3 deltaMove;
    public float speed;
    public int waitTime;
    private int counter = -1, lastTime = 0;
    void Start () 
    {
        initPosition = transform.position;
        endPosition = initPosition + deltaMove;
        travelLength = Vector3.Distance(initPosition, endPosition);
        colliders = new List<Collider>();
        aboveCollider = transform.GetChild(0).gameObject;
    }
    
    // Update is called once per frame
    void Update () 
    {
        if(state == State.extending)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / travelLength;
            Vector3 deltaTransform = Vector3.Lerp(initPosition, endPosition, fracJourney) - transform.position;
            transform.Translate(deltaTransform);
            TranslateObjectsAbove(deltaTransform);
            if(fracJourney >= 1)
            {
                aboveCollider.layer = 2;
                state = State.extended;
            }
            
        }
        else if (state == State.retracting)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / travelLength; 
            Vector3 deltaTransform = Vector3.Lerp(endPosition, initPosition, fracJourney) - transform.position;
            transform.Translate(deltaTransform);
            TranslateObjectsAbove(deltaTransform);
            if(fracJourney >= 1)
            {
                aboveCollider.layer = 2;
                state = State.retracted;
            }
        }

        if(waitTime != 0 && state == State.extended)
        {
            int currentTime = (int) Time.time;

            if (currentTime != lastTime && currentTime % 1 == 0 && state == State.extended)
                counter++;

            if (counter >= waitTime)
            {
                Trigger();
                counter = -1;
            }

            lastTime = currentTime;
        }
    }
    private void TranslateObjectsAbove(Vector3 deltaTransform)
    {
        foreach (Collider collider in colliders)
        {
            if (collider.name == "Player") {
                if (GameObject.Find("PlayerPivot").GetComponent<Movement>().state != Movement.State.falling)
                    collider.transform.Translate(deltaTransform);
            }
            else if (collider.name != "Block")
            {
                if (collider.GetComponent<Movement>().state != Movement.State.falling)
                    collider.transform.Translate(deltaTransform);
            }
        }
    }

    public void AddObject(Collider obj)
    {
        colliders.Add(obj);

        if (obj.tag == "Player")
            hasPlayer = true;
    }

    public void RemoveObject(Collider obj)
    {
        colliders.Remove(obj);

        if (obj.tag == "Player")
            hasPlayer = false;
    }

    public void Trigger()
    {
        if(state == State.retracted)
        {
            state = State.extending;
            startTime = Time.time;
        }
        else if(state == State.extended)
        {
            state = State.retracting;
            startTime = Time.time;
        }

        aboveCollider.layer = 0;

        checkPartyBreak();
    }

    private void checkPartyBreak()
    {
        if(hasPlayer)
        {
            for (int i = Movement.party.Count - 1; i >= 0; i--)
            {
                if(!colliders.Contains(Movement.party[i].GetComponent<Collider>()))
                {
                    if(i == 0)
                        GameObject.Find("PlayerPivot").GetComponent<PlayerMovement>().nextInParty = null;
                    else
                        Movement.party[i - 1].GetComponent<PartyMovement>().nextInParty = null;
                    Movement.party.Remove(Movement.party[i]);
                }
                    
            }
        }
    }
}