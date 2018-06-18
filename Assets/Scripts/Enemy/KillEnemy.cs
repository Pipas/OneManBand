using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemy : MonoBehaviour {
    public GameObject enemy;

    private bool enemyDead = false;
    /*private Color alphaColor;
    private float timeToFade = 1.0f;*/

	// Use this for initialization
	void Start () {
        /*alphaColor = enemy.GetComponent<MeshRenderer>().material.color;
        alphaColor.a = 0;*/
	}
	
	// Update is called once per frame
	void Update () {
        /*if (enemyDead)
        {
            enemy.GetComponent<MeshRenderer>().material.color = Color.Lerp(enemy.GetComponent<MeshRenderer>().material.color, alphaColor, timeToFade * Time.deltaTime);
        }*/
	}

    public bool isEnemyDead()
    {
        return enemyDead;
    }

    public void setEnemyDead(bool dead)
    {
        enemyDead = dead;
        Destroy(enemy);
    }
}
