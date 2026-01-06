using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


//this is not only used for dmg popups but for any popups
public class PopUp : MonoBehaviour
{
    public float TimeToLive{get;set;} //time left until the text starts disappearing
    private Color textColor; //the color of the text
    private TextMeshPro currTextMesh; //the textmesh of the popup
    private Vector3 moveVector; //the vector which is used to move the text up

    public bool IsRealPopUp{get;set;} //this is here to prevent the lodaded in reference despawning
    public static void Create(Vector3 position, string ThingToSay, Color PopUpColor) //used to create a popup. You need to add: The position of the popup, the text that should be displayed (damage in this case),if it is a crit or not, and the color of the popup
    {
        GameObject tempPopUp = GameObject.Find("PopUpPrefab"); //find the loaded in reference

        if (tempPopUp == null) //if it is null
        {
            Debug.LogWarning("PopUp: No PopUpPrefab found in the scene. Please make sure there is a GameObject named 'PopUpPrefab' in the scene.");
            return;
        }

        GameObject newPopUp = Instantiate(tempPopUp, position, Quaternion.identity); //instantiate a new one
        newPopUp.GetComponent<PopUp>().Setup(ThingToSay, PopUpColor); //do the rest of the setup
    }


    public void Setup(string ThingToSay, Color PopUpColor)
    {

        currTextMesh = transform.GetComponent<TextMeshPro>(); //get the textmesh
        textColor = PopUpColor;
        currTextMesh.color = textColor; //set the color 
        currTextMesh.SetText(ThingToSay); //set the text to the damage number
        currTextMesh.fontSize = 5; //smaller text, if necessary this could be a parameter in the create later
        IsRealPopUp = true; //it is not the reference so set it true
        moveVector = new Vector3(0, 1) / 20; //small movevector
    }
    void FixedUpdate()
    {
        if (IsRealPopUp) //if its not the reference
        {
            gameObject.transform.position += moveVector; //move it
            TimeToLive -= Time.fixedDeltaTime; //reduce its time to live

            if (TimeToLive <= 0) //if its 0
            {
                float disappearSpeed = 3f; //set a speed to disappear
                textColor.a -= disappearSpeed * Time.deltaTime; //let it disappear
                currTextMesh.color = textColor; //set the color new to actually see the alpha

                if (textColor.a <= 0) //if its completely gone
                {
                    Destroy(gameObject); //destroy it
                }
            }
        }

    }
}
