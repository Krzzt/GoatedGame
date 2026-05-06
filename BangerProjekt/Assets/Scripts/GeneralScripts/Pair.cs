using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Pair<K,V>
{
    public Pair()
    {
        
    }
    public Pair(K first, V second)
    {
        First = first;
        Second = second;
    }

    [field:SerializeField] public K First {get; set;}
    [field: SerializeField]public V Second {get; set;}
}
