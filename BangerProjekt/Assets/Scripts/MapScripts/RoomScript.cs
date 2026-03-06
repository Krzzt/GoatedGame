using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class RoomScript : MonoBehaviour
{
   [field:SerializeField] public List<GameObject> RoomDoors{get; set;} //Doors this room has
   [field:SerializeField] public Enums.RoomState State {get; private set;} = Enums.RoomState.Uncleared; //The state of the room
   [field:SerializeField] public List<Transform> ObstacleSpots{get; set;}
   [field:SerializeField] public List<GameObject> AllObstacles{get; set;}
   [field:SerializeField] public bool IsBossRoom{get; set;} = false; //Gets true when its a boss room
   [field:SerializeField] public int Depth{get; set;} //This counts up with how far  from the start room we are
    public int Budget {get; set;}
   [field:SerializeField] public List<GameObject> EnemiesInRoom {get; set;}
   public static Action<List<GameObject>, int> SendEnemyList;
   [field:SerializeField] public float SpawnpointSpacingToEachOther {get; set;} = 2f; //Public since we may want to change this as layers get deeper (May be replaced by arithmetic with layer number)
   [field:SerializeField] public List<GameObject> Spawnpoints{get; set;} //List of all available Spawnpoints in this room
   //[field:SerializeField] private Camera mainCam; //For future use
   [field:SerializeField] private int numOfSpawnpoints = 100; //Can be set via inspector for each room defaults to 100 but since the spacing doesnt allow more in smaller rooms this may be changed
   [field:SerializeField] private float spawnpointSpacingToWall = 2f; //So no enemies spawn in a wall or outside of a room
   private Transform spawnpointContainer; //The container for all the spawn points. (No need to set it for each room since every room should have it, so it gets created)


    private void Awake() // used to prevent accidental overrides by the inspector
    {
        State = Enums.RoomState.Uncleared; //A room can never be cleared from the get go (Except the Start room which gets unlocked after room gen)
        foreach (Transform trans in gameObject.transform)
      {
         if (trans.gameObject.CompareTag("Obstacle"))
         {
            ObstacleSpots.Add(trans);
         }
      }
        //mainCam = Camera.main;
        spawnpointContainer = new GameObject("GeneratedSpawnpoints").transform; //Create the Gameobject where all the spawnpoints are saved into
        spawnpointContainer.SetParent(this.transform);   //Sets its parent to the current room so it is not a stray GameObject
        spawnpointContainer.localPosition = Vector3.zero; // Defaults it to zero in the room for good measure

        GenerateSpawnPoints(); //Generate the spawnpoints of the room
    }
   
    public bool IsCleared() //Returns a simple bool to check if the room is cleared. (Self explaining)
   {
      if(State == Enums.RoomState.Cleared)
      {
         return true;
      }
      return false;
   }
   [ContextMenu("Set Room As Cleared")] //Mostly for debug purposes and to replace still missing logic (Wave Manager)
   public void ClearRoom() //This name might change as clear room sounds a bit like it will be emptied but for now it just sets it as Cleared
   {
      State = Enums.RoomState.Cleared;
      foreach (GameObject door in RoomDoors)
      {
         if(door.GetComponent<DoorScript>().State == Enums.DoorState.Locked) //and unlocks the doors once it is
         {
            door.GetComponent<DoorScript>().OpenDoor();
         }
      }
   }

 
    public void OnRoomEnter() //Gets called by RoomBoundScript on each rooms bounds and that checks if the player is inside of the room
    {
      if (State == Enums.RoomState.Uncleared)
      {
         foreach (GameObject door in RoomDoors) //This locks the doors of the room so he may not leave until the room is cleared
         {
            if(door.GetComponent<DoorScript>().State == Enums.DoorState.Open)
            {
               door.GetComponent<DoorScript>().LockDoor();
            }
         }
            GameManager.SetCurrentRoom(this);
            Budget = Random.Range(15,40) * LayerManager.CurrentLayerNumber; //currently random number, not set in stone
            SendEnemyList?.Invoke(EnemiesInRoom, Budget);
            //send event to start enemy Spawn
      }
    }

    public void GenerateSpawnPoints() //Generate the Spawnpoints of the Room (Why am i even explaining self explanatory names?)
   {
      int tries = 0; //Helper variable to count up
      CompositeCollider2D roomBounds = GetComponentInChildren<CompositeCollider2D>(); //Get the collider (The only CompositCollider in the room is the one of the floor)
      Bounds bounds = roomBounds.bounds; //Get its bounds to later check the overlap
      while (tries < numOfSpawnpoints * 20 && numOfSpawnpoints > Spawnpoints.Count) 
      //"tries" prevents an infinite loop if the room is to small to fit 100 Spawnpoints and also checks if the desired number has been reached. The * 20 may change if it takes too long to generate
      {
         tries++; //Count it up real nice

         Vector2 possibleSpot = new Vector2( //Get a random position in the bounds of the room (Yes this is always a square but since its only generated once its ok to try randomly)
            Random.Range(bounds.min.x, bounds.max.x), //random x
            Random.Range(bounds.min.y, bounds.max.y)  //random y
         );
         if (!roomBounds.OverlapPoint(possibleSpot)) continue; //Check if the point is on the rooms bounds if not pick a new spot
         if (SpawnPointToCloseToForbiddenTag(possibleSpot)) continue; //Check if the point is far enough away from a "Wall" or "Door", maybe we will add another tag eventually (Obstacle)
         if (SpawnpointToCloseToAnother(possibleSpot)) continue; //Check if the point is too close to another so they spread more evenly

         GameObject newSpawnpoint = new GameObject("SpawnPoint_" + Spawnpoints.Count); //On success create a Spawnpoint with the number for easy finding in the inspector
         //No instantiation since the spawnpoint does not carry any scripts or is a complex thing in need of a Prefab (its also slightly faster than instantiation)
         newSpawnpoint.transform.SetParent(spawnpointContainer); //Put the Spawnpoint into the container,
         newSpawnpoint.transform.position = possibleSpot; //then set its position to what we figuered out
         newSpawnpoint.tag = "Spawnpoint"; //and lastly give it the Tag so it may be found more easily (for example in an ovelap circle or square with the camera or player)

         Spawnpoints.Add(newSpawnpoint); //Put it into the list for easy getting via code
      }
   }

   private void SetObstacles(List<GameObject> obstacleTypes)
   {
      foreach (Transform obstacleSpot in ObstacleSpots)
      {
         GameObject obstacle = Instantiate(obstacleTypes[Random.Range(0,obstacleTypes.Count)], obstacleSpot);
         AllObstacles.Add(obstacle);
      }
   }

    private void OnEnable()
    {
        RoomManager.sendObstacles += SetObstacles;
    }
    private void OnDisable()
    {
        RoomManager.sendObstacles -= SetObstacles;
    }
   private bool SpawnpointToCloseToAnother(Vector2 position) //Does exactly that what its named after
   {
      foreach(GameObject sp in Spawnpoints)
      {
         Vector2 spPos = sp.transform.position;
         if(Vector2.Distance(spPos, position) < SpawnpointSpacingToEachOther) return true;
      }
      return false;
   }

   private bool SpawnPointToCloseToForbiddenTag(Vector2 position) // the same with this.
   {
      Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, spawnpointSpacingToWall);

      foreach(Collider2D col in hitColliders)
      {
         if(col.CompareTag("Wall")|| col.CompareTag("Door"))
         {
            return true;
         }
      }
      return false;
   }

}

