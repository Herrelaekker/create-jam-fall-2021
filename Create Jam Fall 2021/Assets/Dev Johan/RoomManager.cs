using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class RoomManager : MonoBehaviour
{
    public int roomXpos;
    public int roomYpos;

    //Left, Up, Right, Down
    //A value of 0 means open, 1 means closed
    public int[] roomConnections = new int[] { 0, 0, 0, 0 };
    public List<DoorwayTransition> doorways;
    public SpriteRenderer mapFloor;

    public virtual void InitiateRoom()
    {
        doorways.Clear();
        doorways.AddRange(transform.GetComponentsInChildren<DoorwayTransition>());

        Color floorColor = mapFloor.color;
        floorColor.a = 0;
        mapFloor.color = floorColor;
    }

    public void RevealOnMap(bool fullReveal)
    {
        Debug.Log(transform.name + "_" + mapFloor.color.a);

        if(fullReveal)
        {
            Color floorColor = mapFloor.color;
            if(floorColor.a < 1f)
            {
                floorColor.a = 1f;
                mapFloor.color = floorColor;
            }
        }
        else
        {
            Color floorColor = mapFloor.color;
            if (floorColor.a < .8f)
            {
                floorColor.a = .8f;
                mapFloor.color = floorColor;
            }
        }
        Debug.Log(transform.name + "_" + mapFloor.color.a);
    }

    public virtual void EnterRoom() { }
    public virtual void LockDoors() { }
    public virtual void UnlockDoors() { }

    public void CloseUnusedDoors()
    {
        for (int x = 0; x < doorways.Count; x++)
        {
            if (doorways[x].roomTransitionTo == null)
            {
                doorways[x].CloseDoor();
                doorways.RemoveAt(x);
                x--;
            }
        }
    }

    public DoorwayTransition GetDoor(int index)
    {
        if (index == 0)
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
        for (int x = 0; x < roomConnections.Length; x++)
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

    public virtual bool TestAvailableConnections()
    {
        if (roomConnections[0] == 1 &&
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



