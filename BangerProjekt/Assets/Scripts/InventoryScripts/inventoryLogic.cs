using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;


public class InventoryItem : MonoBehaviour //, IPointerClickHandler
{ /*
    public CanvasGroup canvasGroup;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
    
}
public class DynamicInventory : MonoBehaviour // Creating an inventory class for an dynamic Inventory
{
    [SerializeField]public int maxInventorySize; 
    public List<Item> items = new();

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

public class ItemDisplay
{
    
}

public class InventoryDisplay : MonoBehaviour
{
    
    public DynamicInventory inventory;
    public ItemDisplay[] slots;

    private void Start()
    {
     updateInventory();
    }

   private void updateInventory() // a function that iterates thru the entire inventory to update the display of the items
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
*/
}
