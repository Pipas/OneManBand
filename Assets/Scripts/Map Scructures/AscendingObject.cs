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
    public List<Collider> colliders;
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
            Movement colliderMovement = null;
            if(collider.tag == "Player")
                colliderMovement = collider.transform.parent.GetComponent<Movement>();
            else if(collider.tag == "Party")
                colliderMovement = collider.transform.GetComponent<Movement>();
            
            if(colliderMovement != null && colliderMovement.state != Movement.State.falling)
                colliderMovement.transform.Translate(deltaTransform);
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
                    Movement.party[i].GetComponent<PartyMovement>().toBeDitched = true;
                    if(!Movement.party[i].GetComponent<PartyMovement>().isMoving)
                        Movement.party[i].GetComponent<PartyMovement>().CheckIfDitched();
                }
            }
        }
        else
        {
            foreach (Collider collider in colliders)
            {
                if(collider.tag == "Party")
                {
                    for (int i = Movement.party.Count - 1; i >= 0; i--)
                    {
                        if(Movement.party[i].GetComponent<PartyMovement>().toBeDitched == false)
                        {
                            if(Movement.party[i] == collider.gameObject)
                            {
                                Movement.party[i].GetComponent<PartyMovement>().toBeDitched = true;
                                if(!Movement.party[i].GetComponent<PartyMovement>().isMoving)
                                    Movement.party[i].GetComponent<PartyMovement>().CheckIfDitched();

                                break;
                            }
                            else
                            {
                                Movement.party[i].GetComponent<PartyMovement>().toBeDitched = true;
                                if(!Movement.party[i].GetComponent<PartyMovement>().isMoving)
                                    Movement.party[i].GetComponent<PartyMovement>().CheckIfDitched();
                            }
                        }
                    }
                }
            }
        }
    }
}