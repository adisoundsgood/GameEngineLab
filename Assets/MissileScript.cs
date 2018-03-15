using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour {


    public float speed;
    public float rotationSpeed = 1f;

    private GameObject target;
    private Quaternion rotateToTarget;
    private Vector3 dir;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody> ();
        float random = Random.Range(0f,1f);
        if (random >= 0.5f) {
            target = GameObject.FindWithTag("p1");
        } else {
            target = GameObject.FindWithTag("p2");
        }

	}
	
	// Update is called once per frame
	void Update () {
        dir = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        rotateToTarget = Quaternion.AngleAxis(angle, Vector3.left);
        transform.rotation = Quaternion.Slerp(transform.rotation,rotateToTarget,Time.deltaTime*rotationSpeed);
        rb.velocity = new Vector3(dir.x,dir.y) * speed;
	}
}
