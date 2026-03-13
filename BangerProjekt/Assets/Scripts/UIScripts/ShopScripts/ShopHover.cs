using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field:SerializeField] private Transform detailView;
    [field:SerializeField] private GameObject itemPrefab;

    void Start()
    {
        if (!transform.parent.gameObject.CompareTag("Shelf"))
        {
            detailView = GetComponentInParent<TableScript>().detailView;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.parent.gameObject.CompareTag("Shelf"))
        {
            GameObject item = Instantiate(itemPrefab, detailView, false);
            item.transform.Find("NameText").GetComponent<TMP_Text>().SetText(this.GetComponent<ItemSpotScript>().Item.name);
        }
        else
        {
            GameObject newCard = Instantiate(gameObject,detailView,false);
            newCard.transform.localScale = new Vector2(4,4);
            Destroy(newCard.GetComponent<ShopHover>());
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(detailView.childCount > 0){
            foreach(Transform child in detailView.transform)
            { 
            Destroy(child.gameObject);
            }
        }
    }
}
