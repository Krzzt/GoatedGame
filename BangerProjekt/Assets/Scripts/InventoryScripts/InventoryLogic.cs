using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryLogic : MonoBehaviour
{
    public static List<Item> InventoryItems { get; set; }//the acutal items
    [field: SerializeField] public static int MaxInventorySlots { get; set; } = 18; //amount of slots in the inv

    public static Action<Item> SendItem;

    [field: SerializeField] public static Item[] ItemsEquipped { get; set; } = new Item[(int)Enums.SlotTag.None]; //Serialized for testing

    public static Action<Item, bool> ChangeItemPlayerStats;
    public static Action<GameObject> SendNewWeapon;

    private AllItems allItemList;
    public static InventoryLogic Instance;

    private void Awake()
    {
        allItemList = gameObject.GetComponent<AllItems>();
        InventoryItems = new List<Item>();
    }


    void Start()
    {
        for (int i = 0; i < ItemsEquipped.Length; i++)
        {
            ItemsEquipped[i] = null;
            //reset all items, after that we can load them from save (still need to do tho D:)
        }
        ObtainItem(allItemList.Items[1]); //free dash
        ObtainItem(allItemList.Items[1]); //free dash
        EquipItem(InventoryItems[0]);
        ObtainItem(allItemList.Items[3]); //free Revolver??
        ObtainItem(allItemList.Items[3]); //free Revolver??
        ObtainItem(allItemList.Items[2]);
        ObtainItem(allItemList.Items[4]);
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

    public void EquipButton() //this should be used by the button that equips something
    {
        //EquipItem(SelectedItem); //we call our equip item : )
        //and give the Selected Items slotNumber in the Inventory as an argument
    }


    public static void EquipItem(Item itemToEquip)
    {
        ItemsEquipped[(int)itemToEquip.ItemTag] = itemToEquip;
        InventoryItems.Remove(itemToEquip);
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
        InventoryItems.Add(itemToUnequip);
        ItemsEquipped[tagOfItemInt] = null;
        if (itemToUnequip is WeaponItem)
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
        foreach (Item item in SaveManager.currentSave.EquippedItems)
        {
            if (item != null)
            {
                EquipItem(item);
            }

        }
    }
}