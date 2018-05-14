using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skillbar : MonoBehaviour {

    /* --- Inspector --- */

    // keycode for skill #1
    public KeyCode KC_SKILL1;

    // keycode for skill #2
    public KeyCode KC_SKILL2;

    // keycode for skill #3
    public KeyCode KC_SKILL3;

    // alpha of skills during keydown
    public float KeyDownAlpha;
    

    /* --- Attrs --- */

    // skill #1
    private Skill s1;

    // skill #2
    private Skill s2;

    // skill #3
    private Skill s3;


    /* --- Methods --- */

    void Start ()
    {
        s1 = new Skill(transform, "1");
        s2 = new Skill(transform, "2");
        s3 = new Skill(transform, "3");
    }
    

    void Update () {
        processInput();
    }


    void processInput()
    {
        // skill #1
        if (Input.GetKeyDown(KC_SKILL1))
        {
            s1.SetAlpha(KeyDownAlpha);
        }
        else if (Input.GetKeyUp(KC_SKILL1))
        {
            s1.ResetAlpha();
        }

        // skill #2
        if (Input.GetKeyDown(KC_SKILL2))
        {
            s2.SetAlpha(KeyDownAlpha);
        }
        else if (Input.GetKeyUp(KC_SKILL2))
        {
            s2.ResetAlpha();
        }

        // skill #3
        if (Input.GetKeyDown(KC_SKILL3))
        {
            s3.SetAlpha(KeyDownAlpha);
        }
        else if (Input.GetKeyUp(KC_SKILL3))
        {
            s3.ResetAlpha();
        }
    }
}
