using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Script is supposed to be on the Player GameObject and will only work if this is the case
public class UseAbilities : MonoBehaviour
{
    private Player playerScript;
    private movement movementScript;
    private Weapon weaponScript;

    [field:SerializeField] public float DashSpeedIncrease {get; set;}
    [field:SerializeField] public float DashDuration {get; set;}
    [field:SerializeField] public float DashCooldown {get; set;}

    void Awake()
    {
        playerScript = gameObject.GetComponent<Player>();
        movementScript = gameObject. GetComponent<movement>();
        weaponScript = gameObject.GetComponent<Weapon>();
    }

    public void UseAbility()
    {
        switch(InventoryLogic.ItemsEquipped[(int)Enums.SlotTag.Ability].ID)
        {
            case 1: //Dash
                movementScript.Dash(DashSpeedIncrease,DashDuration,DashCooldown);
                break;
            case 2: //big Damage
                //nothing because the gimmick is to not have an ability
                break;
        }
    }
}
