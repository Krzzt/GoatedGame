using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Script is supposed to be on the Player GameObject and will only work if this is the case
public class UseAbilities : MonoBehaviour
{
    private Player playerScript;
    private movement movementScript;
    private Weapon weaponScript;

    [field: SerializeField] public float Cooldown { get; set; } //doesnt need a serializeField but its there to see if everything works (debugging basically)
    private bool isReady = true;
    public static Action<float> SetAbilityUI;


    void Awake()
    {
        playerScript = gameObject.GetComponent<Player>();
        movementScript = gameObject. GetComponent<movement>();
        weaponScript = gameObject.GetComponent<Weapon>();
    }

    public void UseAbility()
    {
        if (!isReady) return;
        isReady = false;
        switch(InventoryLogic.ItemsEquipped[(int)Enums.SlotTag.Ability].ID)
        {
            case 1: //Dash
                Debug.Log("Dash!");
                float dashSpeedIncrease = 4f; //hard coding is necessary (can be changed here for balancing)
                float dashDuration = 0.2f;
                movementScript.Dash(dashSpeedIncrease,dashDuration);
                break;
            case 2: //big Damage
                //nothing because the gimmick is to not have an ability
                //Damage gets set in the funny script
                //also this is not an AbilityItem but a normal Item that just so happens to be in the "Ability" Slot
                break;
        }
        StartCoroutine(AbilityCooldown(Cooldown));
    }

    public IEnumerator AbilityCooldown(float time) //call by value yessss
    {
        isReady = false;
        while (time > 0) //no yield return because we need to set the UI shit
        {
            time -= Time.fixedDeltaTime;
            SetAbilityUI?.Invoke(time / Cooldown);
            yield return new WaitForFixedUpdate();
        }
        yield return null;
        isReady = true;
    }
}
