using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : RoomManager
{
    public GameObject TheBoss;

    public bool hasBeenCleared;
    public bool bossBattle;

    public override void InitiateRoom()
    {
        base.InitiateRoom();

        TheBoss.SetActive(false);
    }

    void Update()
    {
        if (bossBattle)
        {
            if (TheBoss.gameObject == null)
            {
                bossBattle = false;
                hasBeenCleared = true;
                UnlockDoors();
            }
        }
    }

    public override void EnterRoom()
    {
        if (!hasBeenCleared)
        {
            bossBattle = true;
            SpawnBoss();
            LockDoors();
        }
    }

    public void SpawnBoss()
    {
        TheBoss.SetActive(true);
    }

    public override void LockDoors()
    {
        foreach (DoorwayTransition doorway in doorways)
        {
            doorway.CloseDoor();
        }
    }

    public override void UnlockDoors()
    {
        foreach (DoorwayTransition doorway in doorways)
        {
            doorway.OpenDoor();
        }
    }

    public override bool TestAvailableConnections()
    {
        return false;
    }
}
