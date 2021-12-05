using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyRoomManager : RoomManager
{
    public bool hasBeenCleared;
    public bool enemyBattle;

    public Transform roomEnemies;
    public List<Transform> enemies;


    // Start is called before the first frame update
    public override void InitiateRoom()
    {
        base.InitiateRoom();

        hasBeenCleared = false;
        enemyBattle = false;

        enemies.Clear();
        foreach (Transform child in roomEnemies)
        {
            enemies.Add(child);
        }

        if (enemies.Count == 0)
        {
            hasBeenCleared = true;
        }
        else
        {
            foreach (Transform enemy in enemies)
            {
                enemy.gameObject.SetActive(false);
            }
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
                hasBeenCleared = true;
                UnlockDoors();
            }
        }
    }

    public override void EnterRoom()
    {
        if(!hasBeenCleared)
        {
            enemyBattle = true;
            SpawnEnemies();
            LockDoors();
        }
    }

    public void SpawnEnemies()
    {
        foreach (Transform enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
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
