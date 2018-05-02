using UnityEngine;

public class Skill {

    /* --- Attrs --- */

    // game object of this skill
    private GameObject gameObj;

    // render of this skill
    private CanvasRenderer cRend;

    // default alpha value
    private readonly float DEFAULT_ALPHA;


    /* --- Methods --- */

    // constructor
    public Skill(Transform parent, string name) {
        gameObj = parent.Find(name).gameObject;
        cRend = gameObj.GetComponent<CanvasRenderer>();
        DEFAULT_ALPHA = cRend.GetAlpha();
    }


    // resets alpha to default value
    public void ResetAlpha() {
        cRend.SetAlpha(DEFAULT_ALPHA);
    }

    // setter
    public void SetAlpha(float alpha) {
        cRend.SetAlpha(alpha);
    }
}