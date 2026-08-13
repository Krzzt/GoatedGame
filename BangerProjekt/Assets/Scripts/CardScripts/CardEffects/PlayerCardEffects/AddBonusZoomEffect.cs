using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardEffect/AddBonusZoomEffect")]
public class AddBonusZoomEffect : CardEffect
{
	int value;
	public override void ExecuteEffect(string effect)
	{
		value = int.Parse(effect);
		Player.Instance.BonusZoom += value;
	}

	public override void RevertEffect(string effect)
	{
		value = int.Parse(effect);
		Player.Instance.BonusZoom -= value;
	}
}
