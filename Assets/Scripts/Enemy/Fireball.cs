using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {
    public Vector3 direction { get; set; }
    public float range { get; set; }
    public int damage;
    Vector3 spawnPosition;
    public HealthSystem playerHealth;
    private float fireballSpeed = 10f;

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
            GameObject player = GameObject.Find("PlayerPivot");
            Vector3 playerPosition = player.transform.position;

            if (Vector3.Distance(transform.position, playerPosition) <= 1f)
            {
                playerHealth.TakeDamage(-1);
            }
        }
	}

    public void sendProjectile()
    {
        GetComponent<Rigidbody>().AddForce(direction * fireballSpeed);
    }

    void Extinguish()
    {
        Destroy(gameObject);
    }

}
