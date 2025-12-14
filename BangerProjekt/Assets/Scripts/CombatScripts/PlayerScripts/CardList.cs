using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public static class CardList //We inherit from nothing, since this class is meant to be a "static" Class, which means we can access this from anywhere
//so if i want to reference anything in this class from another Script i just have to write (e.g) "CardList.test".
//static means that we dont have multiple instances of this class, it is just 1 "global" class (like the main functions in java)
{
    public static List<Card> allCards = new List<Card>
    {
        new Card {ID = 0, Name = "test0", Description = "This card sucks ass" ,CardRarity = Card.Rarity.Common, ClassOfCard = Card.CardClass.Universal },
        new Card {ID = 1, Name = "test1", Description = "This card is insane" ,CardRarity = Card.Rarity.Rare, ClassOfCard = Card.CardClass.Fighter },
    };
}

public class Card
{
    private int id;

    public int ID
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }
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

    private Rarity cardRarity;
    public Rarity CardRarity
    {
        get
        {
            return cardRarity;
        }

        set
        {
            cardRarity = value;
        }
    }
    private string name;
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }
    private string description;
    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public enum CardClass //CAPITAL C means not class
    {
        Universal, //0 = Universal
        Fighter, //1 = Fighter
        Mage //2 = Mage
        //This is currently just for testing purposes, these Classes are not depicting actual classes we want later
    }

    private CardClass classOfCard;
    public CardClass ClassOfCard
    {
        get
        {
            return classOfCard;
        }

        set
        {
            classOfCard = value;
        }
    }


}
