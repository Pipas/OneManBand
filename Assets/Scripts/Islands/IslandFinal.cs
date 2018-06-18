using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandFinal : MonoBehaviour {
    public const int NO_SHEETS = 3;

    public GameObject[] bridges;
    public GameObject bridgeTrigger;
    public GameObject bridgeCollider;

    public KillEnemy killEnemy;
    public SheetBar sheetbar;
    public bool moving = false;
	
	// Update is called once per frame
	void Update() {
        if ((sheetbar.getSheetIndex() == NO_SHEETS) && killEnemy.isEnemyDead())
        {
            moving = true;
        }

        if (moving)
        {
            bridgeTrigger.SetActive(true);
            float step = Time.deltaTime;

            for (int i = 0; i < bridges.Length; i++) {
                Vector3 target = new Vector3(bridges[i].transform.position.x, 0.73f, bridges[i].transform.position.z);
                bridges[i].transform.position = Vector3.MoveTowards(bridges[i].transform.position, target, step);
            }

            bridgeCollider.transform.position = new Vector3(bridgeCollider.transform.position.x, 0f, bridgeCollider.transform.position.z);
        }
	}
}
