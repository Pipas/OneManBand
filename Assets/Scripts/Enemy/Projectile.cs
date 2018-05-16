using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public Vector3 direction { get; set; }
    public float range { get; set; }
    public int damage;
    Vector3 spawnPosition;

    public HealthSystem playerHealth;

	// Use this for initialization
	void Start () {
        spawnPosition = transform.position;

        GameObject player = GameObject.Find("Player");
        playerHealth = player.GetComponent<HealthSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(spawnPosition, transform.position) >= range)
        {
            Extinguish();
            playerHealth.TakeDamage(-1);
        }
	}

    public void sendProjectile()
    {
        GetComponent<Rigidbody>().AddForce(direction * 50f);
    }

    void Extinguish()
    {
        Destroy(gameObject);
    }

}
