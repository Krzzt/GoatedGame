using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
	public static PickupManager Instance;
	[field: SerializeField] public int DropChance;
	[SerializeField] private GameObject pickupPrefab;

	[field: SerializeField] public List<Pickup> PickupList;


	void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(this);
	}
	void OnEnable()
	{
		Enemy.enemyDies += CheckForPickupDrop;
		PickupScript.PickedUp += RevertEffect;
	}

	void OnDisable()
	{
		Enemy.enemyDies -= CheckForPickupDrop;
		PickupScript.PickedUp -= RevertEffect;
	}

	public void OnDestroy()
	{
		Instance = null;
	}

	void CheckForPickupDrop(GameObject droppingEnemy)
	{
		int doesDrop = Random.Range(1, 101); //between 1,100
		if (doesDrop <= DropChance)
		{
			Instantiate(pickupPrefab, droppingEnemy.transform.position, Quaternion.identity);
			//drop a Pickup;
		}
	}

	public void RevertEffect(float duration, List<Pair<CardEffect, string>> effectsToRevert)
	{
		StartCoroutine(WaitForRevert(duration, effectsToRevert));
	}

	public IEnumerator WaitForRevert(float duration, List<Pair<CardEffect, string>> effectsToRevert)
	{
		Debug.Log("reverting now...");
		Debug.Log("effects To Revert count: " + effectsToRevert.Count);
		Debug.Log("we will be waiting for " + duration + " seconds");
		yield return new WaitForSeconds(duration);
		Debug.Log("time is up, effect is gone");
		foreach (Pair<CardEffect, string> pair in effectsToRevert)
		{
			pair.First.RevertEffect(pair.Second);
		}
	}
}


