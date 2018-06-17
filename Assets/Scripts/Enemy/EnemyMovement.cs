using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    public Vector3[] pointsMovement;
    public EnemyHitbox hitbox;
    public HealthSystem playerHealth;
    public GameObject player;
    public GameObject enemy;

    private float speed = 1f;
    private float rotationSpeed = 2f;
    private int currentPoint = 0;
    private bool running;

	// Use this for initialization
	void Start () {
        running = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!playerHealth.gameOver)
        {
            if (!hitbox.isPlayerWithinRange())
            {
                autoMovement();
            } else
            {
                stopEnemyAnimation();
                transform.LookAt(hitbox.getPlayerPosition());           // look towards player if he is within range
            }
        }
	}

    public void autoMovement()
    {
        moveEnemyAnimation();

        if (currentPoint < pointsMovement.Length)       // this is to check if it's less than the length of the points
        {
            float dist = Vector3.Distance(transform.position, pointsMovement[currentPoint]);        // distance from current position until next one

            if (dist < 0.5f)                        // if enemy has arrived to next position
            {
                int tmpIndex = currentPoint + 1;

                if (tmpIndex >= pointsMovement.Length)
                {
                    tmpIndex = 0;
                }

                Vector3 targetDir = pointsMovement[tmpIndex] - transform.position;                  // vector in direction of next point
                float step = rotationSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);   // new direction for enemy to look at
                    
                transform.rotation = Quaternion.LookRotation(newDir);

                Vector3 dist_vec = (pointsMovement[tmpIndex] - transform.position).normalized;
                float dotProd = Vector3.Dot(dist_vec, transform.forward);

                if (dotProd >= 0.9)     // almost finished rotating towards point
                {
                    currentPoint++;
                }
            }
            else
            {
                Vector3 targetDir = pointsMovement[currentPoint] - transform.position;
                float step = rotationSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

                transform.rotation = Quaternion.LookRotation(newDir);                               // finished rotating towards current point
                transform.position = Vector3.Lerp(this.transform.position, pointsMovement[currentPoint], Time.deltaTime * speed);   // walk to next position
            }
        }
        else
        {
            currentPoint = 0; // this is to loop it back to zero again
        }
    }

    public void moveEnemyAnimation()
    {
        if (!running)
        {
            enemy.GetComponent<Animator>().SetBool("moveToggle", true);
            running = true;
        }
    }

    public void stopEnemyAnimation() {
        if (running)
        {
            enemy.GetComponent<Animator>().SetBool("moveToggle", false);
            running = false;
        }
    }
}
