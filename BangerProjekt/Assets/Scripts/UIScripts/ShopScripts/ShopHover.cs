using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text;
using Image = UnityEngine.UI.Image;
using System;

public class ShopHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [field:SerializeField] public Transform DetailView{get; set;}
    [field:SerializeField] public Item Item{get; set;} = null;
    [field:SerializeField] public Card Card{get; set;} = null;
    [field:SerializeField] public GameObject ItemViewPrefab{get; set;}

    public static event Action<Item> purchaseItem;
    public static event Action<Card> purchaseCard;


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item != null)
        { //Set all the good shit of an Item
            GetComponent<Image>().color = Color.gray;
            GameObject itemView = Instantiate(ItemViewPrefab, DetailView, false);
            TMP_Text nameText = itemView.transform.Find("NameText").GetComponent<TMP_Text>();
            TMP_Text descriptionText = itemView.transform.Find("DescriptionText").GetComponent<TMP_Text>();
            TMP_Text statText = itemView.transform.Find("StatText").GetComponent<TMP_Text>();
            nameText.SetText(Item.name);
            nameText.font = GameManager.Instance.GameFont;
            descriptionText.SetText(Item.Description);
            descriptionText.font = nameText.font;
            statText.SetText(Item.BuildStatString());
            statText.font = nameText.font;
            itemView.transform.Find("ItemImage").GetComponent<Image>().sprite = Item.Icon;
        }
        else
        {
            GameObject newCard = Instantiate(gameObject,DetailView,false); //Just copy the small card 
            newCard.transform.localScale = new Vector2(3,3); //and make it big
            Destroy(newCard.GetComponent<ShopHover>()); //We dont need a shop hover on the big card
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {  
        DestroyChild();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
         if (Item != null)
         {
             if(GameManager.credits < Item.Cost)
             {
                Debug.Log("Not enough credits to purchase " + Item.name); //TODO: this is to be replaced with a proper UI element
                return;
             }
             purchaseItem?.Invoke(Item);
             DestroyChild();//Kill it
             GameManager.Instance.ChangeCredits(-Item.Cost); //remove the credits for the purchase
             gameObject.SetActive(false); //hide the item from the shop after purchase, because the spots should not be destroyed
         }
         else if (Card != null)
         {
             if(GameManager.credits < Card.Cost)
             {
                Debug.Log("Not enough credits to purchase " + Card.name); //TODO: this as well
                return;
             }
             purchaseCard?.Invoke(Card); //Send out the Event to actually give the player the card
             DestroyChild(); //THE CHILD MUST DIE
             GameManager.Instance.ChangeCredits(-Card.Cost); //remove the credits for the purchase
             Destroy(gameObject); //remove the card from the shop after purchase
         }
         else
         {
             Debug.LogError("Shop element has no Item or Card assigned to it, but was clicked");
         }

        }
    }
    private void DestroyChild() //Yeetus feetus deletus
    {
        if(Item)GetComponent<Image>().color = Color.white;
        if(DetailView.childCount > 0){
            foreach(Transform child in DetailView.transform)
            { 
            Destroy(child.gameObject);
            }
        }
    }

}
