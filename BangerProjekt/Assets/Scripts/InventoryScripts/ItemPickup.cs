using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [field: SerializeField] public Item Item { get; set; }
    [field: SerializeField] public SpriteRenderer SR { get; set; }
    [field: SerializeField] public GameObject PopUp { get; set; }

}
