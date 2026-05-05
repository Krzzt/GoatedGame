using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
	[field: SerializeField] public Canvas Canvas { get; set; }
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
	//Thats a lot of things to set. Luckily most are prefabs and they live in the folders and not the editor itself so they dont need to be re set if we instantate a new Main canvas

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
	public void SetupInventory() //The magic happenes here. For the first time at least.
	{
		UpdateUi();
		foreach (Item item in InventoryLogic.ItemsEquipped) //looks at all the equpped items
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
	private void InstantiateItem(Item item, Transform parent) //The actual item instantiation of the prefab of the item in the inventory
	{
		GameObject IIS = Instantiate(ItemInInventoryPrefab, parent);
		IIS.GetComponent<ItemInSlot>().Item = item;
		IIS.GetComponent<Image>().sprite = item.Icon;
		IIS.name = item.name;
	}
	IEnumerator waitUntilItem() //To prevent race con with save manager. Maybe i change this to Frame in the future. Real time because the inventory starts disabled
	{
		yield return new WaitForSecondsRealtime(0.1f);
		content = this.GetComponentInChildren<ScrollRect>().GetComponentInChildren<GridLayoutGroup>().gameObject.transform;
		player = GameObject.FindGameObjectWithTag("Player");
		SetupInventory();
	}

	public void DropItem(Item itemToDrop, int slotId, bool draggedFromEquipSlot, GameObject parentBeforeDrag) //Dropping from the inventory
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
	public void DropItem(Item itemToDrop) //Generic if something should be dropped at the players position for some reason (Might be obsolete)
	{
		GameObject droppedItem = Instantiate(ItemPickupPrefab, player.transform.position, Quaternion.identity);
		droppedItem.GetComponent<ItemPickup>().setup(itemToDrop);
	}

	public void DropItem(Item itemToDrop, Transform droppedBy) //For chests or enemies that drop items
	{
		GameObject droppedItem = Instantiate(ItemPickupPrefab, droppedBy.position, Quaternion.identity);
		droppedItem.GetComponent<ItemPickup>().setup(itemToDrop);
	}

	private void UpdateUi() //This is not a final solution, i do not think i should have to seperate the child deletion and the recreation but that is how it works now
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
	public void OnDisable() //This needs to exist because maybe there is an internal race condition if i Destroy them in the same frame as i create new ones
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
