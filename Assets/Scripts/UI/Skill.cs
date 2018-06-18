using UnityEngine;

public class Skill {

    /* --- Attrs --- */

    // game object of this skill
    private GameObject gameObj;

    // render of this skill
    private CanvasRenderer cRend;

    // audio to play when skill is pressed
    private AudioSource sound;

    // audio to play when skill is pressed
    private AudioSource secondSound;

    // default alpha value
    private readonly float DEFAULT_ALPHA;

    // default volume value
    private readonly float DEFAULT_VOLUME;


    /* --- Methods --- */

    // constructor
    public Skill(Transform parent, string name) {
        gameObj = parent.Find(name).gameObject;
        cRend = gameObj.GetComponent<CanvasRenderer>();
        DEFAULT_ALPHA = cRend.GetAlpha();
        AudioSource[] srcs = gameObj.GetComponents<AudioSource>();
        sound = srcs[0];
        if (srcs.Length > 1)
        {
            secondSound = srcs[1];
        }
        else
        {
            secondSound = null;
        }
        DEFAULT_VOLUME = sound.volume;
    }

    // activates this skill
    public bool Activate(float alpha) {

        if (cRend.GetAlpha() == DEFAULT_ALPHA)
        {
            cRend.SetAlpha(alpha);

            bool guitar = false;
            bool piano = false;

            foreach (GameObject obj in Movement.party)
            {
                if (obj.name == "PartyGuitar")
                {
                    guitar = true;
                }
                else if (obj.name == "PartyPiano")
                {
                    piano = true;
                }
            }

            if (piano && guitar)
            {
                PlaySound();
                PlaySecondSound();
            }
            else if (piano)
            {
                PlaySecondSound();
            }
            else {
                PlaySound();
            }

            return true;
        }

        return false;
    }

    // deactivates this skill
    public void Deactivate() {
        cRend.SetAlpha(DEFAULT_ALPHA);
    }

    // setAlpha
    public void SetAlpha(float alpha)
    {
        cRend.SetAlpha(alpha);
    }

    // plays sound
    private void PlaySound() {
        if (sound != null) {
            sound.Play();
        }        
    }

    // plays sound
    private void PlaySecondSound()
    {
        if (secondSound != null)
        {
            secondSound.Play();
        }
    }

    public void Silence() {
        sound.volume = (float) (DEFAULT_VOLUME / 5.0);
    }

    public void SilenceSecond()
    {
        if (secondSound != null)
        {
            secondSound.volume = (float)(DEFAULT_VOLUME / 5.0);
        }
    }

    public bool IsSilenced() {
        if (secondSound != null)
        {
            return (sound.volume < DEFAULT_VOLUME && secondSound.volume < DEFAULT_VOLUME);
        }
        else
        {
            return (sound.volume < DEFAULT_VOLUME);
        }        
    }

    public void RestoreVolume() {
        sound.volume = DEFAULT_VOLUME;
        if (secondSound != null)
        {
            secondSound.volume = DEFAULT_VOLUME;
        }
    }
}