using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    [field: SerializeField] public Canvas Canvas { get; set; }
    [field: SerializeField] public int MaxInventorySlots { get; set; }
    [field: SerializeField] public List<Item> inventoryItems { get; set; }
    [field: SerializeField] public GameObject ItemInInventoryPrefab { get; set; }
    [field: SerializeField] public GameObject ItemViewPrefab { get; set; }
    [field: SerializeField] public GameObject ItemSlotPrefab { get; set; }
    [field: SerializeField] public InventoryLogic InvLogic { get; set; }
    [field: SerializeField] public GameObject ArmorSlot { get; set; }
    [field: SerializeField] public GameObject AccessorySlot { get; set; }
    [field: SerializeField] public GameObject WeaponSlot { get; set; }
    [field: SerializeField] public GameObject AbilitySlot { get; set; }
    private Transform content;


    private void Start()
    {
        StartCoroutine(waitUntilItem());

    }
    public void SetupInventory()
    {
        //Debug.Log(MaxInventorySlots);
        inventoryItems = InventoryLogic.InventoryItems;
        for (int i = 0; i < MaxInventorySlots; i++)
        {
            GameObject IS = Instantiate(ItemSlotPrefab, content);
            IS.GetComponent<ItemSlot>().SlotId = i;
        }
        int itemNum = 0;
        foreach (Item item in inventoryItems)
        {
            InstantiateItem(item, content.GetChild(itemNum));
            itemNum++;
        }
        foreach (Item item in InventoryLogic.ItemsEquipped)
        {
            if(!item)continue;
            Debug.Log(item.ItemName);
            GameObject parent = item.ItemTag switch
            {
                Enums.SlotTag.Ability => AbilitySlot,
                Enums.SlotTag.Accessory=> AccessorySlot,
                Enums.SlotTag.Weapon => WeaponSlot,
                Enums.SlotTag.Armor => ArmorSlot,
                _=> null
            };
            InstantiateItem(item, parent.transform);
        }

    }
    private void InstantiateItem(Item item, Transform parent)
    {
        GameObject IIS = Instantiate(ItemInInventoryPrefab, parent);
        IIS.GetComponent<ItemInSlot>().Item = item;
        IIS.GetComponent<Image>().sprite = item.Icon;
        IIS.name = item.name;
    }
    IEnumerator waitUntilItem()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        MaxInventorySlots = InventoryLogic.MaxInventorySlots;
        InvLogic = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryLogic>();
        content = this.GetComponentInChildren<ScrollRect>().GetComponentInChildren<GridLayoutGroup>().gameObject.transform;
        SetupInventory();
    }



}
