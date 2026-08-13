using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/HealEffect")]
public class HealEffect : CardEffect
{
	int value;
	public override void ExecuteEffect(string effect)
	{
		value = int.Parse(effect, info.NumberFormat);
		Player.Instance.HealUnit(value);
	}

	public override void RevertEffect(string effect)
	{
		//no revert
	}
}
