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
    private bool isDashing;

    private PlayerControls pc; //playercontrols detects the inputs

    private void Awake()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>(); //set the Reference to the Camera in "Awake" (before the first frame)
        //this is also not possible in inspector because we set the mainCamera to private (to avoid bloating the inspector and we dont need to reference this instance of the MainCamera anywhere
        //playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
        playerScript = gameObject.GetComponent<Player>();
        isDashing = false;
        pc = new PlayerControls();
    }

    void OnEnable()
    {
        pc.Enable();
        LayerManager.newLayer += SetLayerPosition;
    }

    void OnDisable()
    {
        pc.Disable();
        LayerManager.newLayer -= SetLayerPosition;
    }

    public void SetLayerPosition()
    {
        gameObject.transform.position = new Vector3(0,0,0); //reset position
    }


    void Update()
    {
        Turn();
        ProcessInputs();
        Move();
        //has to be set every frame so the player does not look clunky
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
    }

    public void Turn()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); //we get the Mouse Position here for later (its good to know where the mouse currently is, especially for shooting etc.)
        Vector2 aimDirection = mousePosition - rb.position; //by subtracting the current position of the playerObject (the rb is attached to it) from the mousePosition we got earlier, we can get a new Direction Vector
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f; //we use this lovely function to convert our vector2 to an angle
        rb.rotation = aimAngle; //and set the rotation of the character to this  new rotation (because the character technically always shoots "up", we just rotate this "up" position)
    }

    public void Dash(float speedMult, float dashDuration)
    {
        playerScript.MoveSpeed *= speedMult;  //the player gets really fast
        //Debug.Log("new moveSpeed: " + playerScript.MoveSpeed);
        isDashing = true; 
        StartCoroutine(EndDash(dashDuration,speedMult));
    }
    

    public IEnumerator EndDash(float duration, float speedMult)
    {

        yield return new WaitForSeconds(duration);
        playerScript.MoveSpeed /= speedMult; //works for now but can be buggy if you have multiple effects affecting your speed (i think)
        if (playerScript.MoveSpeed < playerScript.InitialMoveSpeed)
        {
            playerScript.MoveSpeed = playerScript.InitialMoveSpeed; //this is the minimum tho
        }
        isDashing = false;
    }

}

