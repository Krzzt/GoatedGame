using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Pickup : ScriptableObject
{
	[field: SerializeField] public int ID;
	[field: SerializeField] public string Name;
	[field: SerializeField] public Sprite PickupImage;
	[field: SerializeField] public float durationInSeconds;

	[field: SerializeField] public bool doesRevert;//
	[field: SerializeField] public List<Pair<CardEffect, string>> PickupEffects;
}
