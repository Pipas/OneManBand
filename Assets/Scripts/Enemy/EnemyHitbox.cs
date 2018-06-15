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
    Fireball projectile;

    void Start()
    {
        projectile = Resources.Load<Fireball>("Fireball");
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

    public void checkIfObstaclesAhead(Vector3 playerPosition) {
        enemyMovement.stopEnemyAnimation();
        playerWithinRange = true;

        Vector3 origin = transform.position;
        Vector3 direction = playerPosition - transform.position;
        RaycastHit hit;
        
        if (Physics.Raycast(origin, direction, out hit, 3))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            Debug.Log(hit.collider.name);
            if (hit.collider.tag != "Player" && hit.collider.tag != "Party" && hit.collider.tag != "PlayerHitbox")
            {
                playerWithinRange = false;
            }
        }
    }

    public void performAttack(float range)
    {
        Fireball projectileInstance = (Fireball)Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
        projectileInstance.direction = playerPosition - transform.position;
        projectileInstance.range = range-0.5f;
        projectileInstance.sendProjectile();
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
