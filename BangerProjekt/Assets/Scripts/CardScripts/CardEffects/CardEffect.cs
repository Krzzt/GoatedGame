using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


[CreateAssetMenu]
public abstract class CardEffect : ScriptableObject
{
	public abstract void ExecuteEffect(string effect);
	public abstract void RevertEffect(string effect);

	public virtual void OnRoomClear()
	{

	}

	protected CultureInfo info = new CultureInfo("en-EN");
}
