using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

	public float speed;

	private Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody> ();
	}

	void FixedUpdate() {
		rb.MovePosition(rb.position + transform.up * speed * Time.deltaTime);
	}
}
