using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutGenerator : MonoBehaviour
{
    public int floorSizeX;
    public int floorSizeY;
    public int floorRoomMin;
    public int floorRoomMax;

	public int positionMultiplierX;
	public int positionMultiplierY;

	public int targetRoomCount;
	public int maxAttempts;
	public int failedAttempts;

    public RoomManager startRoom;
    public RoomManager[,] floorLayout;
    public List<RoomManager> floorRooms;

    public GameObject startRoomPrefab;
    public GameObject[] enemyRoomPrefabs;
    public GameObject[] specialRoomPrefabs;
	public GameObject bossRoomPrefab;


	public void GenerateFloorLayout()
	{
		floorLayout = new RoomManager[floorSizeX, floorSizeY];

		GenerateStartRoom();
		GenerateEnemyRooms();
		GenerateBossRoom();
		GenerateSpecialRooms();

		foreach (RoomManager room in floorRooms)
		{
			room.CloseUnusedDoors();
		}
	}

	public void GenerateStartRoom()
    {
		startRoom = (Instantiate(startRoomPrefab) as GameObject).GetComponent<RoomManager>();
		startRoom.InitiateRoom();

		startRoom.roomXpos = Random.Range(floorSizeX / 4, floorSizeX - floorSizeX / 4);
		startRoom.roomYpos = Random.Range(floorSizeY / 4, floorSizeY - floorSizeY / 4);

		startRoom.transform.name = "Room_" + startRoom.roomXpos + ":" + startRoom.roomYpos + " _" + startRoom.name;
		startRoom.transform.position = new Vector3(transform.position.x + startRoom.roomXpos * positionMultiplierX, transform.position.y + startRoom.roomYpos * positionMultiplierY, 0);
		startRoom.transform.parent = transform;

		floorRooms.Add(startRoom);
		floorLayout[startRoom.roomXpos, startRoom.roomYpos] = startRoom;
	}

	public void GenerateEnemyRooms()
	{
		targetRoomCount = Random.Range(floorRoomMin, floorRoomMax + 1);
		List<RoomManager> validRoomsToGenerateFrom = new List<RoomManager>();
		validRoomsToGenerateFrom.Add(startRoom);

		failedAttempts = 0;
		while (floorRooms.Count < targetRoomCount
			  && validRoomsToGenerateFrom.Count > 0
			  && failedAttempts < maxAttempts)
		{
			RoomManager roomToGenerateFrom = validRoomsToGenerateFrom[Random.Range(0, validRoomsToGenerateFrom.Count)];

			int directionToGenerate = roomToGenerateFrom.GetAvailableDoor();

			RoomManager newRoom = null;

			//Debug.Log("Attempting to generate from " + roomToGenerateFrom.name + " in direction: " + directionToGenerate);
			if (directionToGenerate > -1)
			{
				switch (directionToGenerate)
				{
					case 0:
						if (roomToGenerateFrom.roomXpos > 0
						   && CountAdjacentRooms(roomToGenerateFrom, -1, 0) < 2)
						{
							if (floorLayout[roomToGenerateFrom.roomXpos - 1, roomToGenerateFrom.roomYpos] == null)
							{
								newRoom = GenerateRoom(enemyRoomPrefabs[Random.Range(0, enemyRoomPrefabs.Length)],
													   roomToGenerateFrom, directionToGenerate, -1, 0);
							}
						}
						roomToGenerateFrom.CloseConnection(0);
						break;

					case 1:
						if (roomToGenerateFrom.roomYpos < floorSizeY
						   && CountAdjacentRooms(roomToGenerateFrom, 0, +1) < 2)
						{
							if (floorLayout[roomToGenerateFrom.roomXpos, roomToGenerateFrom.roomYpos + 1] == null)
							{
								newRoom = GenerateRoom(enemyRoomPrefabs[Random.Range(0, enemyRoomPrefabs.Length)],
													   roomToGenerateFrom, directionToGenerate, 0, +1);
							}
						}
						roomToGenerateFrom.CloseConnection(1);
						break;

					case 2:
						if (roomToGenerateFrom.roomXpos < floorSizeX - 1
						   && CountAdjacentRooms(roomToGenerateFrom, +1, 0) < 2)
						{
							if (floorLayout[roomToGenerateFrom.roomXpos + 1, roomToGenerateFrom.roomYpos] == null)
							{
								newRoom = GenerateRoom(enemyRoomPrefabs[Random.Range(0, enemyRoomPrefabs.Length)],
													   roomToGenerateFrom, directionToGenerate, +1, 0);
							}
						}
						roomToGenerateFrom.CloseConnection(2);
						break;

					case 3:
						if (roomToGenerateFrom.roomYpos > 0
						   && CountAdjacentRooms(roomToGenerateFrom, 0, -1) < 2)
						{
							if (floorLayout[roomToGenerateFrom.roomXpos, roomToGenerateFrom.roomYpos - 1] == null)
							{
								newRoom = GenerateRoom(enemyRoomPrefabs[Random.Range(0, enemyRoomPrefabs.Length)],
													   roomToGenerateFrom, directionToGenerate, 0, -1);
							}
						}
						roomToGenerateFrom.CloseConnection(3);
						break;
				}
			}
			else
			{
				Debug.Log("Derped");
			}

			if (newRoom != null)
			{
				validRoomsToGenerateFrom.Add(newRoom);
			}
			else
			{
				failedAttempts++;
			}

			if (roomToGenerateFrom.TestAvailableConnections() == false)
			{
				validRoomsToGenerateFrom.Remove(roomToGenerateFrom);
			}
		}
		Debug.Log("Failed attempts to generate layout: " + failedAttempts);
	}

	public void GenerateSpecialRooms()
	{ 
		foreach (GameObject room in specialRoomPrefabs)
		{
			bool generateSuccess = false;

			while (!generateSuccess)
			{
				RoomManager roomToGenerateFrom = null;
				do
				{
					roomToGenerateFrom = floorRooms[Random.Range(0, floorRooms.Count)];
				} while (roomToGenerateFrom.TestAvailableConnections() == false);

				RoomManager newRoom = null;

				int direction = roomToGenerateFrom.GetAvailableDoor();

				if (direction > -1)
				{
					switch (direction)
					{
						case 0:
							if (roomToGenerateFrom.roomXpos > 0
							   && floorLayout[roomToGenerateFrom.roomXpos - 1, roomToGenerateFrom.roomYpos] == null)
							{
								newRoom = GenerateRoom(room.gameObject, roomToGenerateFrom, direction, -1, 0);
								generateSuccess = true;
							}
							break;

						case 1:
							if (
								roomToGenerateFrom.roomYpos > 0
								&& floorLayout[roomToGenerateFrom.roomXpos, roomToGenerateFrom.roomYpos - 1] == null)
							{
								newRoom = GenerateRoom(room.gameObject, roomToGenerateFrom, direction, 0, +1);
								generateSuccess = true;
							}
							break;

						case 2:
							if (roomToGenerateFrom.roomXpos < floorSizeX - 1
							   && floorLayout[roomToGenerateFrom.roomXpos + 1, roomToGenerateFrom.roomYpos] == null)
							{
								newRoom = GenerateRoom(room.gameObject, roomToGenerateFrom, direction, +1, 0);
								generateSuccess = true;
							}
							break;

						case 3:
							if (roomToGenerateFrom.roomYpos < floorSizeY - 1
							   && floorLayout[roomToGenerateFrom.roomXpos, roomToGenerateFrom.roomYpos + 1] == null)
							{
								newRoom = GenerateRoom(room.gameObject, roomToGenerateFrom, direction, 0, -1);
								generateSuccess = true;
							}
							break;
					}

					if(generateSuccess)
                    {
						newRoom.CloseUnusedDoors();
                    }
				}
			}
		}
	}

	public void GenerateBossRoom()
    {
		bool bossRoomGenerated = false;
		while (!bossRoomGenerated)
		{
			RoomManager roomToGenerateFrom = floorRooms[Random.Range(0, floorRooms.Count)];

			if (roomToGenerateFrom.GetComponent<EnemyRoomManager>())
			{
				int direction = roomToGenerateFrom.GetAvailableDoor();

				if (direction == 1)
				{
					if (
								roomToGenerateFrom.roomYpos > 0
								&& floorLayout[roomToGenerateFrom.roomXpos, roomToGenerateFrom.roomYpos - 1] == null)
					{
						RoomManager newRoom = GenerateRoom(bossRoomPrefab.gameObject, roomToGenerateFrom, direction, 0, +1);
						bossRoomGenerated = true;
						newRoom.CloseUnusedDoors();
					}
				}
			}
		}
	}

	public RoomManager GenerateRoom(GameObject roomPrefab, RoomManager roomToGenerateFrom, int direction, int Xoffset, int Yoffset)
	{
		RoomManager newRoom = null;

		newRoom = (Instantiate(roomPrefab) as GameObject).GetComponent<RoomManager>();
		newRoom.InitiateRoom();

		floorRooms.Add(newRoom);
		floorLayout[roomToGenerateFrom.roomXpos + Xoffset, roomToGenerateFrom.roomYpos + Yoffset] = newRoom;

		newRoom.roomXpos = roomToGenerateFrom.roomXpos + Xoffset;
		newRoom.roomYpos = roomToGenerateFrom.roomYpos + Yoffset;

		newRoom.transform.position = new Vector3(transform.position.x + newRoom.roomXpos * positionMultiplierX, transform.position.y + newRoom.roomYpos * positionMultiplierY, 0);
		newRoom.transform.parent = transform;

		newRoom.name = "Room_" + newRoom.roomXpos + ":" + newRoom.roomYpos + "_" + roomPrefab.name;

		int otherDirection;
		if (direction > 1)
		{
			otherDirection = direction - 2;
		}
		else
		{
			otherDirection = direction + 2;
		}

		newRoom.MakeConnection(otherDirection, roomToGenerateFrom);
		roomToGenerateFrom.MakeConnection(direction, newRoom);

		CloseBorderConnections(newRoom);

		return newRoom;
	}

	public void CloseBorderConnections(RoomManager testRoom)
    {
		if (testRoom.roomXpos == 0)
			testRoom.CloseConnection(0);
		if (testRoom.roomYpos == floorSizeY - 1)
			testRoom.CloseConnection(1);
		if (testRoom.roomXpos == floorSizeX - 1)
			testRoom.CloseConnection(2);
		if (testRoom.roomYpos == 0)
			testRoom.CloseConnection(3);
	}

	public List<RoomManager> GetSurroundingRooms(RoomManager room)
    {
		List<RoomManager> roomList = new List<RoomManager>();

		for (int x = room.roomXpos - 1; x <= room.roomXpos + 1; x++)
			for (int y = room.roomYpos - 1; y <= room.roomYpos + 1; y++)
            {
				if (x != 2 && y != 2)
				{
					try
					{
						if (floorLayout[x, y] != null)
						{
							roomList.Add(floorLayout[x, y]);
						}

					}
					catch { }
				}
            }
		return roomList;
    }

	public int CountAdjacentRooms(RoomManager roomToCountFor, int Xoffset, int Yoffset)
	{
		int returnInt = 0;

		Debug.Log(roomToCountFor.name + " _ " + Xoffset + ":" + Yoffset);

		//Test left of new room
		if (roomToCountFor.roomXpos + Xoffset > 0 &&
		   floorLayout[roomToCountFor.roomXpos + Xoffset - 1, roomToCountFor.roomYpos + Yoffset] != null)
		{
			Debug.Log(floorLayout[roomToCountFor.roomXpos + Xoffset - 1, roomToCountFor.roomYpos + Yoffset]);
			returnInt++;
		}
		//Test above new room
		if (roomToCountFor.roomYpos + Yoffset < floorSizeY - 1 &&
		   floorLayout[roomToCountFor.roomXpos + Xoffset, roomToCountFor.roomYpos + Yoffset + 1] != null)
		{
			Debug.Log(floorLayout[roomToCountFor.roomXpos + Xoffset, roomToCountFor.roomYpos + Yoffset + 1]);
			returnInt++;
		}
		//Test right of new room
		if (roomToCountFor.roomXpos + Xoffset < floorSizeX - 1 &&
		   floorLayout[roomToCountFor.roomXpos + Xoffset + 1, roomToCountFor.roomYpos + Yoffset] != null)
		{
			Debug.Log(floorLayout[roomToCountFor.roomXpos + Xoffset + 1, roomToCountFor.roomYpos + Yoffset]);
			returnInt++;
		}
		//Test below new room
		if (roomToCountFor.roomYpos + Yoffset > 0 &&
		   floorLayout[roomToCountFor.roomXpos + Xoffset, roomToCountFor.roomYpos + Yoffset - 1] != null)
		{
			Debug.Log(floorLayout[roomToCountFor.roomXpos + Xoffset, roomToCountFor.roomYpos + Yoffset - 1]);
			returnInt++;
		}

		Debug.Log("Found " + returnInt + " adjacent rooms");
		return returnInt;
	}
}
