using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMovement : Movement
{
    public bool toBeDitched = false;
    private Vector3 playerDirection;
    private bool running = false;

    void Start ()
    {
        savedPosition = transform.position;
        state = State.still;
    }
    
    void LateUpdate ()
    {
        if (playerDirection == Vector3.forward)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        }
        else if (playerDirection == Vector3.back)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        }
        else if (playerDirection == Vector3.right)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        }
        else if (playerDirection == Vector3.left)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, -90, transform.eulerAngles.z);
        }

        if (transform.hasChanged)
        {
            transform.hasChanged = false;
            running = true;
            movePartyAnimation();
        }
        else
        {
            running = false;
            stopPartyAnimation();
        }
    }

    public void movePartyAnimation()
    {
        if (running)
        {
            transform.GetComponent<Animator>().SetBool("moveToggle", true);
            running = true;
        }
    }

    public void stopPartyAnimation()
    {
        if (!running)
        {
            transform.GetComponent<Animator>().SetBool("moveToggle", false);
            running = false;
        }
    }

    public override void CheckIfDitched()
    {
        if(toBeDitched)
        {
            GetComponent<BoxCollider>().enabled = true;
            if(party.IndexOf(gameObject) == 0)
            {
                GameObject.Find("PlayerPivot").GetComponent<PlayerMovement>().nextInParty = null;
                party.RemoveAt(0);
            }
            else
            {
                party[party.IndexOf(gameObject) - 1].GetComponent<PartyMovement>().nextInParty = null;
                party.RemoveAt(party.IndexOf(gameObject));
            }
            toBeDitched = false;
        }
    }

    public void setPlayerDirection(Vector3 direction) {
        this.playerDirection = direction;
    } 
}
