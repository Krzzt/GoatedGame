using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using NavMeshPlus.Components;

[TestFixture]
public class RoomManagerTests : MonoBehaviour
{
	GameObject dummyObject;
	GameObject dummyRoom;
	List<GameObject> dummyDoors;
	RoomManager roomManager;
	NavMeshSurface dummySurface;

	[SetUp]
	public void Setup()
	{
		dummyRoom = new GameObject();
		dummyDoors = new List<GameObject>();
		for (int i = 0; i < 3; i++)
		{
			GameObject door = new GameObject();
			dummyDoors.Add(door);
			door.transform.parent = dummyRoom.transform;
			door.tag = "Door";
			door.AddComponent<SpriteRenderer>();
			door.AddComponent<BoxCollider2D>();
			GameObject doorFacing = new GameObject();
			doorFacing.transform.parent = door.transform;
			int decider = Random.Range(0, 2);
			if (decider == 1)
			{
				doorFacing.transform.position += new Vector3(1, 0, 0);
			}
			else
			{
				doorFacing.transform.position -= new Vector3(1, 0, 0);
			}
			GameObject doorMiddle = new GameObject();
			doorMiddle.transform.parent = door.transform;
			DoorScript ds = door.AddComponent<DoorScript>();
			ds.DoorFacing = doorFacing.transform;
			ds.DoorMiddle = doorMiddle.transform;
			door.transform.position += new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), 0);
		}
		dummyObject = new GameObject();
		roomManager = dummyObject.AddComponent<RoomManager>();
		roomManager.Rooms = new List<GameObject>();
		roomManager.StartRoomPrefab = dummyRoom;
		roomManager.UsedDoors = new List<GameObject>();
		roomManager.RoomPrefabs = new List<GameObject>
		{
			dummyRoom
		};
		roomManager.AvailableDoors = new List<GameObject>();
		dummySurface = dummyRoom.AddComponent<NavMeshSurface>();
		RoomScript rs = dummyRoom.AddComponent<RoomScript>();
		rs.AllObstacles = new List<GameObject>();
		rs.RoomDoors = new List<GameObject>(dummyDoors);
		rs.LootPoint = rs.transform;
		roomManager.BossPortal = new GameObject();


		RoomManager.meshSurface = dummySurface;
		RoomManager.Instance = roomManager;
		roomManager.MaxTries = 100000;
	}

	[Test]
	public void GenerateStartRoomTest()
	{
		roomManager.GenerateStartRoom();

		Assert.AreEqual(roomManager.Rooms.Count, 1);
	}

	[Test]
	public void GenerateRoomsTest()
	{
		roomManager.GenerateStartRoom();
		roomManager.GenerateRooms(10);

		Assert.AreEqual(roomManager.Rooms.Count,11); //one more because of start room
	}

	[Test]
	public void GenerateManyRooms()
	{
		roomManager.GenerateStartRoom();
		roomManager.GenerateRooms(100);

		Assert.AreEqual(roomManager.Rooms.Count, 101);
	}
}
