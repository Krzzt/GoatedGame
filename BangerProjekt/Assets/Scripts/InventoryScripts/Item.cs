using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;




[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject // Creating a item class for the standard item
{   //weapon 
    public float fireRate;
    public int bulletAmount;

    public int damage;
    //armour
    public float defense;

    public int healthBonus;
    //item
    public int ID;
    public Enums.SlotTag itemTag; //Why are all of these public? *Shrug* Not my problem.
    public string itemName;
    public Sprite icon;

    public List<Enums.Class> itemClasses; //as a list in case an item should be usable for multiple, but not every class
    public Enums.Rarity itemRarity;
    public string description;
    [field:SerializeField] public int Cost{get; set;} //Pricy stuff
    void Awake()
    {
        if (itemName == null) Debug.Log("Item is missing Name!!!");
        if (itemTag == Enums.SlotTag.None) Debug.Log(itemName + " is missing a Tag!");
        if (ID < 0) Debug.Log(itemName + " has an invalid ID!");
        if (description == null) Debug.Log(itemName + " is missing a Description!");
        if (icon == null) Debug.Log(itemName + " is missing an Icon!");
    }

    public virtual string BuildStatString()
    {
        StringBuilder sb = new StringBuilder();
        AddStat(sb, "Damage", damage);
        AddStat(sb, "Fire Rate", fireRate);
        AddStat(sb, "Defense", defense);
        AddStat(sb, "Health Bonus", healthBonus);
        return sb.ToString();
    }
    protected virtual void AddStat(StringBuilder sb, string label, float value, string suffix = "")
    {
        if (value != 0)
        {
            string prefix = value > 0 ? "+" : ""; //Add a + sign for positive values, - is implied for negative values
            sb.AppendLine($"{prefix}{value}{suffix} {label}"); //append to the string builder
        }
    }
}
