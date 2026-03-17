using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;




[CreateAssetMenu]
public class Item : ScriptableObject // Creating a item class for the standard item
{   //weapon 
    public float fireRate;

    public int damage;
    //armour
    public float defense;

    public int healthBonus;
    //item
    public int ID;
    public Enums.SlotTag itemTag;
    public string itemName;
    public Sprite icon;

    public List<Enums.Class> itemClasses; //as a list in case an item should be usable for multiple, but not every class
    public Enums.Rarity itemRarity;
    public string description;
    void Awake()
    {
        if (itemName == null) Debug.Log("Item is missing Name!!!");
        if ((int)itemTag == 5) Debug.Log(itemName + " is missing a Tag!");
        if (ID < 0) Debug.Log(itemName + " has an invalid ID!");
        if (description == null) Debug.Log(itemName + " is missing a Description!");
        if (icon == null) Debug.Log(itemName + " is missing an Icon!");
    }
}
