using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

    public Animator animator;
    public bool transitionOn = false;

    private int levelToLoad = 2;

    public void FadeToLevel()
    {
        animator.SetTrigger("FadeOut");
    }

    public void onFadeComplete()
    {
        Movement.party = new List<GameObject>();
        SceneManager.LoadScene(levelToLoad);
    }

    public void OnTriggerEnter()
    {
        FadeToLevel();
    }
}
