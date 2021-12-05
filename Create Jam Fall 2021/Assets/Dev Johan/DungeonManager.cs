using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public GameObject player;
    public GameObject dungeonCamera;
    public RoomManager currentRoom;

    public Vector3 roomCamPos;

    public LayoutGenerator layoutGenerator;

    // Start is called before the first frame update
    void Start()
    {
        if(layoutGenerator != null)
        {
            layoutGenerator.GenerateFloorLayout();
            currentRoom = layoutGenerator.startRoom;

            player.transform.position = currentRoom.transform.position;
            roomCamPos = currentRoom.transform.position + new Vector3(0, 0, -10);
            dungeonCamera.transform.position = roomCamPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(dungeonCamera.transform.position != roomCamPos)
        {
            Vector3 dunCamPos = dungeonCamera.transform.position;
            if (Vector3.Distance(dunCamPos, roomCamPos) < .1)
            {
                dungeonCamera.transform.position = roomCamPos;
            }
            else
            {
                //dungeonCamera.transform.position = roomCamPos;
                dungeonCamera.transform.position = Vector3.Slerp(dunCamPos, roomCamPos, 10f * Time.deltaTime);
            }
        }
    }

    public void RoomTransition(GameObject roomTransitionTo)
    {
        Vector3 transitionRoomPos = roomTransitionTo.transform.position;
        Vector3 playerRelativePos = player.transform.position - currentRoom.transform.position;

        if (Mathf.Abs(playerRelativePos.x) > 4)
        {
            player.transform.position = transitionRoomPos + new Vector3(-playerRelativePos.x + (playerRelativePos.x > 0 ? .1f : -.1f), playerRelativePos.y, 0);
            //dungeonCamera.transform.position = transitionRoomPos + new Vector3(0, 0, -10);
            roomCamPos = transitionRoomPos + new Vector3(0, 0, -10);
        }
        else if (Mathf.Abs(playerRelativePos.y) > 2)
        {
            player.transform.position = transitionRoomPos + new Vector3(playerRelativePos.x, -playerRelativePos.y + (playerRelativePos.y > 0 ? .1f : -.1f), 0);
            //dungeonCamera.transform.position = transitionRoomPos + new Vector3(0, 0, -10);
            roomCamPos = transitionRoomPos + new Vector3(0, 0, -10);
        }

        currentRoom = roomTransitionTo.GetComponent<RoomManager>();
        currentRoom.EnterRoom();
    }
}
