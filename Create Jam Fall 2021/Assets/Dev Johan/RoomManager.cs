using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public int roomXpos;
    public int roomYpos;

    public bool isSpecialRoom;

    //Left, Up, Right, Down
    //A value of 0 means open, 1 means closed
    public int[] roomConnections = new int[] { 0, 0, 0, 0 };

    public bool hasBeenCleared;
    public bool clearingRoom;

    public Transform roomEnemies;
    public List<Transform> enemies;

    public List<DoorwayTransition> doorways;

    // Start is called before the first frame update
    public void InitiateRoom()
    {
        hasBeenCleared = false;
        clearingRoom = false;

        doorways.Clear();
        doorways.AddRange(transform.GetComponentsInChildren<DoorwayTransition>());

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

    public void ClearUnusedDoors()
    { 
        for(int x = 0; x < doorways.Count; x++)
        {
            if (doorways[x].roomTransitionTo == null)
            {
                doorways[x].CloseDoor();
                doorways.RemoveAt(x);
                x--;
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
        }
    }

    public void LockDoors()
    {
        foreach(DoorwayTransition doorway in doorways)
        {
            doorway.CloseDoor();
        }
    }

    public void UnlockDoors()
    {
        foreach(DoorwayTransition doorway in doorways)
        {
            doorway.OpenDoor();
        }
    }

    public DoorwayTransition GetDoor(int index)
    {
        if(index == 0)
        {
            doorways = doorways.OrderBy(door => door.transform.position.x).ToList();
            return doorways[0];
        }
        if (index == 1)
        {
            doorways = doorways.OrderByDescending(door => door.transform.position.y).ToList();
            return doorways[0];
        }
        if (index == 2)
        {
            doorways = doorways.OrderByDescending(door => door.transform.position.x).ToList();
            return doorways[0];
        }
        if (index == 3)
        {
            doorways = doorways.OrderBy(door => door.transform.position.y).ToList();
            return doorways[0];
        }
        Debug.Log("Requested a door by an index number that is not supported: " + index);
        return null;
    }

    public int GetAvailableDoor()
    {
        int returnValue = -1;

        List<int> availableIndexes = new List<int>();
        for(int x = 0; x < roomConnections.Length; x++)
        {
            if (roomConnections[x] == 0)
            {
                availableIndexes.Add(x);
            }
        }

        if (availableIndexes.Count != 0)
        {
            returnValue = availableIndexes[Random.Range(0, availableIndexes.Count)];
        }

        return returnValue;
    }

    public void MakeConnection(int indexToConnect, RoomManager otherRoom)
    {
        DoorwayTransition myDoor = GetDoor(indexToConnect);

        myDoor.roomTransitionTo = otherRoom.gameObject;
        roomConnections[indexToConnect] = 1;
    }

    public void CloseConnection(int indexToClose)
    {
        roomConnections[indexToClose] = 1;
    }

    public bool TestAvailableConnections()
    {
        if(roomConnections[0] == 1 &&
            roomConnections[1] == 1 &&
            roomConnections[2] == 1 &&
            roomConnections[3] == 1)
        {
            //All are unavailable
            return false;
        }
        return true;
    }
}
