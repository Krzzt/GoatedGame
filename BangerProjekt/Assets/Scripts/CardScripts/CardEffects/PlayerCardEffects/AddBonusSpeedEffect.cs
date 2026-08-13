using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/AddBonusSpeedEffect")]
public class AddBonusSpeedEffect : CardEffect
{
	float value;
	public override void ExecuteEffect(string effect)
	{
		value = float.Parse(effect, info.NumberFormat);
		Player.Instance.BonusMoveSpeed += value;
	}

	public override void RevertEffect(string effect)
	{
		value = float.Parse(effect, info.NumberFormat);

		Player.Instance.BonusMoveSpeed -= value;
	}
}
