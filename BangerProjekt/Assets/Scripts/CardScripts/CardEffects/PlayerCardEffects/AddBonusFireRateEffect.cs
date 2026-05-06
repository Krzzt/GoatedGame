using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/AddFireRateEffect")]
public class AddBonusFireRateEffect : CardEffect
{

    float value;
	public override void ExecuteEffect(string effect)
	{
		value = float.Parse(effect);
        Player.Instance.AddBonusFireRate(value);
	}

	public override void RevertEffect(string effect)
	{
		Player.Instance.AddBonusFireRate(-value);
	}
}
