using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour {

	[SerializeField]
	private float movementSpeed;

	// Player and ship prefabs
	[SerializeField]
	private GameObject shipPrefab;
	private GameObject playerPrefab;

	// Camera and bounds
	[SerializeField]
	private Camera mainCamera;
	private Bounds cameraBounds;
	private float halfWidth, halfHeight, screenhalfX, screenhalfY;

	// Setup
	private Rigidbody rigidBody;
	private Vector3 input, movement;
	private Vector3 startPos;
	private Quaternion startRotation;
	private MeshRenderer shipMesh;

	// Health
	public GameObject healthManager;
	private PlayerHealthManager hm;
	private bool isInvincible;

	[SerializeField]
	private float invincibleTimer;
	private float startTime = 0f;

	// Shooting
	public GameObject shot;
	public Transform bulletSpawner;
	public float fireRate;
	private float nextFire;

	void Awake() {
		startPos = transform.position;
		startRotation = shipPrefab.transform.rotation;

		playerPrefab = (GameObject) Instantiate(shipPrefab, startPos, startRotation);
		playerPrefab.transform.parent = this.transform;
		rigidBody = GetComponent<Rigidbody>();
		rigidBody.position = startPos;

		shipMesh = playerPrefab.GetComponent<MeshRenderer>();

		movement = Vector3.zero;

		halfWidth = playerPrefab.GetComponent<MeshRenderer>().bounds.extents.x;
		halfHeight = playerPrefab.GetComponent<MeshRenderer>().bounds.extents.y;

		cameraBounds = CameraExtensions.OrthographicBounds(mainCamera);
		screenhalfX = cameraBounds.extents.x;
		screenhalfY = cameraBounds.extents.y;

		hm = healthManager.GetComponent<PlayerHealthManager>();
		isInvincible = false;
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
		if (Input.GetAxisRaw("Horizontal2") < 0f && (rigidBody.position.x - halfWidth > -1 * screenhalfX))  // moving left 
			input += Vector3.left;
		else if (Input.GetAxisRaw("Horizontal2") > 0f && (rigidBody.position.x + halfWidth < screenhalfX))  // moving right
			input += Vector3.right;
		if (Input.GetAxisRaw("Vertical2") > 0f && (rigidBody.position.y + halfHeight < screenhalfY)) // moving up 
			input += Vector3.up;
		else if (Input.GetAxisRaw("Vertical2") < 0f && (rigidBody.position.y - halfHeight > -1 * screenhalfY))  // moving down
			input += Vector3.down;

		input.Normalize();

		movement = input * movementSpeed * Time.fixedDeltaTime;
		rigidBody.MovePosition(rigidBody.position + movement);
	}

	private void OnTriggerEnter(Collider col) {
		if (!isInvincible) {
			if (col.CompareTag("enemyBullet") || col.CompareTag("enemy")) {
				if (hm.getLives() <= 0) {
					Destroy(this);
				}

				hm.gotHit();

				// Respawn with invincibility
				transform.position  = startPos;
				rigidBody.position = startPos;
				playerPrefab.transform.rotation = startRotation;
				StartCoroutine("Flicker");
			}
		}
	}

	IEnumerator Flicker() {
		isInvincible = true;
		while (startTime < invincibleTimer) {
			startTime += Time.deltaTime + 0.2f;

			shipMesh.enabled = false;
			yield return new WaitForSeconds(0.1f);
			shipMesh.enabled = true;
			yield return new WaitForSeconds(0.1f);
		}
		startTime = 0;
		isInvincible = false;
		yield return null;
	}
}
