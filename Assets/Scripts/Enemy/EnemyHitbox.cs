using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour {
    private bool playerWithinRange = false;
    private float playerDistance;
    private Vector3 playerPosition;
    private bool onCooldown = false;
    private const float ATTACK_COOL = 1f;
    private float attackCooldown = ATTACK_COOL;

    public HealthSystem playerHealth;
    public EnemyMovement enemyMovement;
    public KillEnemy killEnemy;

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
        if (!killEnemy.isEnemyDead()) {
            if (playerWithinRange)
            {
                Vector3 tmpPos = GameObject.Find("PlayerPivot").transform.position;
                Vector3 playerPos = new Vector3(tmpPos.x, 1f, tmpPos.z);
                playerPosition = playerPos;
                float distance = Vector3.Distance(playerPosition, transform.position);
                playerDistance = distance;
            }
        
            if (!playerHealth.gameOver) {
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
    }

    public void decreaseCooldown()
    {
        if (attackCooldown <= 0)
        {
            attackCooldown = ATTACK_COOL;
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

    /*public void OnColliderEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.name == "PlayerHitbox")
        {
            enemyMovement.stopEnemyAnimation();
            playerWithinRange = true;
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

    public void OnTriggerExit(Collider other)
    {
        if (other.name == "PlayerHitbox")
        {
            playerWithinRange = false;
        }
    }*/

    public Vector3 getPlayerPosition()
    {
        return playerPosition;
    }

    public void setPlayerWithinRange(bool playerWithin)
    {
        playerWithinRange = playerWithin;
    }

    public bool isOnCooldown()
    {
        return onCooldown;
    }
}
