using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpecialRoomManager : RoomManager
{
    // Start is called before the first frame update
    public override void InitiateRoom()
    {
        doorways.Clear();
        doorways.AddRange(transform.GetComponentsInChildren<DoorwayTransition>());
    }

    public override bool TestAvailableConnections()
    {
        return false;
    }
}
