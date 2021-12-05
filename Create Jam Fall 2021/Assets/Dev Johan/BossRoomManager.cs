using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : RoomManager
{
    public GameObject TheBoss;

    public bool bossAlive;

    public override void InitiateRoom()
    {
        base.InitiateRoom();
        bossAlive = true;

        TheBoss.SetActive(false);
    }

    void Update()
    {
        if (bossAlive && TheBoss.gameObject == null)
        {
            bossAlive = false;
            UnlockDoors();
        }
    }

    public override void EnterRoom()
    {
        if (bossAlive)
        {
            SpawnBoss();
            LockDoors();
        }
    }

    public void SpawnBoss()
    {
        TheBoss.SetActive(true);
    }

    public override bool CheckBattle()
    {
        return bossAlive;
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
