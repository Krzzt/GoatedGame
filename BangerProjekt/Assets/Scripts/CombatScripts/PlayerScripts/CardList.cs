using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public static class CardList //We inherit from nothing, since this class is meant to be a "static" Class, which means we can access this from anywhere
//so if i want to reference anything in this class from another Script i just have to write (e.g) "CardList.test".
//static means that we dont have multiple instances of this class, it is just 1 "global" class (like the main functions in java
{
    public static List<Card> allCards = new List<Card>
    {
        new Card {ID = 0, name = "test0", description = "This card sucks ass" ,cardRarity = Card.Rarity.Common, playerClass = Card.Class.Universal },
        new Card {ID = 1, name = "test1", description = "This card is insane" ,cardRarity = Card.Rarity.Rare, playerClass = Card.Class.Fighter },
    };
}

public class Card
{
    public int ID;
    public enum Rarity
    {
        Common, //0 = common
        Uncommon, //1 = Uncommon
        Rare, //2 = Rare
        Epic, //3 = Epic
        Legendary, //4 = Legendary
        Mythic // 5 = Mythic
        //for more Informations about enums, just google c# enums but they are set at 0,1,2... by default 
    }

    public Rarity cardRarity;
    public string name;
    public string description;

    public enum Class //CAPITAL C means not class
    {
        Universal, //0 = Universal
        Fighter, //1 = Fighter
        Mage //2 = Mage
        //This is currently just for testing purposes, these Classes are not depicting actual classes we want later
    }

    public Class playerClass;


}
