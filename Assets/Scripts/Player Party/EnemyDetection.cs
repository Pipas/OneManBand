using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {
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
            EnemyHitbox hitbox = other.transform.parent.gameObject.GetComponent<EnemyHitbox>();

            hitbox.enemyMovement.stopEnemyAnimation();
            hitbox.setPlayerWithinRange(true);

            if (hitbox.isOnCooldown())
            {
                hitbox.decreaseCooldown();
            }
            else
            {
                hitbox.attackPlayer();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyHitbox hitbox = other.transform.parent.gameObject.GetComponent<EnemyHitbox>();
            hitbox.setPlayerWithinRange(false);
        }
    }
}
