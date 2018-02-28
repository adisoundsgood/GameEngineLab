using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour {

    [SerializeField]
    private float movementSpeed; // How much the player moves in a given direction

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private Camera mainCamera;

    private GameObject playerShip;

    private Rigidbody rigidBody;
    private Vector3 input, movement;

    private Bounds cameraBounds;
    private float halfWidth,halfHeight, screenhalfX, screenhalfY;


    //private Quaternion rotation;

    // Use this for initialization
    void Awake () {
        playerShip = (GameObject) Instantiate(playerPrefab, transform.position, playerPrefab.transform.rotation);
        playerShip.transform.parent = this.transform;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.position = transform.position;

        movement = Vector3.zero;

        halfWidth = playerShip.GetComponent<MeshRenderer>().bounds.extents.x;
        halfHeight = playerShip.GetComponent<MeshRenderer>().bounds.extents.y;
        Debug.Log("halfWidth: " + halfWidth + " halfHeight: " + halfHeight);

        cameraBounds = CameraExtensions.OrthographicBounds(mainCamera);
        screenhalfX = cameraBounds.extents.x;
        screenhalfY = cameraBounds.extents.y;
    }

    // FixedUpdate is called once per fixed framerate frame
    void FixedUpdate () {
        input = Vector3.zero;
        //rotation = Quaternion.identity;

        // Structured so players cannot move simultaneously in opposing directions
        if (Input.GetAxisRaw("Horizontal2") < 0f && (rigidBody.position.x - halfWidth > -1*screenhalfX))  // moving left 
            input += Vector3.left;
        else if (Input.GetAxisRaw("Horizontal2") > 0f && (rigidBody.position.x + halfWidth < screenhalfX))  // moving right
            input += Vector3.right;
        if (Input.GetAxisRaw("Vertical2") > 0f && (rigidBody.position.y + halfHeight < screenhalfY)) // moving up 
            input += Vector3.up;
        else if (Input.GetAxisRaw("Vertical2") < 0f && (rigidBody.position.y - halfHeight > -1*screenhalfY))  // moving down
            input += Vector3.down;


        input.Normalize();

        movement = input * movementSpeed * Time.fixedDeltaTime;
        rigidBody.MovePosition(rigidBody.position + movement);
    }
}