using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBarFill : MonoBehaviour
{
	private Image image;
	private TMP_Text text;
	// Start is called before the first frame update
	void Awake()
	{
		image = GetComponent<Image>();
		text = GetComponentInChildren<TMP_Text>();
	}

	// Update is called once per frame
	void Update()
	{
		image.fillAmount = (float)Player.Instance.CurrentExp / Player.Instance.RequiredExp;
		text.SetText("{0}/{1}", Player.Instance.CurrentExp,Player.Instance.RequiredExp);
	}
}
