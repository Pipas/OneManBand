using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyHitbox hitbox = other.transform.parent.gameObject.GetComponent<EnemyHitbox>();
            hitbox.checkIfObstaclesAhead(transform.position);
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
