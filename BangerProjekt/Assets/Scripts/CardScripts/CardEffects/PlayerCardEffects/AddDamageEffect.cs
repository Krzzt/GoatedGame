using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/AddDamageEffect")]
public class AddDamageEffect : CardEffect
{
	private float value;
	public override void ExecuteEffect(string effect)
	{
		value = float.Parse(effect, info.NumberFormat);
		Player.Instance.BonusDamage += value;
	}

	public override void RevertEffect(string effect)
	{
		value = float.Parse(effect, info.NumberFormat);

		Player.Instance.BonusDamage -= value;
	}

}
