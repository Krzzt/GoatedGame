using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{
    [field: SerializeField] public Item[] slots { get; set; }

    public void Init(int initSize)
    {
        slots = new Item[initSize];
    }

    public List<Item> Resize(int size)
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
        for (int i = 0; i < itemsToCopy; i++)
        {
            newSlots[i] = slots[i];
        }

        slots = newSlots;

        return overflow;
    }

    public bool TryAddItem(Item newItem)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) // Found an empty 'slot' in data
            {
                slots[i] = newItem;
                return true;
            }
        }
        return false; // Inventory full
    }

    public void SwapSlots(int slotA, int slotB)
    {
        Item slotAItem;
        Item slotBItem;

        slotAItem = slots[slotA];
        slotBItem = slots[slotB];

        slots[slotB] = slotAItem;
        slots[slotA] = slotBItem;
    }

    public void RemoveItem(int slot)
    {
        slots[slot] = null;
    }

    public void AddItemToSlot(Item newItem, int slot)
    {
        slots[slot] = newItem;
    }

}
