using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefEnemy : Enemy
{
	private bool hasStolen = false;
	[SerializeField] private int amountOfCreditsToSteal;

	[SerializeField] private float speedMultiplierAfterSteal;

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && !hasStolen) //actually the hitting stuff is declared in the player, but we need this to check if we steal
		{
			//steal some credits from the player hehehehee
			GameManager.Instance.ChangeCredits(-amountOfCreditsToSteal);
			hasStolen = true;
			MoveSpeed *= -speedMultiplierAfterSteal;
		}
		else if ((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Door")) && hasStolen)
		{
			enemyDies?.Invoke(gameObject);
			Destroy(gameObject); //basically die without giving a kill or exp
		}
	}

}
