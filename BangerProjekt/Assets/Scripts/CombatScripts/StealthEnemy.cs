using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthEnemy : Enemy
{
    [SerializeField] private float distanceToStartVisibility;


    new void FixedUpdate()
    {
        base.FixedUpdate();
        SetVisibility();

    }

    public void SetVisibility()
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
       renderer.color = new Color(renderer.color.r,renderer.color.g,renderer.color.b,(distanceToStartVisibility - Distance)/distanceToStartVisibility);
    }

    //identical to "normal" enemies but we need to change the .a value (alpha value) of the spriteRenderer to "make them invisible"
}
