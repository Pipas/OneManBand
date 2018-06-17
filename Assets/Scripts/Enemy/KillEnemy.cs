using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemy : MonoBehaviour {
    public GameObject enemy;
    private bool enemyDead = false;

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
