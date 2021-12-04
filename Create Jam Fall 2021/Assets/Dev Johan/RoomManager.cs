using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    public bool hasBeenCleared;
    public bool clearingRoom;

    public Transform roomEnemies;
    public List<Transform> enemies;

    public List<DoorwayTransition> doorways;

    // Start is called before the first frame update
    void Start()
    {
        hasBeenCleared = false;
        clearingRoom = false;

        doorways.Clear();
        doorways.AddRange(transform.GetComponentsInChildren<DoorwayTransition>());

        for (int x = 0; x < doorways.Count; x++)
        {
            if(doorways[x].roomTransitionTo == null)
            {
                doorways[x].CloseDoor();
                doorways.Remove(doorways[x]);
                x--;
            }
        }

        enemies.Clear();
        foreach(Transform child in roomEnemies)
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
        if(clearingRoom)
        {
            if(roomEnemies.childCount == 0)
            {
                clearingRoom = false;
                hasBeenCleared = true;
                UnlockDoors();
            }
        }
    }

    public void EnterRoom()
    {
        if(!hasBeenCleared)
        {
            Debug.Log("ENTERED ROOM FOR THE FIRST TIME");
            clearingRoom = true;
            SpawnEnemies();
            LockDoors();
        }
    }

    public void SpawnEnemies()
    {
        foreach (Transform enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
            Debug.Log("SPAWN ENEMY HAVE FUN");
        }
    }

    public void LockDoors()
    {
        foreach(DoorwayTransition doorway in doorways)
        {
            Debug.Log("DOORS ARE CLOSED! KILL THE ENEMIES!");
            doorway.CloseDoor();
        }
    }

    public void UnlockDoors()
    {
        foreach(DoorwayTransition doorway in doorways)
        {
            Debug.Log("GOOD JOB HAVE FREEDOM!");
            doorway.OpenDoor();
        }
    }
}
