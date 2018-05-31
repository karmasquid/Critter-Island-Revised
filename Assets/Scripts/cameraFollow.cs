using UnityEngine;

public class cameraFollow : MonoBehaviour {

    private Transform playerChar; //The players transform
    private Vector3 velocity = Vector3.zero; //Gives the camera a starting velocity value for good measures.

    public Vector3 offset; //Gets the offset.
    public float smoothspeed = 0.125f; // The smoothening delay for the camera to follow the players movement.


    private void Awake()
    {
        //Sets the player transform by finding the player in the scene.
        playerChar = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        Vector3 desiredpos = playerChar.position + offset; //Sets a vector with the added offset.
        Vector3 smoothpos = Vector3.SmoothDamp(transform.position, desiredpos,  ref velocity, smoothspeed); //Makes a vector calculation based on all previously gatherered variables.
        transform.position = smoothpos; //Transforms the camera to the calculated area.
    }

} //Mattias Eriksson
