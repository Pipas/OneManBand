using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class SettingsSound : MonoBehaviour {
    public GameObject mainMenu;
    public GameObject settings;

    public GameObject optionsDefault;
    public GameObject optionsSound;
    public GameObject optionsDifficulty;

    public GameObject easy;
    public GameObject medium;
    public GameObject hard;

    public Slider volumeSlider;
    private float bgmVolume = StaticSettings.volumeBGM;

    void Start()
    {
        volumeSlider.value = bgmVolume;
    }

    public void changeBGMVolume()
    {
        StaticSettings.changeVolumeBGM(volumeSlider.value);
    }

    public void SetDifficultyFromSoundSettings()
    {
        checkDifficulty();

        optionsSound.SetActive(false);
        optionsDifficulty.SetActive(true);
    }

    public void BackFromSoundSettings()
    {
        optionsSound.SetActive(false);
        settings.SetActive(false);

        mainMenu.SetActive(true);
    }

    public void checkDifficulty()
    {
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
