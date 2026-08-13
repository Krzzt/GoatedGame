using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "CardEffect/AbilityEffects/FireRateAbilityEffect")]
public class FireRateAbilityEffect : CardEffect
{
	private float duration;
	private float fireRateMult;
	public override void ExecuteEffect(string effect)
	{
		string[] strings = effect.Split(";");
		duration = float.Parse(strings[0], info.NumberFormat);
		fireRateMult = float.Parse(strings[1], info.NumberFormat);
		Player.Instance.BonusFireRate += fireRateMult;
		UseAbilities.Instance.AbilityDuration(this, duration,effect);
	}

	public override void RevertEffect(string effect)
	{
		Player.Instance.BonusFireRate -= fireRateMult;
		movement.Instance.pc.Enable();
	}
}
