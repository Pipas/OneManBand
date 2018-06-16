using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsDifficulty : MonoBehaviour {
    public GameObject mainMenu;
    public GameObject settings;

    public GameObject optionsDefault;
    public GameObject optionsSound;
    public GameObject optionsDifficulty;

    public GameObject easy;
    public GameObject medium;
    public GameObject hard;

    public void SetSoundFromDifficultySettings()
    {
        optionsDifficulty.SetActive(false);
        optionsSound.SetActive(true);
    }

    public void changeDifficulty()
    {
        if (easy.activeSelf)
        {
            easy.SetActive(false);

            StaticSettings.changeDifficulty(StaticSettings.MEDIUM);
            medium.SetActive(true);
        }
        else if (medium.activeSelf)
        {
            medium.SetActive(false);

            StaticSettings.changeDifficulty(StaticSettings.HARD);
            hard.SetActive(true);
        }
        else if (hard.activeSelf)
        {
            hard.SetActive(false);

            StaticSettings.changeDifficulty(StaticSettings.EASY);
            easy.SetActive(true);
        }
    }

    public void BackFromDifficultySettings()
    {
        optionsDifficulty.SetActive(false);
        settings.SetActive(false);

        mainMenu.SetActive(true);
    }
}
