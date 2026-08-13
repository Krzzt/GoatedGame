using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/AddPierce")]

public class AddBonusPierceEffect : CardEffect
{
	private int value;
	public override void ExecuteEffect(string effect)
	{
		value = int.Parse(effect);
		Player.Instance.BonusPierce += value;
	}

	public override void RevertEffect(string effect)
	{
		value = int.Parse(effect);
		Player.Instance.BonusPierce -= value;
	}
}
