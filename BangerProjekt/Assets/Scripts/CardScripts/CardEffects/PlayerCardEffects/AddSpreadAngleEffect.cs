using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/AddSpreadAngleEffect")]
public class AddSpreadAngleEffect : CardEffect
{
    int value;

    public override void ExecuteEffect(string effect)
    {
       value = Int32.Parse(effect);
       Player.Instance.AddBonusSpreadAngle(value); 
    }

	public override void RevertEffect(string effect)
	{
		Player.Instance.AddBonusSpreadAngle(-value);
	}
}
