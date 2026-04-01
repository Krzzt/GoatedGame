using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/AbilityItem")]
public class AbilityItem : Item
{
   [field: SerializeField] public float AbilityCooldown {get; set;} //in seconds


    public override string BuildStatString()
    {
        StringBuilder sb = new StringBuilder();
        AddStat(sb, "Damage", Damage);
        AddStat(sb, "Fire Rate", FireRate);
        AddStat(sb, "Defense", Defense);
        AddStat(sb, "Health Bonus", HealthBonus);
        AddStat(sb, "Seconds Cooldown", AbilityCooldown); //how scuffed can it be
        return sb.ToString();
    }

    protected override void AddStat(StringBuilder sb, string label, float value, string suffix = "")
    {
        if (value != 0)
        {
            string prefix = "";
            if (label != "Seconds Cooldown")
            {
                prefix = value > 0 ? "+" : ""; //Add a + sign for positive values, - is implied for negative values
            }
            else
            {
                prefix = "";
            }
            sb.AppendLine($"{prefix}{value}{suffix} {label}"); //append to the string builder
        }
    }
}
