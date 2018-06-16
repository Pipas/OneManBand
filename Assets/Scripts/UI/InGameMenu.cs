using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour {
    public GameObject menuPanel;
    public HealthSystem healthSystem;
    public Skillbar skillbar;
    public BGM bgm;

    public GameObject mainMenu;
    public GameObject settings;
    public GameObject settingsDefault;
    public GameObject settingsSound;
    public GameObject settingsDifficulty;

    private bool stopGame = false;
    private const float FULL_OPACITY = 255f;
	
	// Update is called once per frame
	void Update () {
        if (!healthSystem.gameOver)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (!menuPanel.activeSelf)
                {
                    stopGame = true;
                    menuPanel.SetActive(true);
                    mainMenu.SetActive(true);
                    Time.timeScale = 0;
                }
                else
                {
                    stopGame = false;
                    menuPanel.SetActive(false);
                    mainMenu.SetActive(false);

                    settings.SetActive(false);
                    settingsDefault.SetActive(false);
                    settingsSound.SetActive(false);
                    settingsDifficulty.SetActive(false);

                    Time.timeScale = 1;

                    int tmpSkillVal = StaticSettings.setSkillMargin();
                    skillbar.updateSkillMargin(tmpSkillVal);

                    bgm.updateBGMVolume(StaticSettings.volumeBGM);
                }
            }
        }
	}

    public void Continue()
    {
        stopGame = false;
        menuPanel.SetActive(false);
        Time.timeScale = 1;

        int tmpSkillVal = StaticSettings.setSkillMargin();
        skillbar.updateSkillMargin(tmpSkillVal);

        bgm.updateBGMVolume(StaticSettings.volumeBGM);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Movement.party = new List<GameObject>();

        int tmpSkillVal = StaticSettings.setSkillMargin();
        skillbar.updateSkillMargin(tmpSkillVal);

        bgm.updateBGMVolume(StaticSettings.volumeBGM);
    }

    public void Settings()
    {
        mainMenu.SetActive(false);
        settingsDefault.SetActive(true);
        settings.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public bool isGameStopped()
    {
        return stopGame;
    }
}
