using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class Skillbar : MonoBehaviour {
    public InGameMenu menu;

    private struct SkillPress {

        public string skill;
        public long time;

        public SkillPress(string skill, long time) {
            this.skill = skill;
            this.time = time;
        }
    }


    /* --- Inspector --- */

    private HealthSystem healthSystem;

    // keycode for skill #1
    public KeyCode KC_SKILL1;

    // keycode for skill #2
    public KeyCode KC_SKILL2;

    // keycode for skill #3
    public KeyCode KC_SKILL3;

    // keycode for skill #4
    public KeyCode KC_SKILL4;

    // alpha of skills during keydown
    public float KeyDownAlpha;

    // player object
    public GameObject Player;

    // radius in which the player can attack
    public double Radius;

    // the max duration a pause can have in the melody
    public int MaxPauseDuration;

    // margin (ms) skill press needs to match melody
    private int SkillPressMargin = StaticSettings.setSkillMargin();

    /* --- Attrs --- */

    /* Audio Source that plays the booping sound. */
    private AudioSource boopASrc;

    // skill #1
    private Skill s1;

    // skill #2
    private Skill s2;

    // skill #3
    private Skill s3;

    // skill #4
    private Skill s4;

    // previous logged time
    private long previousTime;

    // list of pressed skills
    private List<SkillPress> pressedSkills;

    private static Skillbar inst;

    private bool s1KeyUp;
    private bool s2KeyUp;
    private bool s3KeyUp;
    private bool s4KeyUp;
    

    /* --- Methods --- */

    void Start ()
    {
        healthSystem = Player.GetComponent<HealthSystem>();
        
        s1 = new Skill(transform, "1");
        s1.SetAlpha(KeyDownAlpha);
        s1KeyUp = false;
        s2 = new Skill(transform, "2");
        s2.SetAlpha(KeyDownAlpha);
        s2KeyUp = false;
        s3 = new Skill(transform, "3");
        s3.SetAlpha(KeyDownAlpha);
        s3KeyUp = false;
        s4 = new Skill(transform, "4");
        s4.SetAlpha(KeyDownAlpha);
        s4KeyUp = false;
        boopASrc = GetComponent<AudioSource>();
        previousTime = 0;
        pressedSkills = new List<SkillPress>();
        inst = this;
    }
    

    void Update () {
        if (!menu.isGameStopped() && !healthSystem.gameOver)
        {
            processInput();
        }
    }


    void processInput()
    {
        string pressedSkill = "";

        // skill #1
        if (Input.GetKeyDown(KC_SKILL1))
        {
            if (s1.Activate(KeyDownAlpha, true))
            {
                if (!s1.IsSilenced())
                {
                    pressedSkill = "f";
                }
                s1KeyUp = true;
            }
            else
            {
                boopASrc.Play();
            }

            Player.GetComponent<Animator>().SetBool("PlayToggle", true);
        }
        else if (Input.GetKeyUp(KC_SKILL1) && s1KeyUp)
        {
            s1.Deactivate();
            s1KeyUp = false;
        }

        // skill #2
        if (Input.GetKeyDown(KC_SKILL2))
        {
            if (s2.Activate(KeyDownAlpha, false))
            {
                if (!s2.IsSilenced())
                {
                    pressedSkill = "s";
                }
                s2KeyUp = true;
            }
            else
            {
                boopASrc.Play();
            }

            Player.GetComponent<Animator>().SetBool("PlayToggle", true);
        }
        else if (Input.GetKeyUp(KC_SKILL2) && s2KeyUp)
        {
            s2.Deactivate();
            s2KeyUp = false;
        }

        // skill #3
        if (Input.GetKeyDown(KC_SKILL3))
        {
            if (s3.Activate(KeyDownAlpha, false))
            {
                if (!s3.IsSilenced())
                {
                    pressedSkill = "t";
                }
                s3KeyUp = true;
            }
            else
            {
                boopASrc.Play();
            }

            Player.GetComponent<Animator>().SetBool("PlayToggle", true);
        }
        else if (Input.GetKeyUp(KC_SKILL3) && s3KeyUp)
        {
            s3.Deactivate();
            s3KeyUp = false;
        }

        // skill #4
        if (Input.GetKeyDown(KC_SKILL4))
        {
            if (s4.Activate(KeyDownAlpha, false))
            {
                if (!s4.IsSilenced())
                {
                    pressedSkill = "q";
                }
                s4KeyUp = true;
            }
            else
            {
                boopASrc.Play();
            }

            Player.GetComponent<Animator>().SetBool("PlayToggle", true);
        }
        else if (Input.GetKeyUp(KC_SKILL4) && s4KeyUp)
        {
            s4.Deactivate();
            s4KeyUp = false;
        }

        if (!Input.GetKey(KC_SKILL1) && !Input.GetKey(KC_SKILL2) && !Input.GetKey(KC_SKILL3) && !Input.GetKey(KC_SKILL4))
        {
            Player.GetComponent<Animator>().SetBool("PlayToggle", false);
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
            Vector2 playerPosition = new Vector2(Player.transform.position.x, Player.transform.position.z);
            Vector2 melodyPosition = new Vector2(obj.transform.position.x, obj.transform.position.z);

            double dist = Vector2.Distance(playerPosition, melodyPosition);

            if ((dist <= Radius && obj.tag != "Spotlight") ||
                (obj.tag == "Spotlight" && obj.GetComponent<Spotlight>().hasPlayer))
            {
                Melody objMelody = obj.GetComponent<Melody>();
                objMelody.Wait();
                if (checkMelody(objMelody.rythem) && objMelody.IsValid())
                {
                    if (obj.tag == "Enemy")
                    {
                        obj.transform.parent.gameObject.GetComponent<KillEnemy>().setEnemyDead(true);
                    }
                    else if (obj.tag == "Spotlight")
                    {
                        GameObject movBlock = obj.GetComponent<Spotlight>().movingBlock;
                        movBlock.GetComponent<AscendingObject>().Trigger();
                    }
                }
            }
        }
    }


    // checks if the given melody matches the pressed skills
    private bool checkMelody(string strMelody) {

        var m = Regex.Match(strMelody, @"^([fstq])(\d+[fstq])*$");
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

    public void updateSkillMargin(int skillValue)
    {
        this.SkillPressMargin = skillValue;
    }

    public static void ActivateSkill(string name)
    {
        if (name == "PartyTambor")
        {
            inst.s1.Deactivate();
        }
        else
        {
            inst.s2.Deactivate();
            inst.s3.Deactivate();
            inst.s4.Deactivate();
        }
    }

    public static void DeactivateSkill(string name)
    {
        if (name == "PartyTambor")
        {
            inst.s1.SetAlpha(inst.KeyDownAlpha);
            inst.s1KeyUp = false;
        }
        else
        {
            if (Movement.party.Count == 0 || (Movement.party.Count == 1 && Movement.party[0].name == "PartyTambor"))
            {
                inst.s2.SetAlpha(inst.KeyDownAlpha);
                inst.s2KeyUp = false;
                inst.s3.SetAlpha(inst.KeyDownAlpha);
                inst.s3KeyUp = false;
                inst.s4.SetAlpha(inst.KeyDownAlpha);
                inst.s4KeyUp = false;
            }
        }
    }

    public static void SilenceAllExcept(string name)
    {
        // reset
        RestoreAllVolume();
        
        switch (name)
        {
            case "PartyTambor":
                inst.s2.Silence();
                inst.s3.Silence();
                inst.s4.Silence();
                inst.s2.SilenceSecond();
                inst.s3.SilenceSecond();
                inst.s4.SilenceSecond();
                break;
            
            case "Player":
                RestoreAllVolume();
                break;

            case "PartyGuitar":
                inst.s1.Silence();
                inst.s2.SilenceSecond();
                inst.s3.SilenceSecond();
                inst.s4.SilenceSecond();
                break;

            case "PartyPiano":
                inst.s1.Silence();
                inst.s2.Silence();
                inst.s3.Silence();
                inst.s4.Silence();
                break;
                
            default:
                break;
        }
    }

    public static void RestoreAllVolume()
    {
        inst.s1.RestoreVolume();
        inst.s2.RestoreVolume();
        inst.s3.RestoreVolume();
        inst.s4.RestoreVolume();
    }
}
