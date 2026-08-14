using NavMeshPlus.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.Rendering.Universal.Internal;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour
{
	public static RoomManager Instance;
	[field: SerializeField] public List<GameObject> RoomPrefabs { get; set; } //List of all room prefabs available. Does not change during runtime (yet?)
	[field: SerializeField] public List<GameObject> Rooms { get; set; } //List of all rooms in the current layer
	[field: SerializeField] public List<GameObject> AvailableDoors { get; set; } //List of all doors in the current layer
	[field: SerializeField] public List<GameObject> UsedDoors { get; set; } //List of all doors who have a valid room aligned
	[field: SerializeField] public GameObject StartRoomPrefab { get; set; } //The starting room prefab (Open for changes if necessary)
	[SerializeField] private int baseRoomCount = 10; //Base amount of rooms (we use this for our formula)
	[SerializeField] private int tries = 0; //Number of current tries (To prevent infinite Loops)
	[field: SerializeField] public int MaxTries { get; set; } = 10000000; //Number of max Tries before the Loop breaks (To prevent infinite Loops)
	public GameObject StartRoom { get; set; } //The one and only start room instance
	public static NavMeshSurface meshSurface;

	[field: SerializeField] public GameObject BossPortal { get; set; }


	public void Awake()
	{
		meshSurface = gameObject.GetComponent<NavMeshSurface>();
		GenerateStartRoom();
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

	}
	private void Start()
	{
		StartCoroutine(WaitToGenerateRooms());
	}
	public IEnumerator WaitToGenerateRooms()
	{
		yield return new WaitUntil(() => LayerManager.CurrentLayer); //wait until the seed is set
		SetStartRoomAngle();
		GenerateRooms(Random.Range(baseRoomCount, baseRoomCount + 3) + (int)Math.Floor(LayerManager.CurrentLayerNumber / 2f) * 3 + (LayerManager.CurrentLayerNumber % 2));
	}

	public void GenerateStartRoom()
	{
		if (LayerManager.CurrentLayerNumber == 1) return;
		StartRoom = Instantiate(StartRoomPrefab, Vector3.zero, Quaternion.identity); //Make tha start room
		StartRoom.GetComponent<RoomScript>().Depth = 0;
		Rooms.Add(StartRoom); //Yes exactly this room should be added to the rooms list
		GameManager.currentRoom = StartRoom.GetComponent<RoomScript>();
		StartRoom.GetComponent<RoomScript>().ClearRoom();
		GameManager.roomsCleared--; //to prevent startroom counting as a cleared room (fuck this)
		AvailableDoors.Add(GameObject.FindWithTag("Door")); //And lets also get the first door.
		SetStartRoomAngle();

	}

	public void SetStartRoomAngle()
	{
		if (LayerManager.CurrentLayerNumber < 1) return;
		float[] angles = { 0f, 90f, 180f, 270f }; //Simple array, because its not gonna change
		float randomAngle = angles[Random.Range(0, angles.Length)]; //Pick a random start rotation
		StartRoom.transform.rotation = Quaternion.Euler(0, 0, randomAngle); // and apply it.
	}

	public void SetNewLayer()
	{
		if (LayerManager.CurrentLayerNumber <= 1) return;
		AvailableDoors.Clear();
		Rooms.Clear();
		UsedDoors.Clear();
		GenerateStartRoom();
		GenerateRooms();
	}

	private void OnEnable()
	{
		RoomScript.RoomCleared += SetMiniMap;
		LayerManager.newLayer += SetNewLayer;
	}

	private void OnDisable()
	{
		RoomScript.RoomCleared -= SetMiniMap;
		LayerManager.newLayer -= SetNewLayer;
	}

	public void OnDestroy()
	{
		Instance = null;
		meshSurface = null;
	}


	[ContextMenu("Generate Rooms")] //To call GenerateRooms from the inspector (Will probably get obsolete once the Game Manager etc handles when to gen rooms)
	public void GenerateRooms() //Helper methode to be overriden that can be called from the inspector (Since it isn't possible to do so with a methode that has Parameters)
	{
		if (LayerManager.CurrentLayerNumber <= 1) return;
		meshSurface.BuildNavMesh();
		GenerateRooms(Random.Range(baseRoomCount, baseRoomCount + 3) + (int)Math.Floor(LayerManager.CurrentLayerNumber / 2f) * 3 + (LayerManager.CurrentLayerNumber % 2)); //I use this to be able to default to the number set in the inspector if the call was not from an outside source.
	}
	public void GenerateRooms(int numOfRooms) //might later be called by something else, hence public and the Parameter(Optional)
	{
		Debug.Log("room Numbers: " + numOfRooms);
		tries = 0;
		for (int i = 0; i < numOfRooms; i++) //iterrate over how many rooms should be generated
		{
			tries++;
			if (tries >= MaxTries)  //To Prevent infinite Loops (Yes it is a slight bottleneck if you want to create over 10k rooms (Who would do that?))
			{
				break;
			}

			if (AvailableDoors.Count == 0) break; //This implies that no start room has been generated so no place to start generation. There always has to be at least 1 door.
			int randomDoorIndex = Random.Range(0, AvailableDoors.Count);
			GameObject randomDoor = AvailableDoors[randomDoorIndex];

			int randomIndex = Random.Range(0, RoomPrefabs.Count); //Get a random index for the prefab list
			Vector3 spawnPos = new Vector3(50, 50, 0); //spawn it away from the player so the EnemySpawner doesnt immediately trigger
			GameObject newRoom = Instantiate(RoomPrefabs[randomIndex], spawnPos, new Quaternion(0, 0, 0, 0)); //Get the prefab with said random index

			List<GameObject> roomDoors = newRoom.GetComponent<RoomScript>().RoomDoors; //Gets the doors of the new room that has been instantiated. Rooms may have "infinite" doors.
			GameObject newRoomRandomDoor = roomDoors[Random.Range(0, roomDoors.Count)]; //Get the actual door we try to connect to

			if (TryPlaceRoom(randomDoor, newRoomRandomDoor)) //This calls with a random already existing door and the door we just picked returns Bool
			{ //Successfully created a room:
				Rooms.Add(newRoom); //Room added to the rooms list
				UsedDoors.Add(AvailableDoors[randomDoorIndex]); //the doors that were used in the process. These shouldn't be used again
				UsedDoors.Add(newRoomRandomDoor); //2. door =""=
				newRoomRandomDoor.GetComponent<DoorScript>().LinkDoor(AvailableDoors[randomDoorIndex].GetComponent<DoorScript>());
				newRoomRandomDoor.GetComponent<DoorScript>().LockDoor();
				AvailableDoors.RemoveAt(randomDoorIndex); //Remove the used door that already existed from the Available doors
				foreach (GameObject door in roomDoors) //Iterate over all new doors that were added with the room
				{
					if (door != newRoomRandomDoor) //to not add the already used door
					{
						AvailableDoors.Add(door);//and add them to the available doors
					}
				}
				newRoom.GetComponent<RoomScript>().Depth = randomDoor.GetComponentInParent<RoomScript>().Depth + 1;
				//We expect the LayerManager to do its thing before the RoomManager (because first the Layer info gets generated, after that the Rooms get Generated based on that)
				newRoom.GetComponent<RoomScript>().IsReady = true;
			}
			else
			{ //Failed to create a room (due to overlap)
				i--; //add back to the counter of rooms to generate so we are not missing one
				newRoom.SetActive(false); //because the NavMesh still generated if an object is destroyed but was set to active before getting destroyed (wtf?????)
				Destroy(newRoom); //Discard the room that didn't fit and try again
			}

		}
		AddConnectedRooms();//If a random door has luckily aligned with another, we can have those set as "used" as well
		meshSurface.BuildNavMesh(); //after everything is generated, build the NavMesh for the Enemies
									//needs to get recalculated if new obstacles appear
		SetBossRoom();
		StartRoom.GetComponent<RoomScript>().ClearRoom();
		GameManager.roomsCleared--; //to prevent startroom counting as a cleared room (fuck this)
		SetRoomGroundSprite();
	}

	private void SetRoomGroundSprite()
	{
		List<GameObject> grounds = new List<GameObject>();
		foreach (GameObject room in Rooms)
		{
			foreach (Transform obj in room.transform)
			{
				if (obj.gameObject.layer == 6) //background
				{
					grounds.Add(obj.gameObject);
				}
			}
		}
		foreach (GameObject ground in grounds)
		{
			foreach (Transform square in ground.transform)
			{
				square.GetComponent<SpriteRenderer>().material.SetTexture("_Texture", LayerManager.CurrentLayer.GroundSprite);
				square.GetComponent<SpriteRenderer>().color = Color.white;
			}

		}
	}
	private void AlignRooms(GameObject doorA, GameObject doorB) //Now here comes the neat part
	{

		GameObject roomB = doorB.transform.parent.gameObject; //We only need the new room, the already existing room doesn't really matter

		Vector2 dirA = (doorA.GetComponent<DoorScript>().DoorFacing.position - doorA.GetComponent<DoorScript>().DoorMiddle.position).normalized; //We get the vectors of the doors middle to their facing points
		Vector2 dirB = (doorB.GetComponent<DoorScript>().DoorFacing.position - doorB.GetComponent<DoorScript>().DoorMiddle.position).normalized;

		float angleA = Mathf.Atan2(dirA.y, dirA.x) * Mathf.Rad2Deg; //Some math shit to get Angles
		float angleB = Mathf.Atan2(dirB.y, dirB.x) * Mathf.Rad2Deg;

		float targetRotation = (angleA + 180f) - angleB; //The rotation the room has to take so it aligns its door.
														 // The parentheses stay for my dyscalculate brain (Better explicit than implicit (Looking at you DeadLand and your random "void xxx" methodes))

		roomB.transform.rotation = Quaternion.Euler(0, 0, targetRotation); //Lets rotate that bitch

		Vector3 currentDoorBPos = doorB.transform.position;  //get the positon of door B
		Vector3 displacement = doorA.transform.position - currentDoorBPos; //Now we only need to calculate how much we should move it by substracting its position from where it has to go

		roomB.transform.position += displacement; //and move it there by translating the movement to the room.

	}

	private bool IsOverlapping(GameObject room) //Overlap = Bad
	{
		CompositeCollider2D boundsCollider = room.GetComponentInChildren<CompositeCollider2D>(); //Every Bounds has one of these. The bounds are also the floor of the room.
		if (boundsCollider == null) return false; //There was no collider found :( That shouldn't happen.

		Collider2D[] results = new Collider2D[10]; //Max rooms to be overlapped with. This is a magic Literal because it doesn't really matter how big it is but 10 seemed fitting considering how big the rooms are.
												   //This sorta limits how many rooms a single room can overlap with so we might or might not need to change this in the future.

		ContactFilter2D filter = new ContactFilter2D(); //Now lets filter to only look
		filter.SetLayerMask(LayerMask.GetMask("Background"));//for the specific layer
		filter.useTriggers = true; //and if the colliders are set to trigger. (If you use my Prefabs to build a room this should already be present)

		int found = boundsCollider.OverlapCollider(filter, results); //Now here comes the main line. This checks with the filter criteria if the bounds overlap with another and save all overlaps in the result array.

		for (int i = 0; i < found; i++) //Lets have a quick look into the array of overlapping rooms just to be sure we don't check the room with itself.
		{
			if (results[i].GetComponentInParent<RoomScript>().gameObject != room.GetComponentInParent<RoomScript>().gameObject) //Prevents to check if the room that is about to be placed is overlapping with itself
			{
				return true; //Then returns that an overlap was indeed found
			}
		}
		return false; //or if non was found
	}
	private bool TryPlaceRoom(GameObject doorA, GameObject doorB) //Let's try to place a room. YAY :D
	{
		AlignRooms(doorA, doorB); //First align those beautifully crafted rooms
		Physics2D.SyncTransforms(); //Force Unity to update the Transforms or it might take the old location
		if (IsOverlapping(doorB.transform.parent.gameObject)) //Let's see if it overlaps
		{
			return false; //It does. The TryPlaceRoom failed and it needs to be killed :(
		}
		return true; //It doesn't and the room was successfully placed to live a happy life :D
	}

	private void AddConnectedRooms() //If a random door has luckily aligned with another, we can have those set as "used" as well.
	{
		HashSet<GameObject> doorsToRemove = new HashSet<GameObject>(); //HashSet to not have Duplicate doors

		for (int i = 0; i < AvailableDoors.Count; i++) //Iterate over Still available doors
		{
			for (int j = i + 1; j < AvailableDoors.Count; j++) // i+1 to prevent checking the door with itself
			{
				GameObject doorA = AvailableDoors[i]; //Get the doors
				GameObject doorB = AvailableDoors[j];

				if (Vector3.Distance(doorA.transform.position, doorB.transform.position) < 0.08f) //Check distance between doors if they are really close/overlap
				{
					doorsToRemove.Add(doorA); //Add doors to later remove into the HashSet
					doorsToRemove.Add(doorB);

					if (!UsedDoors.Contains(doorA)) UsedDoors.Add(doorA); //if for good measure that doors really not get added twice and are not already present in usedDoors
					if (!UsedDoors.Contains(doorB)) UsedDoors.Add(doorB);

					doorA.GetComponent<DoorScript>().LinkDoor(doorB.GetComponent<DoorScript>());
					doorA.GetComponent<DoorScript>().LockDoor();

					//Debug.Log($"Connected accidental overlap: {doorA.name} and {doorB.name}");
				}
			}
		}

		foreach (GameObject door in doorsToRemove) //Iterating over all doors in the HashSet
		{
			AvailableDoors.Remove(door); //remove doors fr fr
		}
	}

	private void SetBossRoom()
	{
		GameObject highestDepthRoom = StartRoom;
		int highestDepthCount = 0;
		foreach (GameObject room in Rooms)
		{
			foreach (GameObject obs in room.GetComponent<RoomScript>().AllObstacles) //Rotate all obstacles to upright. Why do i do it here?
																					 //Because i already itterate over all rooms and thus dont need to do it after the generation again(saves one GameObject fetch)
			{
				obs.transform.rotation = Quaternion.identity;
			}

			room.GetComponent<RoomScript>().IsBossRoom = false;
			if (room.GetComponent<RoomScript>().Depth > highestDepthCount && room.GetComponent<RoomScript>().RoomDoors.FindAll((x) => x.GetComponent<DoorScript>().State != Enums.DoorState.Hidden).Count == 1)
			{
				highestDepthRoom = room;
				highestDepthCount = room.GetComponent<RoomScript>().Depth;
			}
		}
		if (highestDepthRoom == StartRoom)
		{
			foreach (GameObject room in Rooms)
			{
				if (room.GetComponent<RoomScript>().Depth > highestDepthCount)
				{
					highestDepthRoom = room;
					highestDepthCount = room.GetComponent<RoomScript>().Depth;
				}
			}
		}
		highestDepthRoom.GetComponent<RoomScript>().IsBossRoom = true;
		if (highestDepthRoom.GetComponent<RoomScript>().IsBossRoom)
		{
			foreach (GameObject obs in highestDepthRoom.GetComponent<RoomScript>().AllObstacles)
			{
				obs.SetActive(false);
			}
		}
	}

	public void SetMiniMap()
	{
		foreach (GameObject room in Rooms)
		{
			room.GetComponent<RoomScript>().SetMiniMap();
		}
	}
}
