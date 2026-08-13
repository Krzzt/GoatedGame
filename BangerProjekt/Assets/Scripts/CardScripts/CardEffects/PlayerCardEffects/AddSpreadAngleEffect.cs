using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/AddSpreadAngleEffect")]
public class AddSpreadAngleEffect : CardEffect
{
	float value;

	public override void ExecuteEffect(string effect)
	{
		value = float.Parse(effect, info.NumberFormat);
		Player.Instance.BonusSpreadAngle += value;
	}

	public override void RevertEffect(string effect)
	{
		value = float.Parse(effect, info.NumberFormat);

		Player.Instance.BonusSpreadAngle -= value;
	}
}
