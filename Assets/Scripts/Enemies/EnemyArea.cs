using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArea : MonoBehaviour {
    private bool playerWithinRange = false;
    private float attackCooldown = 3.0f;

    public HealthSystem playerHealth;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (!playerWithinRange) {
            int layerMask = 1 << 8;
            layerMask = ~layerMask;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 2, layerMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                if (hit.collider.name == "Player")
                {
                    playerWithinRange = true;
                    Debug.Log("attack!");
                    playerHealth.TakeDamage(-1);
                }
            }
            else
            {
                attackCooldown = 3.0f;
            }
        }
        else
        {
            attackCooldown -= Time.deltaTime;

            if (attackCooldown <= 0)
            {
                attackCooldown = 3.0f;
                playerWithinRange = false;
            }
        }
	}
}
