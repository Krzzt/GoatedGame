using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[CreateAssetMenu(menuName = "CardEffect/AbilityEffects/DashEffect")]
public class DashEffect : CardEffect
{
	private float duration;
	private float speedMult;
	public override void ExecuteEffect(string effect)
	{
		string[] strings = effect.Split(";");
		duration = float.Parse(strings[0], info.NumberFormat);
		speedMult = float.Parse(strings[1], info.NumberFormat);
		Dash();
		UseAbilities.Instance.AbilityDuration(this, duration,effect);
	}

	public void Dash()
	{
		Player.Instance.BonusMoveSpeed += speedMult;  //the player gets really fast
		movement.Instance.pc.Disable();
		Debug.Log(movement.Instance.rb.velocity);

	}

	public override void RevertEffect(string effect)
	{
		string[] strings = effect.Split(";");
		duration = float.Parse(strings[0], info.NumberFormat);
		speedMult = float.Parse(strings[1], info.NumberFormat);
		Player.Instance.BonusMoveSpeed -= speedMult;
		movement.Instance.pc.Enable();
	}
}
