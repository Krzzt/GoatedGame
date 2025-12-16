using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f); //set an "offset" (so the camera isnt perfectly on the player all the time)
    private float smoothTime = 0.25f; //the time the camera takes to smoothly move back to the player after they move
    private Vector3 velocity = Vector3.zero; //the Velocity. It is initialized as a null vector (every value as 0), but it gets changed all the time by the "SmoothDamp" function later

    [SerializeField] private Transform target; //the target the camera follows (the Transform of the Player)

    private void FixedUpdate() //FixedUpdate Yippie
    {
        Vector3 targetPosition = target.position + offset; //our Target is not the Player directly but a place defined by the players position and the offset we set
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime); //the Transform.Position (because this is added to the Camera, the Cameras location is meant)
        //is set with the "SmoothDamp" function
    }
}
