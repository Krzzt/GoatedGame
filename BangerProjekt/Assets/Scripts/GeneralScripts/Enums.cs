using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
    //We do not need "static" for any of the Enums, since theyre static by default
    //they cant not be static, hence the static keyword will throw an error
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

        public enum Class //CAPITAL C means not class
    {
        Universal, //0 = Universal
        Fighter, //1 = Fighter
        Mage //2 = Mage
        //This is currently just for testing purposes, these Classes are not depicting actual classes we want later
    }
    public enum SlotTag
    {
       Head,
       Chest,
       Legs,
       Feet,
       None 
    }
}
