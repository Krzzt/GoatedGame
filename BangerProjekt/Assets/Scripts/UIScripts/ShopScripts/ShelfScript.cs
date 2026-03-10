using System.Collections.Generic;
using UnityEngine;

public class ShelfScript : MonoBehaviour
{
    [field:SerializeField] private GameObject spot1;

    private void OnEnable()
    {
        ShopManager.sendItemToShelf += PlaceItemsOnShelf;
    }
    private void OnDisable()
    {
        ShopManager.sendItemToShelf -= PlaceItemsOnShelf;
    }

    private void PlaceItemsOnShelf(List<Item> items)
    {
        if(items.Count < 4)
        {
            Debug.LogError("Not Enough Items were sent to be set in the Shop (Must be 4 or more)");
            return;
        }

        if (this.name == "Shelf1")
        {
            transform.GetChild(0).GetComponent<ItemSpotScript>().SetItemOnShelf(items[0]);
            transform.GetChild(1).GetComponent<ItemSpotScript>().SetItemOnShelf(items[1]);   
        }

        else if(this.name == "Shelf2")
        {
            transform.GetChild(0).GetComponent<ItemSpotScript>().SetItemOnShelf(items[2]);
            transform.GetChild(1).GetComponent<ItemSpotScript>().SetItemOnShelf(items[3]);   
        }

    }
}
