using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandEnemy : MonoBehaviour {
    public float bridgeHeight;          // 0.73 no tutorial, 1.85 no nível 1
    public float bridgeColliderHeight;  // 0 no tutorial, 1 no nível 1
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
            Vector3 target = new Vector3(bridge.transform.position.x, bridgeHeight, bridge.transform.position.z);

            bridge.transform.position = Vector3.MoveTowards(bridge.transform.position, target, step);
            bridgeCollider.transform.position = new Vector3(bridgeCollider.transform.position.x, bridgeColliderHeight, bridgeCollider.transform.position.z);
        }
	}
}
