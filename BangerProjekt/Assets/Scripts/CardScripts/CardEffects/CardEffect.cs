using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public abstract class CardEffect : ScriptableObject
{
    public abstract void ExecuteEffect(string effect);
    public abstract void RevertEffect(string effect);
}
