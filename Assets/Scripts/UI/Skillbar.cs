using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class Skillbar : MonoBehaviour {

    private struct SkillPress {

        public string skill;
        public long time;

        public SkillPress(string skill, long time) {
            this.skill = skill;
            this.time = time;
        }
    }


    /* --- Inspector --- */

    // keycode for skill #1
    public KeyCode KC_SKILL1;

    // keycode for skill #2
    public KeyCode KC_SKILL2;

    // keycode for skill #3
    public KeyCode KC_SKILL3;

    // alpha of skills during keydown
    public float KeyDownAlpha;

    // player object
    public GameObject Player;

    // radius in which the player can attack
    public double Radius;

    // margin (ms) skill press needs to match melody
    public int SkillPressMargin;

    // the max duration a pause can have in the melody
    public int MaxPauseDuration;
    

    /* --- Attrs --- */

    // skill #1
    private Skill s1;

    // skill #2
    private Skill s2;

    // skill #3
    private Skill s3;

    // previous logled time
    private long previousTime;

    // list of pressed skills
    private List<SkillPress> pressedSkills;


    /* --- Methods --- */

    void Start ()
    {
        s1 = new Skill(transform, "1");
        s2 = new Skill(transform, "2");
        s3 = new Skill(transform, "3");
        previousTime = 0;
        pressedSkills = new List<SkillPress>();
    }
    

    void Update () {
        processInput();
    }


    void processInput()
    {

        string pressedSkill = "";

        // skill #1
        if (Input.GetKeyDown(KC_SKILL1))
        {
            s1.SetAlpha(KeyDownAlpha);
            pressedSkill = "f";
        }
        else if (Input.GetKeyUp(KC_SKILL1))
        {
            s1.ResetAlpha();
        }

        // skill #2
        if (Input.GetKeyDown(KC_SKILL2))
        {
            s2.SetAlpha(KeyDownAlpha);
            pressedSkill = "s";
        }
        else if (Input.GetKeyUp(KC_SKILL2))
        {
            s2.ResetAlpha();
        }

        // skill #3
        if (Input.GetKeyDown(KC_SKILL3))
        {
            s3.SetAlpha(KeyDownAlpha);
            pressedSkill = "t";
        }
        else if (Input.GetKeyUp(KC_SKILL3))
        {
            s3.ResetAlpha();
        }

        handleSkillPress(pressedSkill);
    }


    // handles a skill press
    private void handleSkillPress(string pressedSkill) {

        if (pressedSkill == "") {
            return;
        }

        long elapsedTime = getElapsedTime();
        
        if (elapsedTime > MaxPauseDuration)
        {
            Debug.Log("Memory cleared!");
            pressedSkills.Clear();
        }

        pressedSkills.Add(new SkillPress(pressedSkill, elapsedTime));

        GameObject[] triggerables = GameObject.FindGameObjectsWithTag("Triggerable");

        foreach(GameObject obj in triggerables) {
            
            double dist = Vector3.Distance(Player.transform.position, obj.transform.position);

            if (dist <= Radius) {
                if (checkMelody(obj.GetComponent<Melody>().melody))
                {
                    Debug.Log("Attack!");
                }
            }
        }
    }


    // checks if the given melody matches the pressed skills
    private bool checkMelody(string strMelody) {

        var m = Regex.Match(strMelody, @"^([fst])(\d+[fst])*$");
        if (m.Success)
        {
            // parse melody
            List<SkillPress> melody = new List<SkillPress>();
            melody.Add(new SkillPress(m.Groups[1].Value, 0));
            foreach (string g in m.Groups[2].Captures.Cast<Capture>().Select(t => t.Value))
            {
                melody.Add(new SkillPress("" + g[g.Length - 1], Convert.ToInt64(g.Substring(0, g.Length - 1))));
            }

            // no need to check if there are not enough presses
            if (pressedSkills.Count < melody.Count)
            {
                return false;
            }

            // check if input matches melody (backwards)
            for (int i = 1; i <= melody.Count; i++)
            {
                SkillPress melodySP = melody[melody.Count - i];
                SkillPress inputSP = pressedSkills[pressedSkills.Count - i];

                if (inputSP.skill == melodySP.skill)
                {
                    if ((i != melody.Count && i != pressedSkills.Count) &&
                        !(inputSP.time >= melodySP.time - SkillPressMargin && inputSP.time <= melodySP.time + SkillPressMargin))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;         
        }

        return false;
    }


    // returns elapsed time and updates previous time
    private long getElapsedTime()
    {
        long currentTime = currentTimeMillis();
        long elapsedTime = 0;
        if (previousTime != 0)
        {
            elapsedTime = currentTime - previousTime;
        }
        previousTime = currentTimeMillis();
        return elapsedTime;
    }
    

    private long currentTimeMillis()
    {
        return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }
}
