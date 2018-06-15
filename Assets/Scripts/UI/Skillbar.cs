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

    public GameObject player;
    private bool maestroPlaying = false;

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

    // previous logged time
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
            s1.Activate(KeyDownAlpha);
            pressedSkill = "f";
            
            maestroPlaying = true;
            player.GetComponent<Animator>().SetBool("PlayToggle", true);
        }
        else if (Input.GetKeyUp(KC_SKILL1))
        {
            s1.Deactivate();
        }

        // skill #2
        if (Input.GetKeyDown(KC_SKILL2))
        {
            s2.Activate(KeyDownAlpha);
            pressedSkill = "s";

            maestroPlaying = true;
            player.GetComponent<Animator>().SetBool("PlayToggle", true);
        }
        else if (Input.GetKeyUp(KC_SKILL2))
        {
            s2.Deactivate();
        }

        // skill #3
        if (Input.GetKeyDown(KC_SKILL3))
        {
            s3.Activate(KeyDownAlpha);
            pressedSkill = "t";

            maestroPlaying = true;
            player.GetComponent<Animator>().SetBool("PlayToggle", true);
        }
        else if (Input.GetKeyUp(KC_SKILL3))
        {
            s3.Deactivate();
        }

        if (!Input.GetKey(KC_SKILL1) && !Input.GetKey(KC_SKILL2) && !Input.GetKey(KC_SKILL3))
        {
            maestroPlaying = false;
            player.GetComponent<Animator>().SetBool("PlayToggle", false);
        }

        handleSkillPress(pressedSkill);
    }


    // handles a skill press
    private void handleSkillPress(string pressedSkill) {

        if (pressedSkill == "") {
            return;
        }

        long elapsedTime = MyTime.ElapsedTime(previousTime);
        previousTime = MyTime.CurrentTimeMillis();

        if (elapsedTime > MaxPauseDuration)
        {
            pressedSkills.Clear();
        }

        pressedSkills.Add(new SkillPress(pressedSkill, elapsedTime));

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] spotlights = GameObject.FindGameObjectsWithTag("Spotlight");

        GameObject[] triggerables = new GameObject[enemies.Length + spotlights.Length];
        Array.Copy(enemies, triggerables, enemies.Length);
        Array.Copy(spotlights, 0, triggerables, enemies.Length, spotlights.Length);


        foreach(GameObject obj in triggerables) {
            
            double dist = Vector3.Distance(Player.transform.position, obj.transform.position);

            if (dist <= Radius) {
                Melody objMelody = obj.GetComponent<Melody>();
                objMelody.Stop(true);
                if (checkMelody(objMelody.rythem))
                {
                    if (obj.tag == "Enemy")
                    {
                        obj.transform.parent.gameObject.GetComponent<KillEnemy>().setEnemyDead(true);
                    } else if (obj.tag == "Spotlight") {
                        GameObject movBlock = obj.GetComponent<Spotlight>().movingBlock;
                        movBlock.GetComponent<AscendingObject>().Trigger();
                    }
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
}
