using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour {
    private bool playerWithinRange = false;
    private float playerDistance;
    private bool onCooldown = false;
    private float attackCooldown = 3.0f;

    public HealthSystem playerHealth;
    public EnemyMovement enemyMovement;

    public Transform projectileSpawn;
    Projectile projectile;

    void Start()
    {
        projectile = Resources.Load<Projectile>("Projectile");
    }

    public void performAttack(float range)
    {
        Projectile projectileInstance = (Projectile)Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
        projectileInstance.direction = projectileSpawn.forward;
        projectileInstance.range = range;
        projectileInstance.sendProjectile();
    }
	
	// Update is called once per frame
    void LateUpdate()
    {
        if (!playerHealth.gameOver) {
            int layerMask = 1 << 8;
            Vector3 origin = transform.position;
            Vector3 direction = transform.TransformDirection(Vector3.forward);
            RaycastHit hit;
        
            if (Physics.Raycast(origin, direction, out hit, 3, layerMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

                if (hit.collider.name == "Player")
                {
                    enemyMovement.stopEnemyAnimation();
                    playerDistance = hit.distance;
                    playerWithinRange = true;
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

            //if (enemyMovement.isIdleAnimationPlaying()) {
                if (onCooldown)
                {
                    decreaseCooldown();
                }
                else
                {
                    attackPlayer();
                }
            //}
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
            performAttack(playerDistance);
            onCooldown = true;
        }
    }

    public bool isPlayerWithinRange()
    {
        return playerWithinRange;
    }
}
