using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour {

	[SerializeField]
	private float movementSpeed;

	[SerializeField]
	private GameObject playerPrefab;
	private GameObject playerShip;

	[SerializeField]
	private Camera mainCamera;
	private Bounds cameraBounds;
	private float halfWidth, halfHeight, screenhalfX, screenhalfY;

	private Rigidbody rigidBody;
	private Vector3 input, movement;

	// Shooting
	public GameObject shot;
	public Transform bulletSpawner;
	public float fireRate;
	private float nextFire;

	void Awake() {
		playerShip = (GameObject) Instantiate(playerPrefab, transform.position, playerPrefab.transform.rotation);
		playerShip.transform.parent = this.transform;
		rigidBody = GetComponent<Rigidbody>();
		rigidBody.position = transform.position;

		movement = Vector3.zero;

		halfWidth = playerShip.GetComponent<MeshRenderer>().bounds.extents.x;
		halfHeight = playerShip.GetComponent<MeshRenderer>().bounds.extents.y;

		cameraBounds = CameraExtensions.OrthographicBounds(mainCamera);
		screenhalfX = cameraBounds.extents.x;
		screenhalfY = cameraBounds.extents.y;
	}

	void Update() {
		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Instantiate (shot, bulletSpawner.position, bulletSpawner.rotation);
		}
	}

	void FixedUpdate() {
		input = Vector3.zero;

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
