using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemList

{
    public static ItemList<Item> allItems = new List<Item>
    {
        new Armour {ID = 0, itemName="Rusty Helmet", itemCategory = "armour" , itemtag = slotTag.head  ,description = "some rust in helmet shape", itemRarity= item.rarity.Dev, defense=0.1 , healthBonus=0}

    };
}
public enum slotTag
{
    none,
    head,
    chest,
    legs,
    feet
}
public class Item // Creating a item class for the standart item
{

    public enum itemCategory
    {
        weapon,
        armour
    }   // Item Categories include armour, weapons

    public int ID;
    public slotTag itemtag;
    public string itemName;
    public Sprite icon;

    public enum rarity
    {
        common,
        uncommen,
        rare,
        epic,
        Legendary,
        Mythic,
        Special,
        Dev
    }

    public rarity itemRarity;
    public string description;
    

}

public class Armour : item // creating a armour class with the required information for armour
{
    
    public float defense;

    public float healthBonus;

}



 public class Weapon : item // creating the weapon class withe the required information about a weapon
{
    
    public float fireRate;

    public float damage;
    

}