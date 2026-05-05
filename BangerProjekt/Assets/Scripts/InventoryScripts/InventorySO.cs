using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{
	[field: SerializeField] public Item[] slots { get; set; }

	public void Init(int initSize) //Initialize the array. Necessary before usage
	{
		slots = new Item[initSize];
	}

	public List<Item> Resize(int size) //If inventory resized by any Upgrade/item etc. Needs the Value of slots that should be there.
	{
		List<Item> overflow = new List<Item>();

		if (size < slots.Length)
		{
			for (int i = size; i < slots.Length; i++)
			{
				if (slots[i] != null)
				{
					overflow.Add(slots[i]);
				}
			}
		}
		Item[] newSlots = new Item[size];
		int itemsToCopy = Mathf.Min(slots.Length, size);
		for (int i = 0; i < itemsToCopy; i++)//Copy to new array since Arrays are not dynamic like lists
		{
			newSlots[i] = slots[i];
		}

		slots = newSlots;

		return overflow; //Returns items that didn't fit into the new array if the inventory has been shrunk. Overflow needs to be handled by the Script that caused it.
	}

	public bool TryAddItem(Item newItem) //Function that returns false if the item was not able to be added to the inventory.
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (slots[i] == null)
			{
				slots[i] = newItem;
				return true;
			}
		}
		return false; // Inventory full
	}

	public void SwapSlots(int slotA, int slotB) //Self explaining (i know its inefficient because i make 2 helper Objects but who cares about these approx. 10 bytes of ram more or less)
	{
		Item slotAItem;
		Item slotBItem;

		slotAItem = slots[slotA];
		slotBItem = slots[slotB];

		slots[slotB] = slotAItem;
		slots[slotA] = slotBItem;
	}

	public Item RemoveItem(int slot) //Removes an item from a slot. Returns the item that was removed (can be ignored in cases and has been for now).
	{
		if (slot < 0 || slot >= slots.Length)
		{
			Debug.LogError("You dingus. That slot doesn't exist.");
			return null;
		}
		Item itemToReturn = slots[slot];
		slots[slot] = null;
		return itemToReturn;
	}


	public void AddItemToSlot(Item newItem, int slot) //Adds an item to a specific slot unlike TryAddItem (use with caution, overwrites existing items)
	{
		if (slot < 0 || slot >= slots.Length)
		{
			Debug.LogError("You dingus. That slot doesn't exist.");
			return;
		}
		slots[slot] = newItem;

	}
	//TODO: Inventory sorting
}
