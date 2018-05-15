using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour {
    private bool playerWithinRange = false;
    private bool onCooldown = false;
    private float attackCooldown = 3.0f;

    public HealthSystem playerHealth;
    public EnemyMovement enemyMovement;
	
	// Update is called once per frame
    void Update()
    {
        if (!playerHealth.gameOver) {
            int layerMask = 1 << 8;
            layerMask = ~layerMask;
            Vector3 origin = transform.position;
            Vector3 direction = transform.TransformDirection(Vector3.forward);
            RaycastHit hit;
        
            if (Physics.Raycast(origin, direction, out hit, 3, layerMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

                if (hit.collider.name == "Player")
                {
                    playerWithinRange = true;
                    enemyMovement.stopEnemyAnimation();
                }
                else
                {
                    playerWithinRange = false;
                }
            }
            else
            {
                playerWithinRange = false;
            }

            if (onCooldown)
            {
                decreaseCooldown();
            }
            else
            {
                attackPlayer();
            }
        }
    }

    public void decreaseCooldown()
    {
        if (attackCooldown <= 0)
        {
            attackCooldown = 3.0f;
            onCooldown = false;
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public void attackPlayer()
    {
        if (playerWithinRange)
        {
            playerHealth.TakeDamage(-1);
            onCooldown = true;
        }
    }

    public bool isPlayerWithinRange()
    {
        return playerWithinRange;
    }
}
