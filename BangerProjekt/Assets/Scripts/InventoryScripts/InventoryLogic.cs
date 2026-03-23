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
    public static Action<GameObject> SendNewWeapon;

    private AllItems allItemList;

    private void Awake()
    {
        allItemList = gameObject.GetComponent<AllItems>();
        InventoryItems = new List<Item>();


    }


    void Start()
    {
        
        ObtainItem(allItemList.Items[1]); //free dash
        EquipItem(0);
        ObtainItem(allItemList.Items[3]); //free Revolver??
        EquipItem(0);
        //UnEquipItem(4); if you want to start with fists :)
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

    public void ObtainItem(Item itemToGet)
    {
       if (InventoryItems.Count < MaxInventorySlots) //if there is space
        {
            InventoryItems.Add(itemToGet);
        }
        else //if there isnt
        {
            //probably need to change something here so items CAN lay on the ground. If someone purchases something with a full inv
            //this item needs to lay on the ground
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
        EquipItem(SelectedItem); //we call our equip item : )
        //and give the Selected Items slotNumber in the Inventory as an argument
    }

    public void EquipItem(int invIDToEquip)
    {
        Enums.SlotTag tagOfItem = InventoryItems[invIDToEquip].itemTag; //we get the ItemTag
        Debug.Log("The item is: " + InventoryItems[invIDToEquip].name);
        if (ItemsEquipped[(int)tagOfItem]) //if we already have something equipped at that tag
        {
            Item tempItemSave = ItemsEquipped[(int)tagOfItem];
            if (tempItemSave is WeaponItem) //if we have a weapon
            {
                Debug.Log("We equip a weapon wooooooooooo");
                WeaponItem tempWeapon = (WeaponItem)ItemsEquipped[(int)tagOfItem]; //hope this works
                ItemsEquipped[(int)tagOfItem] = InventoryItems[invIDToEquip]; //code dupe is forced sadly because of the events :()
                InventoryItems[invIDToEquip] = tempItemSave; 
                SendNewWeapon?.Invoke(tempWeapon.CorrespondingPrefab); //gets called in player btw
            }
            else //if we do not have a weapon
            {
                ChangeItemPlayerStats?.Invoke(ItemsEquipped[(int)tagOfItem], false); // false because we subtract the stats
                ItemsEquipped[(int)tagOfItem] = InventoryItems[invIDToEquip];
                InventoryItems[invIDToEquip] = tempItemSave;
                //standard Swap
                ChangeItemPlayerStats?.Invoke(ItemsEquipped[(int)tagOfItem], true); // true because we add the stats   
            }


        }
        else
        {
            EquipFreshItem(InventoryItems[invIDToEquip]); //if nothing is equipped, we call this method
        }


    }

    public void EquipFreshItem(Item itemToEquip)
    {
        ItemsEquipped[(int)itemToEquip.itemTag] = itemToEquip;
        InventoryItems.Remove(itemToEquip);
        if (itemToEquip is WeaponItem)
        {
            WeaponItem tempWeapon = (WeaponItem)itemToEquip; //hope this works
            Debug.Log("shotSpeed: " + tempWeapon.ShotSpeed);
            SendNewWeapon?.Invoke(tempWeapon.CorrespondingPrefab); //gets called in player btw
        }
        else
        {
            ChangeItemPlayerStats?.Invoke(ItemsEquipped[(int)itemToEquip.itemTag], true); // true because we add the stats
            //if nothing is equipped, we equip the one we have and increase our stats accordingly
            //this gets called when the Player has nothing equipped 
        }

    }
    public void UnEquipItem(int tagOfItemInt)
    {
        Item ItemToUnequip = ItemsEquipped[tagOfItemInt];
        InventoryItems.Add(ItemToUnequip);
        ItemsEquipped[tagOfItemInt] = null;
        if (ItemToUnequip is WeaponItem)
        {
            SendNewWeapon?.Invoke(null); //just dont send a new weapon the PlayerScript does the magic :)
        }
        else
        {
            ChangeItemPlayerStats?.Invoke(ItemsEquipped[tagOfItemInt], false); // false because subtract the stats
        }


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