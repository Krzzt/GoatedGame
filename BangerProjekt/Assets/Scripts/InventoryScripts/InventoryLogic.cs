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

    [field: SerializeField] public static Item[] ItemsEquipped { get; set; } = new Item[(int)Enums.SlotTag.None]; //Serialized for testing

    public static Action<Item, bool> ChangeItemPlayerStats;

    private AllItems allItemList;

    private void Awake()
    {
        allItemList = gameObject.GetComponent<AllItems>();
        InventoryItems = new List<Item>();

        ObtainItem(allItemList.Items[1]);
        EquipItem(0);

    }

    private void OnEnable()
    {
        SaveManager.SavingGame += SaveInventory;
        SaveManager.LoadingGame += LoadInventory;
    }

    private void OnDisable()
    {
        SaveManager.SavingGame -= SaveInventory;
        SaveManager.LoadingGame -= LoadInventory;
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
            ChangeItemPlayerStats?.Invoke(ItemsEquipped[(int)tagOfItem], false); // false because we subtract the stats
            Item tempItemSave = ItemsEquipped[(int)tagOfItem];
            ItemsEquipped[(int)tagOfItem] = InventoryItems[invIDToEquip];
            InventoryItems[invIDToEquip] = tempItemSave;
            //standard Swap
            ChangeItemPlayerStats?.Invoke(ItemsEquipped[(int)tagOfItem], true); // true because we add the stats

        }
        else
        {
            EquipFreshItem(InventoryItems[invIDToEquip]); //if nothing is equipped, we call this method
        }


    }

    public void EquipFreshItem(Item itemToEquip)
    {
        ItemsEquipped[(int)itemToEquip.itemTag] = itemToEquip;
        ChangeItemPlayerStats?.Invoke(ItemsEquipped[(int)itemToEquip.itemTag], true); // true because we add the stats
        InventoryItems.Remove(itemToEquip);
       //if nothing is equipped, we equip the one we have and increase our stats accordingly
       //this gets called when the Player has nothing equipped
    }
    public void UnEquipItem(int tagOfItemInt)
    {
        InventoryItems.Add(ItemsEquipped[tagOfItemInt]);
        ChangeItemPlayerStats?.Invoke(ItemsEquipped[tagOfItemInt], false); // false because subtract the stats
        ItemsEquipped[tagOfItemInt] = null;
    }

    private void SaveInventory()
    {
        SaveManager.currentSave.InventoryItems = InventoryItems;
        SaveManager.currentSave.EquippedItems = ItemsEquipped;
    }

    private void LoadInventory()
    {
        InventoryItems = SaveManager.currentSave.InventoryItems;
        foreach(Item item in SaveManager.currentSave.EquippedItems)
        {
            if (item != null)
            {
                EquipFreshItem(item);
            } 

        }
    }
}