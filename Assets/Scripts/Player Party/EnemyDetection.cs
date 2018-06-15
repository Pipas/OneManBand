using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {
    public EnemyHitbox enemyHitbox;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyHitbox.enemyMovement.stopEnemyAnimation();
            enemyHitbox.setPlayerWithinRange(true);

            if (enemyHitbox.isOnCooldown())
            {
                enemyHitbox.decreaseCooldown();
            }
            else
            {
                enemyHitbox.attackPlayer();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        enemyHitbox.setPlayerWithinRange(false);
    }
}
