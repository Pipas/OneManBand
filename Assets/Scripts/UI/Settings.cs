using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {
    public GameObject mainMenu;
    public GameObject settings;

    public GameObject optionsDefault;
    public GameObject optionsSound;
    public GameObject optionsDifficulty;

    public GameObject easy;
    public GameObject medium;
    public GameObject hard;

    public void SetSoundSettings()
    {
        optionsDefault.SetActive(false);
        optionsSound.SetActive(true);
    }

    public void SetDifficultySettings()
    {
        checkDifficulty();

        optionsDefault.SetActive(false);
        optionsDifficulty.SetActive(true);
    }

    public void BackToMenu()
    {
        optionsDefault.SetActive(false);
        settings.SetActive(false);

        mainMenu.SetActive(true);
    }

    public void checkDifficulty() {
        if (StaticSettings.difficulty == StaticSettings.EASY)
        {
            easy.SetActive(true);

            medium.SetActive(false);
            hard.SetActive(false);
        }
        else if (StaticSettings.difficulty == StaticSettings.MEDIUM)
        {
            medium.SetActive(true);

            easy.SetActive(false);
            hard.SetActive(false);
        }
        else if (StaticSettings.difficulty == StaticSettings.HARD)
        {
            hard.SetActive(true);

            easy.SetActive(false);
            medium.SetActive(false);
        }
    }
}
