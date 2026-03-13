using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    private GameObject shopPanel;

    public static event Action<List<Item>> sendItemToShelf;
    public static event Action<List<Card>> sendCardsToTable;


    private void OnEnable()
    {
        LayerManager.sendLayer += NewLayer;
    }
    private void OnDisable()
    {
        LayerManager.sendLayer -= NewLayer;
    }
    void Awake()
    {
        shopPanel = GameObject.FindWithTag("Shop");
    }

    public void ToggleShop()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
        if (shopPanel.activeSelf)
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1;
        }
    }

    public void NewLayer(Layer layer)
    {
        Debug.Log(layer.name);
        List<Item> itemsToSend = new List<Item>();
        for(int i = 0; i < 4; i++)
        {
            itemsToSend.Add(layer.PossibleItems[Random.Range(0, layer.PossibleItems.Count)]);
        }
        Debug.Log("Trying to place items");
        sendItemToShelf?.Invoke(itemsToSend);

        List<Card> cardsToSend = new List<Card>();
        for(int i = 0; i < 5; i++)
        {
            cardsToSend.Add(layer.PossibleCards[Random.Range(0, layer.PossibleCards.Count)]);
        }
        Debug.Log("Trying to place cards");
        sendCardsToTable?.Invoke(cardsToSend); 
        ToggleShop();
    }
}
