using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryLogic : MonoBehaviour
{
    public List<Item> InventoryItems {get; set;} //the acutal items
    [field:SerializeField] public int MaxInventorySlots {get; set;} //amount of slots in the inv
    public int SelectedItem {get; set;} //the number of slot we have selected (first one is 0 etc.)

    public void ObtainItem(Item itemToGet)
    {
       if (InventoryItems.Count < MaxInventorySlots) //if there is space
        {
            InventoryItems.Add(itemToGet);
        }
        else //if there isnt
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
        GameObject.FindWithTag("Player").GetComponent<Player>().EquipItem(SelectedItem); //we search for the playerScript and call its equip function
        //and give the Selected Items slotNumber in the Inventory as an argument
    }


}