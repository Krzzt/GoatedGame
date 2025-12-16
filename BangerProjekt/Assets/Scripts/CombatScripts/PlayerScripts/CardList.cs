using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public static class CardList //We inherit from nothing, since this class is meant to be a "static" Class, which means we can access this from anywhere
//so if i want to reference anything in this class from another Script i just have to write (e.g) "CardList.test".
//static means that we dont have multiple instances of this class, it is just 1 "global" class (like the main functions in java
{
    public static List<Card> allCards = new List<Card>
    {
        new Card {ID = 0, Name = "test0", Description = "This card sucks ass" ,CardRarity = Enums.Rarity.Common, ClassOfCard = Enums.Class.Universal },
        new Card {ID = 1, Name = "test1", Description = "This card is insane" ,CardRarity = Enums.Rarity.Rare, ClassOfCard = Enums.Class.Fighter },
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
    private Enums.Rarity cardRarity;
    public Enums.Rarity CardRarity
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
    private Enums.Class classOfCard;
    public Enums.Class ClassOfCard
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
