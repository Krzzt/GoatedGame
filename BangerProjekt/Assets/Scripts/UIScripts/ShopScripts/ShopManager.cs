using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private GameObject shopPanel;

    public static event Action<List<Item>> sendItemToShelf;
    public static event Action<List<Card>> sendCardsToTable;
    // Start is called before the first frame update


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
        ToggleShop();
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

    private void NewLayer(Layer layer)
    {
        
    }
}
