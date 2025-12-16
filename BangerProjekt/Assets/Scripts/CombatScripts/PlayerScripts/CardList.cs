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
        new Card {ID = 0, name = "test0", description = "This card sucks ass" ,cardRarity = Enums.Rarity.Common, playerClass = Enums.Class.Universal },
        new Card {ID = 1, name = "test1", description = "This card is insane" ,cardRarity = Enums.Rarity.Rare, playerClass = Enums.Class.Fighter },
    };
}

public class Card
{
    public int ID;

    public Enums.Rarity cardRarity;
    public string name;
    public string description;



    public Enums.Class playerClass;


}
