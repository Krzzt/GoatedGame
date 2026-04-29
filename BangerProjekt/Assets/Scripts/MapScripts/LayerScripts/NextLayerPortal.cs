using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NextLayerPortal : MonoBehaviour
{
    [SerializeField] private GameObject actionText;


	void Awake()
	{
		actionText.SetActive(false);
	}

	void OnEnable()
	{
		Player.InteractEvent += Interacted;
	}

	void OnDisable()
	{
		Player.InteractEvent -= Interacted;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player")) actionText.SetActive(true);
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player")) actionText.SetActive(false);
	}

    public void Interacted()
    {
        if (actionText.activeSelf) LayerManager.Instance.NextLayer();
    }
}
