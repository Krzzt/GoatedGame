using UnityEngine;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    [field:SerializeField] public int ID{get;set;}
    [field: SerializeField] public Enums.Rarity CardRarity{get;set;}
    [field: SerializeField] public string Name{get;set;}
    [field: SerializeField] public string Description{get;set;}
    [field: SerializeField] public Enums.Class ClassOfCard{get;set;}
    [field: SerializeField] public Sprite CardImage {get;set;}
    [field: SerializeField] public int Cost {get;set;} //Pricy little things
    [field: SerializeField] public Layer LayerOfCard {get; set;}
    [field: SerializeField] public int CurrencyCost {get; set;}
    
}

