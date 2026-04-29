using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/WeaponItem")]
public class WeaponItem : Item
{
    [field: SerializeField] public float DamageMult {get; set;}
    [field:SerializeField] public float ShotDelayOrRange {get; set;}
    [field:SerializeField] public int SpreadAngle {get; set;}
    [field:SerializeField] public int ShotSpeed {get; set;}
    [field:SerializeField] public float CritChance{get; set;}
    [field:SerializeField] public float CritDamage{get; set;}
    [field: SerializeField] public float LifeSteal{get; set;}
    [field: SerializeField] public int BulletPierce{get; set;}
    // TEST?
    [field: SerializeField] public int BulletBounces{get; set;}
    [field: SerializeField] public int defense{get; set;}
    [field: SerializeField] public int healthBonus{get; set;}


    [field: SerializeField] public GameObject BulletPrefab {get; set;}
    [field:SerializeField] public GameObject CorrespondingPrefab {get; set;}
    

    public override string BuildStatString()
    {
        StringBuilder sb = new StringBuilder();
        AddStat(sb, "Damage", Damage);
        AddStat(sb, "Fire Rate", FireRate);
        AddStat(sb, "ShotSpeed", ShotSpeed);
        //maybe something for Spears with range or some shit but i dont care i hate this shit :)
        AddStat(sb, "Defense", defense);
        AddStat(sb, "Health Bonus", healthBonus);
        AddStat(sb, "CritChance", CritChance);
        AddStat(sb, "CritDamage", CritDamage);
        AddStat(sb, "Lifesteal", LifeSteal);
        //i guess if some weapons wanna do it go for it???
        return sb.ToString();
    }

    protected override void AddStat(StringBuilder sb, string label, float value, string suffix = "")
    {
        if (value != 0)
        {
            string prefix = ""; //we do not have a plus as its stats stand for the weapon
            sb.AppendLine($"{prefix}{value}{suffix} {label}"); //append to the string builder
        }
    }
}
