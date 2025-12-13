using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class item // Creating a item class for the standart item
{

    public enum itemCategory
    {
        weapon,
        armour
    }   // Item Categories include armour, weapons
    public string itemName;
    public Sprite icon;
    [TextArea]

    public string description;
    

}

public class armour : item // creating a armour class with the required information for armour
{
    
    public float defense;

    public float healthBonus;

}



 public class weapon : item // creating the weapon class withe the required information about a weapon
{
    
    public float fireRate;

    public float damage;

    public float reloadTime;

    public int maxAmmunition;
    

}

public class dynamicInventory : MonoBehaviour // Creating an inventory class for an dynamic Inventory
{
    public int maxInventorySize = 20; 
    public List<itemInstance> items = new();

    public bool addItem(){ // a Function to add an item to the inventory with a test if there is enough inventory space
        
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null) 
            {
                
                items[i] = itemToAdd;
                return true;
            }
        }

    if(items.Count < maxInventorySize)
    {

        items.Add(itemToAdd);
        return true;
    }


    Debug.Log("No Space in the inventory");
    return false;

    }


    public void removeItem()
    {
        items.Remove(itemToRemove);
    }
}

public class inventoryDisplay : MonoBehaviour
{
    
    public dynamicInventory inventory;
    public itemDisplay[] slots;

    private void Start()
    {
        updateInventory();
    }

    void updateInventory() // a function that iterates thru the entire inventory to update the display of the items
    {
        for(int i = 0; i < slots.Length; i++){
            
            if(i < inventory.items.Count)
            {
                
                slots[i].gameObject.SetActive(true);
                slots[i].UpdateItemDisplay(inventory.items[i].itemType.icon, i);

            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }
}

