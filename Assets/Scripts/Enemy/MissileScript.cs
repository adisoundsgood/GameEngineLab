using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour {


    public float speed;
    public float rotationSpeed = 1f;
    public float lifeTime = 4f;

    private GameObject target;
    private Quaternion rotateToTarget;
    private Vector3 dir;
    private Rigidbody rb;
    private float spawnTime;

	// Use this for initialization
	void Start () {
        spawnTime = Time.time;
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
        if (Time.time < spawnTime + lifeTime) {
            dir = (target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
            rotateToTarget = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation,rotateToTarget,Time.deltaTime*rotationSpeed);
            rb.velocity = new Vector3(dir.x,dir.y) * speed;
        } 
	}
}
