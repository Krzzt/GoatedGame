
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveState
{

    //INVENTORY RELATED (in InventoryLogic)
    [field: SerializeField] public Item[] EquippedItems { get; set; }
    [field: SerializeField] public List<Item> InventoryItems { get; set; }

    //CARD RELATED (in DeckLogic)
    [field: SerializeField] public List<Card> CardsInHand { get; set; }
    [field: SerializeField] public List<Card> EntireDeck { get; set; }
    [field: SerializeField] public List<Card> DrawPile { get; set; }
    [field: SerializeField] public List<Card> DiscardPile { get; set; }

    //LAYER RELATED (in LayerManager)
    [field: SerializeField] public Layer ActiveLayer { get; set; }
    [field: SerializeField] public int LayerNumber { get; set; }

    //PLAYER RELATED (in Player)
    [field: SerializeField] public int EnemiesKilled { get; set; }
    [field: SerializeField] public int Level { get; set; }
    [field: SerializeField] public Class PlayerClass { get; set; }
    [field: SerializeField] public int CurrentHealth { get; set; }

    //GENERAL (in GameManager)
    [field: SerializeField] public int RoomsCleared { get; set; }
    [field: SerializeField] public int Seed {  get; set; }
    [field: SerializeField] public bool IsSeeded {  get; set; }
    [field: SerializeField] public int Credits { get; set; }

    //PlayerStats that can be calculated do not need to be saved --> example: BonusDamage, FireRate
    //except for EnemiesKilled
    //IMPORTANT FOR INVENTORYITEMS: HAVE TO BE EQUIPPED AGAIN FOR EVERYTHING TO WORK

    public SaveState()
    {
        EquippedItems = new Item[(int)Enums.SlotTag.None];
        InventoryItems = new List<Item>();
        CardsInHand = new List<Card>();
        EntireDeck = new List<Card>(); //maybe needs to be changed in the future to a dummy deck
        DrawPile = new List<Card>();
        DiscardPile = new List<Card>();
        ActiveLayer = null; //maybe needs to be changed to a dummy layer
        LayerNumber = 1;
        EnemiesKilled = 0;
        Level = 1; //Starting Level is 1
        PlayerClass = null; //no class given because that should happen externally in the Char Select
        CurrentHealth = 10; //just to not instantly die
        RoomsCleared = 0;
        Seed = 0;
        IsSeeded = false;
        Credits = 0; // Yay Money. WOOOOOO. (Name pending)
    }

    public SaveState(Class playerClass)
    {
        EquippedItems = playerClass.StartingItems;
        InventoryItems = new List<Item>();
        CardsInHand = new List<Card>();
        EntireDeck = new List<Card>(playerClass.StartingDeck); //maybe needs to be changed in the future to a dummy deck
        DrawPile = new List<Card>(playerClass.StartingDeck); //yippie we can draw cards
        DiscardPile = new List<Card>();
        ActiveLayer = null; //maybe needs to be changed to a dummy layer
        LayerNumber = 1;
        EnemiesKilled = 0;
        Level = 1; //Starting Level is 1
        PlayerClass = playerClass; //no class given because that should happen externally in the Char Select
        CurrentHealth = playerClass.StartingHealth; //just to not instantly die
        RoomsCleared = 0;
        Seed = 0;
        IsSeeded = false;
        Credits = 0; // Yay Money. WOOOOOO. (Name pending)
    }
}
