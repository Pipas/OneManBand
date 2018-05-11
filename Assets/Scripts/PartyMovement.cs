using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMovement : Movement
{
    public bool toBeDitched = false;

    void Start ()
    {
        savedPosition = transform.position;
        state = State.still;
    }
    
    void Update ()
    {

    }

    public override void CheckIfDitched()
    {
        if(toBeDitched)
        {
            if(party.IndexOf(gameObject) == 0)
            {
                GameObject.Find("Player").GetComponent<PlayerMovement>().nextInParty = null;
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
}
