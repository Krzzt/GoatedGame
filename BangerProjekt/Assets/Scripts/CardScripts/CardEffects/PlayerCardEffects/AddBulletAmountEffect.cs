using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/AddBulletAmountEffect")]
public class AddBulletAmountEffect : CardEffect
{
    int value;
	public override void ExecuteEffect(string effect)
	{
		value = Int32.Parse(effect);
        Player.Instance.AddBonusBulletAmount(value);
	}
	public override void RevertEffect(string effect)
	{
		Player.Instance.AddBonusBulletAmount(-value);
	}
}
