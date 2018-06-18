using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public GameObject tutorial;
    public GameObject menu;

    public void GoBackToMenu()
    {
        tutorial.SetActive(false);
        menu.SetActive(true);
    }
}
