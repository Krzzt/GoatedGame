using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
    [field: SerializeField] public Item Item { get; set; }
    [field: SerializeField] public GameObject PopUp { get; set; }
    private SpriteRenderer SR;
    private TMP_Text text;

    private InputAction interactAction;

    private void Awake()
    {   
        interactAction = Player.playerInput.actions.FindAction("Interact");
        SR = this.GetComponentInChildren<SpriteRenderer>(true);
        text = this.GetComponentInChildren<TMP_Text>(true);
    }
    public void setup(Item item)
    {
        Item = item;
        SR.sprite = item.Icon;
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string controlBinding = interactAction.GetBindingDisplayString(options:InputBinding.DisplayStringOptions.DontIncludeInteractions,group: Player.playerInput.currentControlScheme);
            text.SetText($"press '{controlBinding}' to pick up '{Item.ItemName}'");
            PopUp.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PopUp.SetActive(false);
        }
    }

}
