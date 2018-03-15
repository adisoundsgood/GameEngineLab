using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float movementSpeed = 7f;

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
	private float invincibleTimer = 1.5f;
	private float startTime = 0f;
	
	// Skills
	private bool ultReady = true;
	[SerializeField]
	private float ultTimer = 3f;
	private float startUlt = 0f;
	private float ultCD = 13f;
	private float lastUlt;
	
	private bool shieldReady = true;
	[SerializeField]
	private float shieldTimer = 3f;
	private float startShield = 0f;
	private float shieldCD = 10f;
	private float lastShield;

	// Shooting
	public GameObject shot;
	public GameObject rapidShot;
	public Transform bulletSpawner;
	public float fireRate = 0.25f;
	public float rapidRate = 0.1f;
	private float nextFire;

    //Audio
    private AudioSource[] audioSources;
    private AudioSource shootAudioSource;
    private AudioSource hurtAudioSource;
    private AudioClip playerHurt;
    private AudioClip playerShoot;
    private AudioClip forceField;

	void Awake() {
		// Instantiating players
		startPos = transform.position;
		startRotation = shipPrefab.transform.rotation;

		playerPrefab = (GameObject) Instantiate(shipPrefab, startPos, startRotation);
		playerPrefab.transform.parent = this.transform;
		rigidBody = GetComponent<Rigidbody>();
		rigidBody.position = startPos;

		shipMesh = playerPrefab.GetComponent<MeshRenderer>();
		
		shieldPrefab.SetActive(false);

		// Movement constraints
		movement = Vector3.zero;

		halfWidth = playerPrefab.GetComponent<MeshRenderer>().bounds.extents.x;
		halfHeight = playerPrefab.GetComponent<MeshRenderer>().bounds.extents.y;

		cameraBounds = CameraExtensions.OrthographicBounds(mainCamera);
		screenhalfX = cameraBounds.extents.x;
		screenhalfY = cameraBounds.extents.y;

		// Health
		hm = healthManager.GetComponent<PlayerHealthManager>();
		isInvincible = false;

		// Audio
        audioSources = GetComponents<AudioSource>();
        shootAudioSource = audioSources[0];
        hurtAudioSource = audioSources[1];
        playerHurt = (AudioClip) Resources.Load("Audio/SFX/Player Hurt");
        playerShoot = (AudioClip) Resources.Load("Audio/SFX/PlayerShoot");
        forceField = (AudioClip) Resources.Load("Audio/SFX/PlayerForceField");
	}

	void Update() {

        // Play shoot music
        if (!isRespawning && !shootAudioSource.isPlaying) {
            shootAudioSource.PlayOneShot(playerShoot, 0.5f);
        }

		// Continuous shooting
		if (Time.time > nextFire && !isRespawning && !usingUlt) {
			nextFire = Time.time + fireRate;
			Instantiate (shot, bulletSpawner.position, bulletSpawner.rotation);
		}
		
		// Using ult
		if (Time.time > nextFire && !isRespawning && usingUlt) {
			nextFire = Time.time + fireRate;
			Instantiate (rapidShot, bulletSpawner.position, bulletSpawner.rotation);
		}
		
		// Skill CD tracking
		if (!shieldReady && lastShield < shieldCD) {
			lastShield += Time.deltaTime;
		}
		else {
			shieldReady = true;
		}
		
		if (!ultReady && lastUlt < ultCD) {
			lastUlt += Time.deltaTime;
		}
		else {
			ultReady = true;
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
			if (Input.GetButtonDown("Ult1") && ultReady) {
				lastUlt = 0f;
				StartCoroutine("UseUlt");
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
				lastUlt = 0f;
				StartCoroutine("UseUlt");
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
                hurtAudioSource.PlayOneShot(playerHurt);

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
        shootAudioSource.Stop();
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
        hurtAudioSource.PlayOneShot(forceField,0.7f);
		
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
	
	IEnumerator UseUlt() {
		usingUlt = true;
		
		float prevRate = fireRate;
		fireRate = rapidRate;
		
		while (startUlt < ultTimer) {
			startUlt += Time.deltaTime + 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
		
		usingUlt = false;
		fireRate = prevRate;
	}
}
