using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemInSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField] public Item Item { get; set; }
    private Image image;
    private RectTransform rectTransform;
    public Transform parentBeforeDrag { get; set; }
    private CanvasGroup canvasGroup;
    private GameObject itemHoverPrefab;
    private Transform itemHoverAnchor;
    [field: SerializeField] private Canvas canvas;

    void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = this.GetComponent<RectTransform>();
        canvasGroup = this.GetComponent<CanvasGroup>();
        InventoryScript invScript = GetComponentInParent<InventoryScript>();
        canvas = invScript.Canvas;
        itemHoverPrefab = invScript.ItemViewPrefab;
        itemHoverAnchor = invScript.ItemViewAnchor;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentBeforeDrag = transform.parent;
        //Debug.Log(parentBeforeDrag.gameObject.name);
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        Debug.Log(parentBeforeDrag.gameObject.name);
        if (!eventData.pointerCurrentRaycast.gameObject || !eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>())
        {
            transform.SetParent(parentBeforeDrag);
            rectTransform.anchoredPosition = Vector2.zero;
        }

    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked");
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemSlot isc = GetComponentInParent<ItemSlot>();
        isc.OnDrop(eventData);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject itemView = Instantiate(itemHoverPrefab, itemHoverAnchor, false);
            itemView.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            TMP_Text nameText = itemView.transform.Find("NameText").GetComponent<TMP_Text>();
            TMP_Text descriptionText = itemView.transform.Find("DescriptionText").GetComponent<TMP_Text>();
            TMP_Text statText = itemView.transform.Find("StatText").GetComponent<TMP_Text>();
            nameText.SetText(Item.name);
            nameText.font = GameManager.Instance.GameFont;
            descriptionText.SetText(Item.Description);
            descriptionText.font = nameText.font;
            statText.SetText(Item.BuildStatString());
            statText.font = nameText.font;
            itemView.transform.Find("ItemImage").GetComponent<Image>().sprite = Item.Icon;
            
    }
    public void OnPointerExit(PointerEventData eventData)
    {
            if(itemHoverAnchor.childCount > 0){
            foreach(Transform child in itemHoverAnchor.transform)
            { 
            Destroy(child.gameObject);
            }
        }
    }
}
