using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour {
    private bool playerWithinRange = false;
    private bool onCooldown = false;
    private float attackCooldown = 3.0f;

    public HealthSystem playerHealth;
	
	// Update is called once per frame
    void Update()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            if (hit.collider.name == "Player")
            {
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

        if (onCooldown)
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
        else
        {
            if (playerWithinRange) {
                playerHealth.TakeDamage(-1);
                onCooldown = true;
            }
        }
    }

    public bool isPlayerWithinRange()
    {
        return playerWithinRange;
    }
}
