using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour {
    private bool playerWithinRange = false;
    private bool noObstacles = true;

    private float playerDistance;
    private Vector3 playerPosition;
    private bool onCooldown = false;
    private const float ATTACK_COOL = 2f;
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
    void Update()
    {
        if (!killEnemy.isEnemyDead()) {
            if (playerWithinRange)
            {
                playerPosition = calculatePlayerPosition();
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
        this.noObstacles = true;
        
        Vector3 origin = transform.position;
        playerPosition = calculatePlayerPosition();

        Vector3 direction = playerPosition - transform.position;
        RaycastHit hit;
        
        if (Physics.Raycast(origin, direction, out hit, 2))
        {
            Debug.DrawRay(transform.position, direction * hit.distance, Color.red);

            if (hit.collider.tag != "Player" && hit.collider.tag != "Party")
            {
                noObstacles = false;
            }
        }
    }

    public void performAttack(float range)
    {
        Fireball projectileInstance = (Fireball)Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
        projectileInstance.direction = playerPosition - transform.position;
        projectileInstance.range = 5.0f;
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
            checkIfObstaclesAhead(playerPosition);

            if (noObstacles)
            {
                enemyMovement.stopEnemyAnimation();

                performAttack(playerDistance);
                onCooldown = true;
            }
            else
            {
                enemyMovement.moveEnemyAnimation();
            }
        }
    }

    public Vector3 calculatePlayerPosition() {
        Vector3 tmpPos = GameObject.Find("PlayerPivot").transform.position;
        Vector3 playerPos = new Vector3(tmpPos.x, transform.position.y, tmpPos.z);
        return playerPos;
    }

    public bool isPlayerWithinRange()
    {
        return playerWithinRange;
    }

    public bool hasNoObstacles()
    {
        return noObstacles;
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
