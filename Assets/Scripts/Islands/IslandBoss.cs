using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandBoss : MonoBehaviour
{
    public float islandPosition = 80f;
    public GameObject player;
    public GameObject bossHealth;

	// Update is called once per frame
	void Update () {
        if (player.transform.position.x >= 80f) {
            bossHealth.SetActive(true);
        } else {
            bossHealth.SetActive(false);
        }
	}

}
