using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class movement : MonoBehaviour
{
    private Vector2 mousePosition; //Vector in which we will save the mousePosition

    private Camera mainCamera; //Main Camera Reference for getting the MousePos
    public Rigidbody2D rb; //RigidBody Reference (dont forget 2D!)
    private Vector2 moveDirection; //The Vector in which we save the movement direction as a Vector2

    private Player playerScript;

    private void Awake()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>(); //set the Reference to the Camera in "Awake" (before the first frame)
        //this is also not possible in inspector because we set the mainCamera to private (to avoid bloating the inspector and we dont need to reference this instance of the MainCamera anywhere
        playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
        //playerScript = this.GetComponent<Player>();
    }


    void Update() //we use Update and not FixedUpdate because input is frame dependent. If a player has over 50fps (the standard set by fixedUpdate), his movement wont register correctly
    {
            
        ProcessInputs();
        move();
        //just references to other functions that should happen every frame
    }





    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized; //generate a vector based on the x and y coordinates of the Input (WASD). This Vector is normalized
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); //we get the Mouse Position here for later (its good to know where the mouse currently is, especially for shooting etc.)


    }
    void move()
    {

        rb.velocity = new Vector2(moveDirection.x * playerScript.MoveSpeed, moveDirection.y * playerScript.MoveSpeed); //rb is the Rigidbody, and its velocity is set in Vectors (Vector2 because we only need x and y because its a 2D game)
        Vector2 aimDirection = mousePosition - rb.position; //by subtracting the current position of the playerObject (the rb is attached to it) from the mousePosition we got earlier, we can get a new Direction Vector
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f; //we use this lovely function to convert our vector2 to an angle
        rb.rotation = aimAngle; //and set the rotation of the character to this  new rotation (because the character technically always shoots "up", we just rotate this "up" position)


    }


}

