using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryLogic : MonoBehaviour
{
    public List<Item> InventoryItems {get; set;} //the acutal items
    [field:SerializeField] public int MaxInventorySlots {get; set;} //amount of slots in the inv
    public int SelectedItem {get; set;} //the number of the slot we have selected (first one is 0 etc.)

    public static Action<Item> SendItem;

    [field: SerializeField] public static List<Item> ItemsEquipped { get; set; } //Serialized for testing

    public static Action<Item, bool> ChangeItemPlayerStats;

    private AllItems allItemList;

    private void Awake()
    {
        allItemList = gameObject.GetComponent<AllItems>();
        InventoryItems = new List<Item>();
        ItemsEquipped = new List<Item>();
        for (int i = 0; i < (int)Enums.SlotTag.None; i++)
        {
            ItemsEquipped.Add(null);
        }
        ObtainItem(allItemList.Items[1]);
        EquipItem(0);

    }

    public void ObtainItem(Item itemToGet)
    {
       if (InventoryItems.Count < MaxInventorySlots) //if there is space
        {
            InventoryItems.Add(itemToGet);
        }
        else //if there isnt
        {
            Debug.Log("Inventory is full!");
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
        EquipItem(SelectedItem); //we search for the playerScript and call its equip function
        //and give the Selected Items slotNumber in the Inventory as an argument
    }

    public void EquipItem(int invIDToEquip)
    {
        Enums.SlotTag tagOfItem = InventoryItems[invIDToEquip].itemTag; //we get the ItemTag
        if (ItemsEquipped[(int)tagOfItem]) //if we already have something equipped at that tag
        {
            ChangeItemPlayerStats?.Invoke(ItemsEquipped[(int)tagOfItem], false); // false because subtract
            Item tempItemSave = ItemsEquipped[(int)tagOfItem];
            ItemsEquipped[(int)tagOfItem] = InventoryItems[invIDToEquip];
            InventoryItems[invIDToEquip] = tempItemSave;
            //standard Swap
            ChangeItemPlayerStats?.Invoke(ItemsEquipped[(int)tagOfItem], true); // true because we add

        }
        else
        {
            ItemsEquipped[(int)tagOfItem] = InventoryItems[invIDToEquip];
            ChangeItemPlayerStats?.Invoke(ItemsEquipped[(int)tagOfItem], true); // true because we add
            //if nothing is equipped, we equip the one we have and increase our stats accordingly
        }


    }
    public void UnEquipItem(int tagOfItemInt)
    {
        InventoryItems.Add(ItemsEquipped[tagOfItemInt]);
        ChangeItemPlayerStats?.Invoke(ItemsEquipped[tagOfItemInt], false); // false because subtract
        ItemsEquipped[tagOfItemInt] = null;
    }
}