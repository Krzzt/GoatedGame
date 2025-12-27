using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;




[CreateAssetMenu]
public class Item : ScriptableObject // Creating a item class for the standart item
{   //weapon 
    public float fireRate;

    public float damage;
    //armour
    public float defense;

    public float healthBonus;
    //item
    public int ID;
    public Enums.SlotTag itemTag;
    public string itemName;
    public Sprite icon;


    public Enums.Rarity itemRarity;
    public string description;

}