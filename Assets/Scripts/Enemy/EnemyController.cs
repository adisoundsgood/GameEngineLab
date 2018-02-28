using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float speed;
	public bool patrol = true;

	public Transform[] waypoints;
	public int curWaypoint;
	public Vector3 target;
	public Vector3 moveDirection;
	public Vector3 velocity;

	void Update() {
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

		transform.Rotate (new Vector3 (0, 0, 300) * Time.deltaTime);
	}
}
