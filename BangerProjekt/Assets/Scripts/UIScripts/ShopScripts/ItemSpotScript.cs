using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using Image =UnityEngine.UI.Image;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSpotScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item Item {get; set;} = null;
    Image image; 

    void Awake()
    {
        image = GetComponent<Image>();
    }
    public void SetItemOnShelf(Item item)
    {
        this.Item = item;
        
        image.sprite = item.icon;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.green;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.gray;
    }
}
