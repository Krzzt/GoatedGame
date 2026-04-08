using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class RoomScript : MonoBehaviour
{
    [field:SerializeField] public List<GameObject> RoomDoors{get; set;} //Doors this room has.
    [field:SerializeField] public Enums.RoomState State {get; private set;} = Enums.RoomState.Uncleared; //The state of the room
    [field:SerializeField] public bool IsBossRoom{get; set;} = false; //Gets true when its a boss room
    [field:SerializeField] public int Depth{get; set;} //This counts up with how far  from the start room we are
    [field:SerializeField] public int Budget {get; set;} 
    public static Action<int> StartWaves;
    public static Action SpawnBoss;
    public static Action RoomCleared;
    [field:SerializeField] public List<GameObject> AllObstacles{get; set;} //List of all obstacles in the room
    [field:SerializeField] public List<GameObject> Spawnpoints{get; set;} //List of all available Spawnpoints in this room
    //[field:SerializeField] private Camera mainCam; //For future use
    [field:SerializeField] private int numOfSpawnpoints = 100; //Can be set via inspector for each room defaults to 100 but since the spacing doesnt allow more in smaller rooms this may be changed
    [field:SerializeField] public float SpawnpointSpacingToEachOther {get; set;} = 2f; //Public since we may want to change this as layers get deeper (May be replaced by arithmetic with layer number)
    private float spawnpointSpacingToWall = 2f; //So no enemies spawn in a wall or outside of a room
    private int numOfObstacles = 10; //Can be set via inspector for each room defaults to 100 but since the spacing doesnt allow more in smaller rooms this may be changed
    private float obstaclesSpacingToEachOther = 7f; //Spacing between Obstacles
    private float obstaclesSpacingToWall = 5f; //Spacing between Obstacle and wall/Door

    private Transform spawnpointContainer; //The container for all the spawn points. (No need to set it for each room since every room should have it, so it gets created)
    private Transform obstacleContainer;
    private Transform LootPoint;
    private List<Obstacle> allAvailableObstacles;
    public bool IsReady {get; set;} = false;

    private void Awake() // used to prevent accidental overrides by the inspector
    {
        State = Enums.RoomState.Uncleared; //A room can never be cleared from the get go (Except the Start room which gets unlocked after room gen)
        foreach (Transform trans in gameObject.transform)
        {
           if (trans.gameObject.CompareTag("Door"))
            {
                RoomDoors.Add(trans.gameObject);
            }
            else if (trans.gameObject.CompareTag("LootPoint"))
            {
                LootPoint = trans;
            }

        }
        //mainCam = Camera.main;
        spawnpointContainer = new GameObject("GeneratedSpawnpoints").transform; //Create the Gameobject where all the spawnpoints are saved into
        spawnpointContainer.SetParent(this.transform);   //Sets its parent to the current room so it is not a stray GameObject
        spawnpointContainer.localPosition = Vector3.zero; // Defaults it to zero in the room for good measure
        obstacleContainer = new GameObject("GeneratedObstacles").transform; //Create the Gameobject where all the Obstacles are saved into
        obstacleContainer.SetParent(this.transform);   //Sets its parent to the current room so it is not a stray GameObject
        obstacleContainer.localPosition = Vector3.zero; // Defaults it to zero in the room for good measure
        if(LayerManager.CurrentLayer)
        allAvailableObstacles = LayerManager.CurrentLayer.PossibleObstacles;
        if(allAvailableObstacles != null && allAvailableObstacles.Count > 0 && this.gameObject.name != "StartRoom(Clone)")
        SetObstacles();
        GenerateSpawnPoints(); //Generate the spawnpoints of the room
        Debug.Log("Awake Called");
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
        RoomCleared?.Invoke();
        GameManager.roomsCleared++;
        if (IsBossRoom)
         {
         //also do all the fun stuff that boss rooms do like nextLayer shit and stuff
         }
        else if (LootPoint)//normal room gets normal chest while boss gets something cooler ig (except for startroom loser)
        {
            GameObject newLootChest = Instantiate(GameManager.Instance.LootChest, LootPoint.position, Quaternion.identity, LootPoint);
        }
   }

 
    public void OnRoomEnter() //Gets called by RoomBoundScript on each rooms bounds and that checks if the player is inside of the room
    {
      if (!(State == Enums.RoomState.Uncleared && IsReady))
      {
        return;
      }
      foreach (GameObject door in RoomDoors) //This locks the doors of the room so he may not leave until the room is cleared  
      {
        if (door.GetComponent<DoorScript>().State == Enums.DoorState.Open)
        {
            door.GetComponent<DoorScript>().LockDoor();
        }
      }
      GameManager.SetCurrentRoom(this);
      if (!IsBossRoom) //if its a "normal" room (so no boss room)
      {
        Budget = Random.Range(15, 41) * LayerManager.CurrentLayerNumber + GameManager.roomsCleared * 3; //currently random number, not set in stone
        StartWaves?.Invoke(Budget);
        //send event to start enemy Spawn
      }
      else //if we have a boss room
      {
      //spawn boss :o
      SpawnBoss?.Invoke();
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
         if (PointToCloseToForbiddenTag(possibleSpot, spawnpointSpacingToWall,false)) continue; //Check if the point is far enough away from a "Wall" or "Door", maybe we will add another tag eventually (Obstacle)
         if (PointToCloseToAnother(possibleSpot, SpawnpointSpacingToEachOther,false)) continue; //Check if the point is too close to another so they spread more evenly

         GameObject newSpawnpoint = new GameObject("SpawnPoint_" + Spawnpoints.Count); //On success create a Spawnpoint with the number for easy finding in the inspector
         //No instantiation since the spawnpoint does not carry any scripts or is a complex thing in need of a Prefab (its also slightly faster than instantiation)
         newSpawnpoint.transform.SetParent(spawnpointContainer); //Put the Spawnpoint into the container,
         newSpawnpoint.transform.position = possibleSpot; //then set its position to what we figuered out
         newSpawnpoint.tag = "Spawnpoint"; //and lastly give it the Tag so it may be found more easily (for example in an ovelap circle or square with the camera or player)

         Spawnpoints.Add(newSpawnpoint); //Put it into the list for easy getting via code
      }
   }

   private void SetObstacles()
   {
    Debug.Log("SettingObs");
    int tries = 0; //Helper variable to count up
    CompositeCollider2D roomBounds = GetComponentInChildren<CompositeCollider2D>(); //Get the collider (The only CompositCollider in the room is the one of the floor)
    Bounds bounds = roomBounds.bounds; //Get its bounds to later check the overlap
        while (tries < numOfObstacles * 20 && numOfObstacles > AllObstacles.Count) 
      //"tries" prevents an infinite loop if the room is to small to fit 100 Spawnpoints and also checks if the desired number has been reached. The * 20 may change if it takes too long to generate
      {
         tries++; //Count it up real nice

         Vector2 possibleSpot = new Vector2( //Get a random position in the bounds of the room (Yes this is always a square but since its only generated once its ok to try randomly)
            Random.Range(bounds.min.x, bounds.max.x), //random x
            Random.Range(bounds.min.y, bounds.max.y)  //random y
         );
         Obstacle newObstacleData = allAvailableObstacles[Random.Range(0, allAvailableObstacles.Count)];
         
         if (!roomBounds.OverlapPoint(possibleSpot)) continue; //Check if the point is on the rooms bounds if not pick a new spot
         if (PointToCloseToForbiddenTag(possibleSpot, obstaclesSpacingToWall + newObstacleData.GetWorldRadius(),true)) continue; //Check if the point is far enough away from a "Wall" or "Door", maybe we will add another tag eventually (Obstacle)
         if (PointToCloseToAnother(possibleSpot, obstaclesSpacingToEachOther + newObstacleData.GetWorldRadius(),true)) continue; //Check if the point is too close to another so they spread more evenly
         GameObject newObstacle = Instantiate(newObstacleData.ObstaclePrefab, possibleSpot, Quaternion.identity, obstacleContainer);
         newObstacle.GetComponent<ObstacleScript>().Obstacle = newObstacleData;
         newObstacle.GetComponent<ObstacleScript>().Setup();
         newObstacle.transform.localScale = newObstacleData.GetRequiredScale();
         AllObstacles.Add(newObstacle);
        
      }
      RoomManager.meshSurface.BuildNavMesh();
   }

   private bool PointToCloseToAnother(Vector2 position, float distanceToWhatever, bool IsObstacle) //Does exactly that what its named after
   {
    if (IsObstacle)
    {
        foreach(GameObject obs in AllObstacles)
            {
              Vector2 obsPos = obs.transform.position;
              if(Vector2.Distance(obsPos, position) < distanceToWhatever) return true;
            }
          return false;
    }
      foreach(GameObject sp in Spawnpoints)
      {
         Vector2 spPos = sp.transform.position;
         if(Vector2.Distance(spPos, position) < distanceToWhatever) return true;
      }
      return false;
   }

   private bool PointToCloseToForbiddenTag(Vector2 position,float distanceToWhatever, bool IsObstacle) // the same with this.
   {
      Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, distanceToWhatever);
        if (IsObstacle)
        {
        foreach(Collider2D col in hitColliders)
            {
         if(col.CompareTag("Wall") || col.CompareTag("Door"))
         {
            return true;
         }
            }
        }else{  
        foreach(Collider2D col in hitColliders)
        {
         if(col.CompareTag("Wall") || col.CompareTag("Door") || col.CompareTag("Obstacle"))
         {
            return true;
         }
        }
      }

      return false;
   }

    public void SetMiniMap()
    {
        bool shouldBeVisible = false;
        foreach (GameObject door in RoomDoors)
        {
            if (door.GetComponent<DoorScript>().State == Enums.DoorState.Open) //basically if any door is open in the room, it should be visible
            {
                shouldBeVisible = true;
            }
        }
        foreach(Transform obj in transform) //then we set every object to either visible or invisible, except for the BG, since it needs its own layer
        {
            if (obj.gameObject.layer != 6) //if we even want to change
            {
                if (shouldBeVisible)
                {
                    obj.gameObject.layer = 8;
                }
                else
                {
                    obj.gameObject.layer = 7;
                }
            }

        }
        List<GameObject> squares = new List<GameObject>(GameObject.FindGameObjectsWithTag("MiniMapSquare")).FindAll(g => g.transform.IsChildOf(this.transform));
        foreach (GameObject s in squares) //to find the BG, we find every MiniMapSquare Object in the Room itself, and set them individually
        {
            if (shouldBeVisible)
            {
                s.layer = 8;
            }
            else
            {
                s.layer = 7;
            }

            if (IsCleared()) //after that we give colors based upon the State of the room
            {
                s.GetComponent<SpriteRenderer>().color = new Color(0.035f, 0.566f, 0.16f,1f);
            }
            else
            {
                s.GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }



    }

    private void OnDrawGizmos()
    {
        if (AllObstacles != null)
        {
            Gizmos.color = Color.red;
            foreach (GameObject obs in AllObstacles)
            {
                if (obs == null) return;
                var script = obs.GetComponent<ObstacleScript>();
                if (script != null && script.Obstacle != null)
                {
                    // Draw the actual radius the algorithm is using
                    Gizmos.DrawWireSphere(obs.transform.position, script.Obstacle.GetWorldRadius());
                    Gizmos.DrawWireSphere(obs.transform.position, (float)obs.GetComponent<ObstacleScript>()?.Obstacle.ExplodeRange + (float)obs.GetComponent<ObstacleScript>()?.Obstacle.GetWorldRadius());
                }
            }
        }
    }

}

