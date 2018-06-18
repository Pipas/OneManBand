using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {
    public GameObject mainMenu;
    public GameObject tutorial;
    public GameObject settings;
    public GameObject settingsDefault;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void seeTutorial()
    {
        mainMenu.SetActive(false);
        tutorial.SetActive(true);
    }

    public void SetSettings()
    {
        mainMenu.SetActive(false);
        settingsDefault.SetActive(true);
        settings.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
