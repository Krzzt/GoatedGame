using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Class : ScriptableObject
{
	[field: SerializeField] public string Name { get; set; }
	[field: SerializeField] public Item[] StartingItems { get; set; }
	[field: SerializeField] public List<Card> StartingDeck { get; set; }
	[field: SerializeField] public float StartingBonusDamage { get; set; }
	[field: SerializeField] public float StartingBonusFireRate { get; set; }
	[field: SerializeField] public int StartingHealth { get; set; }
	[field: SerializeField] public int StartingMoveSpeed { get; set; }

	[field: SerializeField] public float StartingBonusShotSpeed { get; set; }
	[field: SerializeField] public float StartingBonusMoveSpeed { get; set; }
	[field: SerializeField] public float StartingBonusCritChance { get; set; }
	[field: SerializeField] public float StartingBonusCritDamage { get; set; }

}
