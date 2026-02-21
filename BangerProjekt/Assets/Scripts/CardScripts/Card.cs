using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    [field:SerializeField] public int ID{get;set;}
    [field: SerializeField] public Enums.Rarity CardRarity{get;set;}
    [field: SerializeField] public string Name{get;set;}
    [field: SerializeField] public string Description{get;set;}
    [field: SerializeField] public Enums.Class ClassOfCard{get;set;}
    [field: SerializeField] public Image CardImage {get;set;}
}

