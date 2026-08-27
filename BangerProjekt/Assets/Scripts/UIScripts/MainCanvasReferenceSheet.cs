using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvasReferenceSheet : MonoBehaviour
{
	[field: SerializeField] public GameObject ShopRelated { get; set; }
	[field: SerializeField] public GameObject InventoryRelated { get; set; }
	[field: SerializeField] public GameObject SkipWaveButton { get; set; }
	[field: SerializeField] public GameObject WaveText { get; set; }
	[field: SerializeField] public GameObject EnemiesAliveText { get; set; }
	[field: SerializeField] public GameObject CooldownIndicator { get; set; }
	[field: SerializeField] public GameObject AbilityImage { get; set; }
	[field: SerializeField] public GameObject CreditsText { get; set; }
	[field: SerializeField] public GameObject ShopButtonOutOfShop { get; set; }
	[field: SerializeField] public GameObject ShopButtonInShop { get; set; }
	[field: SerializeField] public GameObject GameOverScreen { get; set; }


	[field: Header("In Scene References")]
	[field: SerializeField] public UIManager UiManager { get; set; }

	public void ToggleShop()
	{
		UiManager.ToggleShop();
	}


}
