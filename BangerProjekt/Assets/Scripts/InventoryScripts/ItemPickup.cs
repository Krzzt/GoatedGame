using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
	[field: SerializeField] public Item Item { get; set; }
	[field: SerializeField] public GameObject PopUp { get; set; }
	private SpriteRenderer SR;
	private TMP_Text text;
	private bool onItem = false;

	private InputAction interactAction;

	void OnEnable()
	{
		interactAction.performed += PickUpItem; //Subscribe to the action of the player
	}
	void OnDisable()
	{
		interactAction.performed -= PickUpItem; //Unsubscribe to prevent memory leaks
	}

	private void Awake()
	{
		interactAction = Player.playerInput.actions.FindAction("Interact"); //finding the player action
		SR = this.GetComponentInChildren<SpriteRenderer>(true);
		text = this.GetComponentInChildren<TMP_Text>(true);
	}
	public void setup(Item item) //Needs to be called by the thing creating the item pickup
	{
		Item = item;
		SR.sprite = item.Icon;
	}
	void OnTriggerEnter2D(Collider2D other) //Checks if the player is on the item. Might be replaced with spatial awareness so only the closest item gets picked up.
	//Currently all items that are stacked get collected
	{
		if (other.CompareTag("Player"))
		{
			string controlBinding = interactAction.GetBindingDisplayString(options: InputBinding.DisplayStringOptions.DontIncludeInteractions, group: Player.playerInput.currentControlScheme);
			text.SetText($"press '{controlBinding}' to pick up '{Item.ItemName}'");
			PopUp.SetActive(true);
			onItem = true;
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			PopUp.SetActive(false);
			onItem = false;
		}
	}
	private void PickUpItem(InputAction.CallbackContext context) //see the comment above about collecting all items on a stack
	{
		if(!onItem)return;
		if (InventoryLogic.ActiveInventory.TryAddItem(Item))
		{
			Destroy(this.gameObject);
		}
	}
}
