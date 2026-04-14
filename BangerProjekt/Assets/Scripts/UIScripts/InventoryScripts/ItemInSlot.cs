using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemInSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    public Item item;
    private Image image;
    private RectTransform rectTransform;
    private Transform parentAfterDrag;
    private CanvasGroup canvasGroup;
    [field:SerializeField] private Canvas canvas;

    void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = this.GetComponent<RectTransform>();
        canvasGroup = this.GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<InventoryScript>().Canvas;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
         rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
         
    }

	public void OnPointerClick(PointerEventData eventData)
	{
		Debug.Log("Clicked");
	}
}
