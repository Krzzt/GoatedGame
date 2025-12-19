using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryLogic : MonoBehaviour
{
    private List<Item> inventoryItems = new List<Item>();
    [field:SerializeField] public int MaxInventorySlots {get; set;}

    public void ObtainItem(Item itemToGet)
    {
       if (inventoryItems.Count < MaxInventorySlots)
        {
            inventoryItems.Add(itemToGet);
        }
        else
        {
            Debug.Log("inv is full!");
        }
    }

    public void RemoveItem(int idSlotToRemove)
    {
        inventoryItems.RemoveAt(idSlotToRemove);
    }





}
