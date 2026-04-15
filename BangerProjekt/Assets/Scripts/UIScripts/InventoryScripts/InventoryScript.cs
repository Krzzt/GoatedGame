using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    [field: SerializeField] public Canvas Canvas { get; set; }
    [field: SerializeField] public int MaxInventorySlots { get; set; }
    [field: SerializeField] public List<Item> InventoryItems { get; set; }
    [field: SerializeField] public GameObject ItemInInventoryPrefab { get; set; }
    [field: SerializeField] public GameObject ItemViewPrefab { get; set; }
    [field: SerializeField] public GameObject ItemSlotPrefab { get; set; }

    [field: SerializeField] public GameObject[] InventorySlots { get; set; }
    private Transform content;


    private void Start()
    {
        MaxInventorySlots = InventoryLogic.Instance.MaxInventorySlots;
        content = this.GetComponentInChildren<GridLayoutGroup>().gameObject.transform;
        SetupInventory();
    }
    public void SetupInventory()
    {
        Debug.Log(MaxInventorySlots);
        InventoryItems = InventoryLogic.Instance.InventoryItems;
        for (int i = MaxInventorySlots; i > 0 ; i--)
        {
            Instantiate(ItemSlotPrefab,content);
        }
        
    }


}
