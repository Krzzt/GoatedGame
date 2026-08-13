using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
public class PickupScript : MonoBehaviour
{
	public static event Action<float, List<Pair<CardEffect, string>>> PickedUp;
	private Pickup pickupType;
	void Awake()
	{
		pickupType = PickupManager.Instance.PickupList[Random.Range(0, PickupManager.Instance.PickupList.Count)];
		gameObject.GetComponent<SpriteRenderer>().sprite = pickupType.PickupImage;
	}


	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			foreach (Pair<CardEffect, string> pair in pickupType.PickupEffects)
			{
				pair.First.ExecuteEffect(pair.Second);

			}
			if (pickupType.doesRevert) PickedUp?.Invoke(pickupType.durationInSeconds, pickupType.PickupEffects);
			Destroy(gameObject);
		}
	}
}
