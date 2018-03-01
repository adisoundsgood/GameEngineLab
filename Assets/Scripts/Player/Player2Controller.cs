using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour {

    [SerializeField]
    private float movementSpeed;

    //player and ship prefabs
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject playerShip;

    // camera and bounds
    [SerializeField]
    private Camera mainCamera;
    private Bounds cameraBounds;
    private float halfWidth, halfHeight, screenhalfX, screenhalfY;

    // setup
    private Rigidbody rigidBody;
    private Vector3 input, movement;
    private Vector3 startPos;
    private Quaternion startRotation;
    private MeshRenderer shipMesh;

    // health
    public GameObject healthManager;
    HealthManager hm;
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
        startRotation = playerPrefab.transform.rotation;

        playerShip = (GameObject) Instantiate(playerPrefab, startPos, startRotation);
        playerShip.transform.parent = this.transform;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.position = startPos;

        shipMesh = playerShip.GetComponent<MeshRenderer>();

        movement = Vector3.zero;

        halfWidth = playerShip.GetComponent<MeshRenderer>().bounds.extents.x;
        halfHeight = playerShip.GetComponent<MeshRenderer>().bounds.extents.y;

        cameraBounds = CameraExtensions.OrthographicBounds(mainCamera);
        screenhalfX = cameraBounds.extents.x;
        screenhalfY = cameraBounds.extents.y;

        hm = healthManager.GetComponent<HealthManager>();
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

    private void OnTriggerEnter(Collider other)
    {
        if (!isInvincible) {
            if (other.CompareTag("enemy bullet")) {
                if (hm.getLives() < 0) {
                    Destroy(this);
                }

                hm.gotHit();
                transform.position  = startPos;
                rigidBody.position = startPos;
                playerPrefab.transform.rotation = startRotation;
                isInvincible = true;
                StartCoroutine("Flicker");
            }
        }
    }

    IEnumerator Flicker() {
        while (startTime < invincibleTimer) {
            startTime += Time.deltaTime + 0.25f;
            //Debug.Log("startTime: " + startTime);
            shipMesh.enabled = false;
            yield return new WaitForSeconds(0.125f);
            shipMesh.enabled = true;
            yield return new WaitForSeconds(0.125f);
        }
        isInvincible = false;
        startTime = 0;
        yield return null;
    }
}
