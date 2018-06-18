using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemy : MonoBehaviour {
    public GameObject enemy;
    public const string BOSS_NAME = "EvilPianoPivot";
	public BossHealth bossHealth;

    private bool enemyDead = false;

    public bool isEnemyDead()
    {
        return enemyDead;
    }

    public void setEnemyDead(bool dead)
    {
        if (this.gameObject.name != BOSS_NAME)
        {
			enemyDead = dead;
            Destroy(enemy);
		} else {
			bossHealth.TakeDamage(-1);
		}
    }
}
