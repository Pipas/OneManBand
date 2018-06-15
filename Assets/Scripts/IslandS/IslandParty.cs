using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandParty : MonoBehaviour {
    public GameObject bridge;
    public GameObject bridgeTrigger;
    public GameObject bridgeCollider;

    public PlayerMovement player;
    public bool moving = false;
	
	// Update is called once per frame
	void Update () {
        if (player.hasFoundParty())
        {
            moving = true;
        }

        if (moving)
        {
            player.setFoundParty(false);
            bridgeTrigger.SetActive(true);
            float step = Time.deltaTime;
            Vector3 target = new Vector3(bridge.transform.position.x, 0.73f, bridge.transform.position.z);
            bridge.transform.position = Vector3.MoveTowards(bridge.transform.position, target, step);
            bridgeCollider.transform.position = new Vector3(bridgeCollider.transform.position.x, 0f, bridgeCollider.transform.position.z);
        }
	}
}
