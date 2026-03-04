using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Layer : ScriptableObject
{
    [field:SerializeField] public int ID {get; set;}
    [field:SerializeField] public string Name {get; set;}
    [field:SerializeField] public List<GameObject> SpawnableEnemies {get; set;}
    [field:SerializeField] public List<GameObject> SpawnableExtraRooms {get; set;}
    [field:SerializeField] public List<int> PossibleLayers {get; set;}
}
