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
    [field: SerializeField] public GameObject ArmorSlot { get; set; }
    [field: SerializeField] public GameObject AccessorySlot { get; set; }
    [field: SerializeField] public GameObject WeaponSlot { get; set; }
    [field: SerializeField] public GameObject AbilitySlot { get; set; }
    [field: SerializeField] public GameObject ItemPickupPrefab { get; set; }
    [field: SerializeField] public Transform ItemViewAnchor { get; set; }
    public static InventoryScript Instance { get; private set; }
    private GameObject player;
    private Transform content;


    private void Start()
    {
        StartCoroutine(waitUntilItem());
    }
    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(this);
        }
    }
    public void SetupInventory()
    {
        //Debug.Log(MaxInventorySlots);
        for (int i = 0; i < InventoryLogic.ActiveInventory.slots.Length; i++)
        {
            GameObject IS = Instantiate(ItemSlotPrefab, content);
            IS.GetComponent<ItemSlot>().SlotId = i;
            IS.name = $"Slot {i}";
        }
        for (int i = 0; i < InventoryLogic.ActiveInventory.slots.Length; i++)
        {
            if (InventoryLogic.ActiveInventory.slots[i] == null) continue;
            InstantiateItem(InventoryLogic.ActiveInventory.slots[i], content.GetChild(i));
        }
        foreach (Item item in InventoryLogic.ItemsEquipped)
        {
            if (!item) continue;
            //Debug.Log(item.ItemName);
            GameObject parent = item.ItemTag switch
            {
                Enums.SlotTag.Ability => AbilitySlot,
                Enums.SlotTag.Accessory => AccessorySlot,
                Enums.SlotTag.Weapon => WeaponSlot,
                Enums.SlotTag.Armor => ArmorSlot,
                _ => null
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
        yield return new WaitForSecondsRealtime(0.2f);
        MaxInventorySlots = InventoryLogic.InventorySlots;
        content = this.GetComponentInChildren<ScrollRect>().GetComponentInChildren<GridLayoutGroup>().gameObject.transform;
        player = GameObject.FindGameObjectWithTag("Player");
        SetupInventory();
    }

    public void DropItem(Item itemToDrop, int slotId, bool draggedFromEquipSlot, GameObject parentBeforeDrag)
    {
        if (draggedFromEquipSlot)
        {
           InventoryLogic.UnEquipItem((int)parentBeforeDrag.GetComponent<ItemSlot>().SlotTag);
        }
        else
        {
            InventoryLogic.ActiveInventory.RemoveItem(slotId); 
        }
        
        GameObject droppedItem = Instantiate(ItemPickupPrefab, player.transform.position, Quaternion.identity);
        droppedItem.GetComponent<ItemPickup>().setup(itemToDrop);
    }
    public void DropItem(Item itemToDrop)
    {
        GameObject droppedItem = Instantiate(ItemPickupPrefab, player.transform.position, Quaternion.identity);
        droppedItem.GetComponent<ItemPickup>().setup(itemToDrop);
    }

    public void DropItem(Item itemToDrop, Transform droppedBy)
    {
        GameObject droppedItem = Instantiate(ItemPickupPrefab, droppedBy.position, Quaternion.identity);
        droppedItem.GetComponent<ItemPickup>().setup(itemToDrop);
    }

    public void UpdateUi()
    {
        if (content != null)
        {
            for (int i = 0; i < InventoryLogic.ActiveInventory.slots.Length; i++)
            {
                GameObject IS = Instantiate(ItemSlotPrefab, content);
                IS.GetComponent<ItemSlot>().SlotId = i;
                IS.name = $"Slot {i}";
            }
            for (int i = 0; i < InventoryLogic.ActiveInventory.slots.Length; i++)
            {
                if (InventoryLogic.ActiveInventory.slots[i] == null) continue;
                InstantiateItem(InventoryLogic.ActiveInventory.slots[i], content.GetChild(i));
            }
        }
    }
    public void OnEnable()
    {
        UpdateUi();
    }
    public void OnDisable()
    {
        if (content != null)
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
