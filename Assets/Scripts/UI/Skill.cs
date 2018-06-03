using UnityEngine;

public class Skill {

    /* --- Attrs --- */

    // game object of this skill
    private GameObject gameObj;

    // render of this skill
    private CanvasRenderer cRend;

    // audio to play when skill is pressed
    private AudioSource sound;

    // default alpha value
    private readonly float DEFAULT_ALPHA;


    /* --- Methods --- */

    // constructor
    public Skill(Transform parent, string name) {
        gameObj = parent.Find(name).gameObject;
        cRend = gameObj.GetComponent<CanvasRenderer>();
        DEFAULT_ALPHA = cRend.GetAlpha();
        sound = gameObj.GetComponent<AudioSource>();
    }

    // activates this skill
    public void Activate(float alpha) {
        cRend.SetAlpha(alpha);
        PlaySound();
    }

    // deactivates this skill
    public void Deactivate() {
        cRend.SetAlpha(DEFAULT_ALPHA);
    }

    // plays sound
    private void PlaySound() {
        if (sound != null) {
            sound.Play();
        }        
    }
}