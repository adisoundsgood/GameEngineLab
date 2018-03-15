using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	// Movement
	public float speed;
	public bool patrol = true;

	public Transform[] waypoints;
	public int curWaypoint;
	public Vector3 target;
	public Vector3 moveDirection;
	public Vector3 velocity;

	// Health
	public GameObject EnemyHealthManager;
	private EnemyHealthManager ehm;

    // Audio
    private AudioSource audioSource;
    private AudioClip enemyHurt;


	void Awake() {
		ehm = EnemyHealthManager.GetComponent<EnemyHealthManager>();
        audioSource = GetComponent<AudioSource>();
        enemyHurt = (AudioClip) Resources.Load("Audio/SFX/EnemyHurt");
	}

	void Update() {
		// Movement
		if (curWaypoint < waypoints.Length) {
			target = waypoints [curWaypoint].position;
			moveDirection = target - transform.position;
			velocity = GetComponent<Rigidbody> ().velocity;

			if (moveDirection.magnitude < 1) {
				curWaypoint++;
			}
			else {
				velocity = moveDirection.normalized * speed;
			}
		}
		else {
			if (patrol) {
				curWaypoint = 0;
			}
			else {
				velocity = Vector3.zero;
			}
		}

		GetComponent<Rigidbody> ().velocity = velocity;

		// Rotation
		transform.Rotate (new Vector3 (0, 0, 300) * Time.deltaTime);
	}

	void OnTriggerEnter(Collider col) {
		if (col.CompareTag("playerBullet")) {
			ehm.gotHit (5);
            if (!audioSource.isPlaying) {
                audioSource.PlayOneShot(enemyHurt,0.4f);
            }
		}
	}

	void Death() {
		Destroy (this.gameObject);
	}
}
