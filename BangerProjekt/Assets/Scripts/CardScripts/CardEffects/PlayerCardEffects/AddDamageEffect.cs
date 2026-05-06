using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/AddDamageEffect")]
public class AddDamageEffect : CardEffect
{
    private int value;
    public override void ExecuteEffect(string effect) 
    {
        value = Int32.Parse(effect);
        Player.Instance.AddBonusDamage(value);
    }

	public override void RevertEffect(string effect)
	{
        Player.Instance.AddBonusDamage(-value);
	}

}
