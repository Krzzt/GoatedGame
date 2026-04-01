using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;




[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject // Creating a item class for the standard item
{   //weapon 
    [field: SerializeField] public float FireRate { get; set; }
    [field: SerializeField] public int BulletAmount { get; set; }

    [field: SerializeField] public int Damage { get; set; }
    //armour
    [field: SerializeField] public float Defense { get; set; }

    [field: SerializeField] public int HealthBonus { get; set; }
    //item
    [field: SerializeField] public int ID { get; set; }
    [field: SerializeField] public Enums.SlotTag ItemTag { get; set; }
    [field: SerializeField] public string ItemName { get; set; }
    [field: SerializeField] public Sprite Icon { get; set; }

    [field: SerializeField] public List<Enums.Class> ItemClasses { get; set; } //as a list in case an item should be usable for multiple, but not every class
    [field: SerializeField] public Enums.Rarity ItemRarity { get; set; }
    [field: SerializeField] public string Description { get; set; }
    [field:SerializeField] public int Cost{get; set;} //Pricy stuff
    void Awake()
    {
        if (ItemName == null) Debug.Log("Item is missing Name!!!");
        if (ItemTag == Enums.SlotTag.None) Debug.Log(ItemName + " is missing a Tag!");
        if (ID < 0) Debug.Log(ItemName + " has an invalid ID!");
        if (Description == null) Debug.Log(ItemName + " is missing a Description!");
        if (Icon == null) Debug.Log(ItemName + " is missing an Icon!");
    }

    public virtual string BuildStatString()
    {
        StringBuilder sb = new StringBuilder();
        AddStat(sb, "Damage", Damage);
        AddStat(sb, "Fire Rate", FireRate);
        AddStat(sb, "Defense", Defense);
        AddStat(sb, "Health Bonus", HealthBonus);
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
