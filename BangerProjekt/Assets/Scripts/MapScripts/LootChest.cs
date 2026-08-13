using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootChest : MonoBehaviour
{
	[field: SerializeField] public static int MinCredits { get; set; }
	[field: SerializeField] public static int MaxCredits { get; set; }



	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int creditsToGet = Random.Range(MinCredits, MaxCredits) + (int)((Random.Range(30, 101) / 100f) * GameManager.roomsCleared); 
            //we take a number between min and MaxCredits and add to that between 30% and 100% of the rooms cleared
            StartCoroutine(OpenChest(creditsToGet)); //for now 50 credits idk this would scale
        }
    }


    public IEnumerator OpenChest(int creditAmount)
    {
        //play cool opening animation and shit and probably particles and yippie
        int creditsPaid = 0;
        while (creditsPaid < creditAmount)
        {
            creditsPaid++;
            GameManager.Instance.ChangeCredits(1);
            //Debug.Log("Credits: " + GameManager.credits);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
}
