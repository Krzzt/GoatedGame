using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Player))]
public class movement : MonoBehaviour
{
    private Vector2 mousePosition; //Vector in which we will save the mousePosition

    private Camera mainCamera; //Main Camera Reference for getting the MousePos
    public Rigidbody2D rb; //RigidBody Reference (dont forget 2D!)
    private Vector2 moveDirection; //The Vector in which we save the movement direction as a Vector2

    private Player playerScript;

    private bool canDash;
    private bool isDashing;

    private PlayerControls pc; //playercontrols detects the inputs

    private void Awake()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>(); //set the Reference to the Camera in "Awake" (before the first frame)
        //this is also not possible in inspector because we set the mainCamera to private (to avoid bloating the inspector and we dont need to reference this instance of the MainCamera anywhere
        playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
    }


    void Update()
    {
        Turn();
        ProcessInputs();
        Move();
    }




    public void ProcessInputs()
    {
        if (!isDashing)
        {
            moveDirection = pc.Player.Move.ReadValue<Vector2>(); //new input system
        }

    }
    public void Move()
    {

        rb.velocity = new Vector2(moveDirection.x * playerScript.MoveSpeed, moveDirection.y * playerScript.MoveSpeed); //rb is the Rigidbody, and its velocity is set in Vectors (Vector2 because we only need x and y because its a 2D game)
        Vector2 aimDirection = mousePosition - rb.position; //by subtracting the current position of the playerObject (the rb is attached to it) from the mousePosition we got earlier, we can get a new Direction Vector
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f; //we use this lovely function to convert our vector2 to an angle
        rb.rotation = aimAngle; //and set the rotation of the character to this  new rotation (because the character technically always shoots "up", we just rotate this "up" position)
    }

    public void Dash(float speedMult, float dashDuration, float dashCooldown)
    {   
        if (canDash)
        {
            playerScript.MoveSpeed *= speedMult;  //the player gets really fast
            //Debug.Log("new moveSpeed: " + playerScript.MoveSpeed);
            isDashing = true; 
            canDash = false;
            StartCoroutine(EndDash(dashDuration,speedMult));
            StartCoroutine(DashCooldown(dashCooldown));
            //start 2 coroutines to end the dash and start the cooldown


        }


    }
    
    public IEnumerator DashCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canDash = true;
        //dash is up again
    }

    public IEnumerator EndDash(float duration, float speedMult)
    {

        yield return new WaitForSeconds(duration);
        playerScript.MoveSpeed /= speedMult; //works for now but can be buggy if you have multiple effects affecting your speed (i think)
        isDashing = false;
    }

}