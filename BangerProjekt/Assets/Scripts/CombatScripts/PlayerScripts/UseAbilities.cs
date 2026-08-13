using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Script is supposed to be on the Player GameObject and will only work if this is the case
public class UseAbilities : MonoBehaviour
{

	public static UseAbilities Instance;
	private Player playerScript;
	private movement movementScript;
	private Weapon weaponScript;

	[field: SerializeField] public float Cooldown { get; set; } //doesnt need a serializeField but its there to see if everything works (debugging basically)
	private bool isReady = true;
	public static Action<float> SetAbilityUI;


	void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(this);
		playerScript = gameObject.GetComponent<Player>();
		movementScript = gameObject.GetComponent<movement>();
		weaponScript = gameObject.GetComponent<Weapon>();
	}

	public void UseAbility()
	{
		if (!isReady) return;
		if (InventoryLogic.ItemsEquipped[(int)Enums.SlotTag.Ability] == null) return;
		AbilityItem currItem = InventoryLogic.ItemsEquipped[(int)Enums.SlotTag.Ability] as AbilityItem;
		foreach (Pair<CardEffect, string> p in currItem.AbilityEffectList)
		{
			p.First.ExecuteEffect(p.Second);
		}
		isReady = false;
		StartCoroutine(AbilityCooldown(Cooldown));
	}

	public void AbilityDuration(CardEffect effect, float duration, string effectString)
	{
		StartCoroutine(AbilityDurationCountdown(effect, duration, effectString));
	}

	public IEnumerator AbilityDurationCountdown(CardEffect effect, float duration, string effectString)
	{
		yield return new WaitForSeconds(duration);
		effect.RevertEffect(effectString);
	}

	public IEnumerator AbilityCooldown(float time) //call by value yessss
	{
		isReady = false;
		while (time > 0) //no yield return because we need to set the UI shit
		{
			time -= Time.fixedDeltaTime;
			SetAbilityUI?.Invoke(time / Cooldown);
			yield return new WaitForFixedUpdate();
		}
		yield return null;
		isReady = true;
	}
}
