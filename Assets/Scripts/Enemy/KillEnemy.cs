using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemy : MonoBehaviour {
    public GameObject enemy;
    public bool isBoss = false;
	public BossHealth bossHealth;

    private bool enemyDead = false;

    public bool isEnemyDead()
    {
        return enemyDead;
    }

    public void setEnemyDead(bool dead)
    {
        if (!isBoss)
        {
			enemyDead = dead;
            Destroy(enemy);
		} else {
			bossHealth.TakeDamage(-1);
		}
    }
}
