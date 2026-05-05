using System;
using UnityEngine;

public class InventoryLogic : MonoBehaviour
{
	[field: SerializeField] public static Item[] ItemsEquipped { get; set; } = new Item[(int)Enums.SlotTag.None]; //Serialized for testing
	[field: SerializeField] public static int InventorySlots { get; set; } = 18; //amount of slots in the inv
	[field: SerializeField] public Inventory InventoryBlueprint { get; set; }
	public static Inventory ActiveInventory;
	public static Action<Item> SendItem;
	public static Action<Item, bool> ChangeItemPlayerStats;
	public static Action<GameObject> SendNewWeapon;
	public static InventoryLogic Instance;
	private AllItems allItemList;


	private void Awake()
	{
		ActiveInventory = Instantiate(InventoryBlueprint);
		allItemList = gameObject.GetComponent<AllItems>();
	}

	void Start()
	{
		ActiveInventory.Init(InventorySlots);
		for (int i = 0; i < ItemsEquipped.Length; i++)
		{
			ItemsEquipped[i] = null;
			//reset all items, after that we can load them from save
		}
		ObtainItem(allItemList.Items[1]); //free dash
		ObtainItem(allItemList.Items[1]); //free dash
		EquipItem(ActiveInventory.slots[0]); // This is the only line that matters if you start with a save file (so if you start from Title Screen)
		/*ObtainItem(allItemList.Items[3]); //free Revolver??
        ObtainItem(allItemList.Items[3]); //free Revolver??
        ObtainItem(allItemList.Items[2]);
        ObtainItem(allItemList.Items[4]);*/
		//EquipItem(0);
		//UnEquipItem(2); //if you want to start with fists :)
	}
	private void OnEnable()
	{
		SaveManager.SavingGame += SaveInventory;
		SaveManager.LoadingGame += LoadInventory;
		ShopHover.purchaseItem += ObtainItem;
	}

	private void OnDisable()
	{
		SaveManager.SavingGame -= SaveInventory;
		SaveManager.LoadingGame -= LoadInventory;
		ShopHover.purchaseItem -= ObtainItem;
	}

	public static void ObtainItem(Item itemToGet)
	{
		if (!ActiveInventory.TryAddItem(itemToGet))
		{
			InventoryScript.Instance.DropItem(itemToGet);
		}
	}


	public void EquipButton() //this should be used by the button that equips something
	{
		//EquipItem(SelectedItem); //we call our equip item : )
		//and give the Selected Items slotNumber in the Inventory as an argument
	}


	public static void EquipItem(Item itemToEquip)
	{
		ItemsEquipped[(int)itemToEquip.ItemTag] = itemToEquip;
		if (itemToEquip is WeaponItem)
		{
			WeaponItem tempWeapon = itemToEquip as WeaponItem; //this works man this is scuffed
			SendNewWeapon?.Invoke(tempWeapon.CorrespondingPrefab); //gets called in player btw
		}
		else
		{
			ChangeItemPlayerStats?.Invoke(ItemsEquipped[(int)itemToEquip.ItemTag], true); // true because we add the stats
																						  //if nothing is equipped, we equip the one we have and increase our stats accordingly
																						  //this gets called when the Player has nothing equipped
		}
	}

	public static void UnEquipItem(int tagOfItemInt)
	{
		Item itemToUnequip = ItemsEquipped[tagOfItemInt];
		if (itemToUnequip is WeaponItem)
		{
			SendNewWeapon?.Invoke(null); //just dont send a new weapon the PlayerScript does the magic :)
		}
		else
		{
			ChangeItemPlayerStats?.Invoke(ItemsEquipped[tagOfItemInt], false); // false because subtract the stats
		}
		ItemsEquipped[tagOfItemInt] = null;
	}

	private void SaveInventory()
	{
		SaveManager.currentSave.InventoryItems = ActiveInventory.slots;
		SaveManager.currentSave.EquippedItems = ItemsEquipped;
	}

	private void LoadInventory()
	{
		ActiveInventory.slots = SaveManager.currentSave.InventoryItems;

		foreach (Item item in SaveManager.currentSave.EquippedItems)
		{
			if (item != null)
			{
				EquipItem(item);
			}

		}
	}
}
