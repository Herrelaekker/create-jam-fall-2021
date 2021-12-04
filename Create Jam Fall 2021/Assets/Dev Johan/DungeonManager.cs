using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public GameObject player;
    public GameObject dungeonCamera;
    public GameObject currentRoom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RoomTransition(GameObject roomTransitionTo)
    {
        Vector3 transitionRoomPos = roomTransitionTo.transform.position;
        Vector3 playerRelativePos = player.transform.position - currentRoom.transform.position;

        if (Mathf.Abs(playerRelativePos.x) > 4)
        {
            player.transform.position = transitionRoomPos + new Vector3(-playerRelativePos.x, playerRelativePos.y, 0);
            dungeonCamera.transform.position = transitionRoomPos + new Vector3(0, 0, -10);
        }
        else if (Mathf.Abs(playerRelativePos.y) > 2)
        {
            player.transform.position = transitionRoomPos + new Vector3(playerRelativePos.x, -playerRelativePos.y, 0);
            dungeonCamera.transform.position = transitionRoomPos + new Vector3(0, 0, -10);
        }

        currentRoom = roomTransitionTo;
    }
}
