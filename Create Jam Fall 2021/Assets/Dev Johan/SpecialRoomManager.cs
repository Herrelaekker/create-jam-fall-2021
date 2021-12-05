using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpecialRoomManager : RoomManager
{
    
    public override bool TestAvailableConnections()
    {
        return false;
    }
}
