using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeTrigger : MonoBehaviour 
{
    public CameraAnimation camAnimation;
    public PlayerMovement playerMov;
    public int playerMove = 5;

    private int direction;
    private Island island;

    // Use this for initialization
    private void Start() 
    {
        island = transform.root.GetComponent<Island>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.name == "Player")
        {
            PlayerMovement Player = GameObject.Find("PlayerPivot").GetComponent<PlayerMovement>();
            if(other.transform.position.x < transform.position.x)
                direction = 1;
            else
                direction = -1;

            playerMov.setAutoMoving(true);

            for (int i = 0; i < playerMove; i++)
                Player.QueueAnimation(new AnimationItem(Vector3.right * direction, Player.baseSpeed, true, true));

            camAnimation.MoveCamera(direction, island.cameraShift);
        }
    }
}
