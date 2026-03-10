using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Layer : ScriptableObject
{
    [field:SerializeField] public int ID {get; set;}
    [field:SerializeField] public string Name {get; set;}
    [field:SerializeField] public List<GameObject> SpawnableEnemies {get; set;}
    [field:SerializeField] public List<GameObject> SpawnableExtraRooms {get; set;}
    [field:SerializeField] public List<int> PossibleLayers {get; set;}
    [field:SerializeField] public List<Item> PossibleItems {get; set;}
    [field:SerializeField] public List<Card> PossibleCards {get; set;}
    [field:SerializeField] public Sprite[] CardBackround {get; set;}
}
