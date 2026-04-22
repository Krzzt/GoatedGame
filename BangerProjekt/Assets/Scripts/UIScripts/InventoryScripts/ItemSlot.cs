using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    // [field: SerializeField] public Item Item { get; set; }
    [field: SerializeField] public int SlotId { get; set; }
    [field: SerializeField] public bool equipSlot { get; set; } = false;
    [field: SerializeField] public Enums.SlotTag SlotTag { get; set; }


    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject == null) return;
        ItemInSlot draggedItem = droppedObject.GetComponent<ItemInSlot>();
        if (draggedItem == null) return;
        ItemSlot sourceSlot = draggedItem.parentBeforeDrag.GetComponent<ItemSlot>();
        if (sourceSlot != null && sourceSlot.equipSlot && !equipSlot)
        {
            InventoryLogic.UnEquipItem((int)draggedItem.Item.ItemTag);
        }
        if (equipSlot && draggedItem.Item.ItemTag != SlotTag)
        {
            draggedItem.transform.SetParent(draggedItem.parentBeforeDrag);
            draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            return;
        }
        if (equipSlot && draggedItem.Item.ItemTag == SlotTag && !sourceSlot.equipSlot)
        {
            if (transform.childCount > 0)
            {
                InventoryLogic.UnEquipItem((int)SlotTag);
            }
            StartCoroutine(waitForFrame(draggedItem.Item));
               
            
        }
        if (transform.childCount > 0)
        {
            Transform currentItemInSlot = transform.GetChild(0);
            currentItemInSlot.SetParent(eventData.pointerDrag.GetComponent<ItemInSlot>().parentBeforeDrag);
            currentItemInSlot.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        draggedItem.parentBeforeDrag = transform;
        droppedObject.transform.SetParent(transform);
        droppedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    IEnumerator waitForFrame(Item draggedItem)
    {
        yield return new WaitForEndOfFrame();
        InventoryLogic.EquipItem(draggedItem);
    }

}
