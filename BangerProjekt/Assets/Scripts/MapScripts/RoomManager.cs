using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
   public List<GameObject> roomPrefabs; //List of all room prefabs available
   public List<GameObject> rooms; //List of all rooms in the current layer
   public List<GameObject> doors; //List of all doors in the current layer
   public GameObject startRoom; //The starting room prefab

   public int numOfRooms = 1; //Number of rooms to generate in the layer

    public void Awake()
    {
        GameObject StartRoom = Instantiate(startRoom, Vector3.zero, Quaternion.identity);
        doors.Add(GameObject.FindWithTag("Door"));

    }

    [ContextMenu("Generate Rooms")]
   public void GenerateRooms()
   {
       for (int i = 0; i < numOfRooms; i++)
       {
           int randomIndex = Random.Range(0, roomPrefabs.Count);
           GameObject newRoom = Instantiate(roomPrefabs[randomIndex], doors[i].transform.position, Quaternion.identity);
           AlignRooms(rooms[i], newRoom);
           rooms.Add(newRoom);
       }
   }

   public void AlignRooms(GameObject roomA, GameObject roomB)
   {
       //Implementation for aligning doors between rooms
   }
}
