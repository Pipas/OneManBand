using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandEnemy : MonoBehaviour {
    public GameObject bridge;
    public GameObject bridgeTrigger;
    public GameObject bridgeCollider;

    public KillEnemy killEnemy;
    public bool moving = false;
	
	// Update is called once per frame
	void Update () {
        if (killEnemy.isEnemyDead())
        {
            moving = true;
        }

        if (moving)
        {
            bridgeTrigger.SetActive(true);
            float step = Time.deltaTime;
            Vector3 target = new Vector3(bridge.transform.position.x, 0.73f, bridge.transform.position.z);
            bridge.transform.position = Vector3.MoveTowards(bridge.transform.position, target, step);
            bridgeCollider.transform.position = new Vector3(bridgeCollider.transform.position.x, 0f, bridgeCollider.transform.position.z);
        }
	}
}
