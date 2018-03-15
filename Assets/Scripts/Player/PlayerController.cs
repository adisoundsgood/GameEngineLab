using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float movementSpeed;

	// Player and ship prefabs
	[SerializeField]
	private GameObject shipPrefab;
	private GameObject playerPrefab;
	[SerializeField]
	private GameObject shieldPrefab;

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
	
	// States
	private bool isInvincible;
	private bool isRespawning;
	private bool usingUlt;

	[SerializeField]
	private float invincibleTimer;
	private float startTime = 0f;
	
	private bool shieldReady = true;
	[SerializeField]
	private float shieldTimer;
	private float startShield = 0f;
	private float shieldCD = 10f;
	private float lastShield;

	// Shooting
	public GameObject shot;
	public Transform bulletSpawner;
	public float fireRate;
	private float nextFire;

    //Audio
    private AudioSource audioSource;
    private AudioClip playerHurt;

	void Awake() {
		startPos = transform.position;
		startRotation = shipPrefab.transform.rotation;

		playerPrefab = (GameObject) Instantiate(shipPrefab, startPos, startRotation);
		playerPrefab.transform.parent = this.transform;
		rigidBody = GetComponent<Rigidbody>();
		rigidBody.position = startPos;

		shipMesh = playerPrefab.GetComponent<MeshRenderer>();
		
		shieldPrefab.SetActive(false);

		movement = Vector3.zero;

		halfWidth = playerPrefab.GetComponent<MeshRenderer>().bounds.extents.x;
		halfHeight = playerPrefab.GetComponent<MeshRenderer>().bounds.extents.y;

		cameraBounds = CameraExtensions.OrthographicBounds(mainCamera);
		screenhalfX = cameraBounds.extents.x;
		screenhalfY = cameraBounds.extents.y;

		hm = healthManager.GetComponent<PlayerHealthManager>();
		isInvincible = false;

        audioSource = GetComponent<AudioSource>();
        playerHurt = (AudioClip) Resources.Load("Audio/SFX/Player Hurt");
	}

	void Update() {
		if (Time.time > nextFire && !isRespawning && !usingUlt) {
			nextFire = Time.time + fireRate;
			Instantiate (shot, bulletSpawner.position, bulletSpawner.rotation);
		}
		
		if (!shieldReady && lastShield < shieldCD) {
			lastShield += Time.deltaTime;
		}
		else {
			shieldReady = true;
		}
	}
	
	void FixedUpdate() {
		if (this.gameObject.tag == "p1") {
			input = Vector3.zero;

			// Movement
			if (Input.GetAxisRaw ("Horizontal1") < 0f && (rigidBody.position.x - halfWidth > -1 * screenhalfX))  // moving left 
				input += Vector3.left;
			else if (Input.GetAxisRaw ("Horizontal1") > 0f && (rigidBody.position.x + halfWidth < screenhalfX))  // moving right
				input += Vector3.right;
			if (Input.GetAxisRaw ("Vertical1") > 0f && (rigidBody.position.y + halfHeight < screenhalfY)) // moving up 
				input += Vector3.up;
			else if (Input.GetAxisRaw ("Vertical1") < 0f && (rigidBody.position.y - halfHeight > -1 * screenhalfY))  // moving down
				input += Vector3.down;

			input.Normalize ();

			movement = input * movementSpeed * Time.fixedDeltaTime;
			rigidBody.MovePosition (rigidBody.position + movement);
			
			// Skills
			if (Input.GetButtonDown("Ult1")) {
				// do this
			}
			
			if (Input.GetButtonDown("Shield1") && shieldReady) {
				lastShield = 0f;
				StartCoroutine("UseShield");
			}	
		}

		if (this.gameObject.tag == "p2") {
			input = Vector3.zero;

			// Movement
			if (Input.GetAxisRaw ("Horizontal2") < 0f && (rigidBody.position.x - halfWidth > -1 * screenhalfX))  // moving left 
				input += Vector3.left;
			else if (Input.GetAxisRaw ("Horizontal2") > 0f && (rigidBody.position.x + halfWidth < screenhalfX))  // moving right
				input += Vector3.right;
			if (Input.GetAxisRaw ("Vertical2") > 0f && (rigidBody.position.y + halfHeight < screenhalfY)) // moving up 
				input += Vector3.up;
			else if (Input.GetAxisRaw ("Vertical2") < 0f && (rigidBody.position.y - halfHeight > -1 * screenhalfY))  // moving down
				input += Vector3.down;

			input.Normalize ();

			movement = input * movementSpeed * Time.fixedDeltaTime;
			rigidBody.MovePosition (rigidBody.position + movement);
						
			// Skills
			if (Input.GetButtonDown("Ult2")) {
				// do this
			}
			
			if (Input.GetButtonDown("Shield2") && shieldReady) {
				lastShield = 0f;
				StartCoroutine("UseShield");
			}
		}
	}

	private void OnTriggerEnter(Collider col) {
		if (!isInvincible) {
			if (col.CompareTag("enemyBullet") || col.CompareTag("enemy")) {
				if (hm.getLives() <= 0) {
					Destroy(this);
				}

				hm.gotHit();
                audioSource.PlayOneShot(playerHurt);


				// Respawn with invincibility
				transform.position  = startPos;
				rigidBody.position = startPos;
				playerPrefab.transform.rotation = startRotation;
				StartCoroutine("Flicker");
			}
		}
	}
	
	void SetInvincible(bool val) {
		isInvincible = val;
	}
	
	IEnumerator Flicker() {
		SetInvincible(true);
		isRespawning = true;
		
		while (startTime < invincibleTimer) {
			startTime += Time.deltaTime + 0.1f;

			shipMesh.enabled = false;
			yield return new WaitForSeconds(0.05f);
			shipMesh.enabled = true;
			yield return new WaitForSeconds(0.05f);
		}
		
		startTime = 0;
		
		SetInvincible(false);
		isRespawning = false;
		
		yield return null;
	}
	
	IEnumerator UseShield() {
		SetInvincible(true);
		shieldPrefab.SetActive(true);
		
		MeshRenderer shieldMesh = shieldPrefab.GetComponent<MeshRenderer> ();
		
		while (startShield < shieldTimer) {
			startShield += Time.deltaTime + 0.1f;
				
			if ((shieldTimer - startShield) < 0.5f) {
				shieldMesh.enabled = false;
				yield return new WaitForSeconds(0.05f);
				shieldMesh.enabled = true;
				yield return new WaitForSeconds(0.05f);
			}
			else {
				yield return new WaitForSeconds(0.1f);
			}
		}
		
		startShield = 0;
		shieldReady = false;
		
		SetInvincible(false);
		shieldPrefab.SetActive(false);
		
		yield return null;
	}
}
