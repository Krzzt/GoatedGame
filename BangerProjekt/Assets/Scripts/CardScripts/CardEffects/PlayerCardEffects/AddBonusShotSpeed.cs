using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/AddShotSpeed")]
public class AddBonusShotSpeed : CardEffect
{
	private float value;
	public override void ExecuteEffect(string effect)
	{
		value = float.Parse(effect, info.NumberFormat);
		Player.Instance.BonusShotSpeed += value;
	}

	public override void RevertEffect(string effect)
	{
		value = float.Parse(effect, info.NumberFormat);
		Player.Instance.BonusShotSpeed -= value;
	}
}
