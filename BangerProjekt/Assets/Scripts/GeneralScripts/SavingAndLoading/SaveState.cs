
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveState
{
    //[field:SerializeField] public List<GameObject> CurrentLayerRooms { get; set; }
    [field: SerializeField] public Item[] EquippedItems { get; set; }
    [field: SerializeField] public List<Item> InventoryItems { get; set; }
    [field: SerializeField] public List<Card> CardsInHand { get; set; }
    [field: SerializeField] public List<Card> EntireDeck { get; set; }
    [field: SerializeField] public List<Card> DrawPile { get; set; }
    [field: SerializeField] public List<Card> DiscardPile { get; set; }
    [field: SerializeField] public Layer ActiveLayer { get; set; }
    [field: SerializeField] public int LayerNumber { get; set; }
    [field: SerializeField] public int EnemiesKilled { get; set; }

    //[field: SerializeField] public RoomScript CurrentRoom { get; set; }
    [field: SerializeField] public int RoomsCleared { get; set; }
    [field: SerializeField] public int Seed {  get; set; }
    [field: SerializeField] public bool IsSeeded {  get; set; }
    //PlayerStats do not need to be saved, only the stuff that modifies it!
    //except for EnemiesKilled
    //IMPORTANT FOR INVENTORYITEMS AND ACTIVECARDS: HAVE TO BE EQUIPPED / PLAYED AGAIN FOR EVERYTHING TO WORK

    public SaveState()
    {
        EquippedItems = new Item[(int)Enums.SlotTag.None];
        InventoryItems = new List<Item>();
        CardsInHand = new List<Card>();
        EntireDeck = new List<Card>(); //maybe needs to be changed in the future to a dummy deck
        DrawPile = new List<Card>();
        DiscardPile = new List<Card>();
        ActiveLayer = null; //maybe needs to be changed to a dummy layer
        LayerNumber = 0;
        EnemiesKilled = 0;
        RoomsCleared = 0;
        Seed = 0;
        IsSeeded = false;
    }
}
