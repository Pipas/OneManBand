using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour {
    public GameObject menuPanel;
    public HealthSystem healthSystem;

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
                    Time.timeScale = 0;
                }
                else
                {
                    stopGame = false;
                    menuPanel.SetActive(false);
                    Time.timeScale = 1;
                }
            }
        }
	}

    public void Continue()
    {
        stopGame = false;
        menuPanel.SetActive(false);
        Time.timeScale = 1;
    }


}
