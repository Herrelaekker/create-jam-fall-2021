using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyRoomManager : RoomManager
{
    public bool enemyBattle;

    public Transform roomEnemies;
    private bool hasEnemies { get { return roomEnemies.childCount > 0; } }

    // Start is called before the first frame update
    public override void InitiateRoom()
    {
        base.InitiateRoom();

        enemyBattle = false;

        foreach (Transform child in roomEnemies)
        {
            child.gameObject.SetActive(false);
        }     
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyBattle)
        {
            if(roomEnemies.childCount == 0)
            {
                enemyBattle = false;
                UnlockDoors();
            }
        }
    }

    public override void EnterRoom()
    {
        if(hasEnemies)
        {
            enemyBattle = true;
            SpawnEnemies();
            LockDoors();
        }
    }

    public void SpawnEnemies()
    {
        foreach (Transform enemy in roomEnemies)
        {
            enemy.gameObject.SetActive(true);
        }
    }

    public override bool CheckBattle()
    {
        return hasEnemies;
    }

    public override void LockDoors()
    {
        foreach(DoorwayTransition doorway in doorways)
        {
            doorway.CloseDoor();
        }
    }

    public override void UnlockDoors()
    {
        foreach(DoorwayTransition doorway in doorways)
        {
            doorway.OpenDoor();
        }
    }

    
}
