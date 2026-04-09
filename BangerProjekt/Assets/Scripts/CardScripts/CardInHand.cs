using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CardInHand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    
    public Card CardSO {get; set;}

    private Vector3 moveVector = new Vector3(0,1,0);
    private Vector3 selectedScale = new Vector3(2,2,1);

    private GameObject bigView;

    public int cardInHandID {get; set;}

    public static Action<int> CardPlayed;

    private bool isHovering = false;

	void Awake()
	{
		bigView = gameObject.transform.parent.parent.GetChild(2).gameObject; //xd
	}

	public void OnPointerEnter(PointerEventData eventData)
    {
        //like make it bigger and make it selected
        transform.position += moveVector;
        transform.localScale = selectedScale;
        GameObject newCard = Instantiate(gameObject,bigView.transform,false); //Just copy the small card 
        newCard.transform.localScale = new Vector2(3,3); //and make it big
        Destroy(newCard.GetComponent<CardInHand>());
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //make it small again
        transform.position -= moveVector;
        transform.localScale = new Vector3(1,1,1);
        Destroy(bigView.transform.GetChild(0).gameObject);
        isHovering = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //play it?
        if (!isHovering) return;
        CardPlayed?.Invoke(cardInHandID);

    }
}
