using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "CardEffect/TemporaryDamageUpEffect")]
public class TemporaryDamageUpEffect : CardEffect
{
	private float value;
	private float downPerRoom;
	public override void ExecuteEffect(string effect)
	{
		string[] strings = effect.Split(";");
		value += float.Parse(strings[0], info.NumberFormat);
		downPerRoom += float.Parse(strings[1], info.NumberFormat);
		Player.Instance.BonusDamage += value;
	}

	public override void OnRoomClear()
	{
		Player.Instance.BonusDamage -= downPerRoom;
		value -= downPerRoom;
	}

	public override void RevertEffect(string effect)
	{
		Player.Instance.BonusDamage -= value;
		//uhhhhhhh set damage to normal type shit
	}
}
