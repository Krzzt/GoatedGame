using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryLogic : MonoBehaviour
{
    public List<Item> InventoryItems {get; set;}
    [field:SerializeField] public int MaxInventorySlots {get; set;}
    public int SelectedItem {get; set;}

    public void ObtainItem(Item itemToGet)
    {
       if (InventoryItems.Count < MaxInventorySlots)
        {
            InventoryItems.Add(itemToGet);
        }
        else
        {
            Debug.Log("inv is full!");
        }
    }

    public void RemoveItem(int idSlotToRemove)
    {
        InventoryItems.RemoveAt(idSlotToRemove);
    }

    public void SelectItem(int idToSet)
    {
        SelectedItem = idToSet;
    }

    public void EquipButton() //this should be used by the button that equips something
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().EquipItem(SelectedItem);
    }


}
