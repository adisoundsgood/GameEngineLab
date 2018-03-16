using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour {

    public float speed;
    public float lifeTime = 4f;

    private GameObject target;
    private Vector3 dir;
    private Rigidbody rb;
    private float spawnTime;

	void Start () {
		rb = GetComponent<Rigidbody> ();
        spawnTime = Time.time;
        float random = Random.Range(0f,1f);

        if (random >= 0.5f) {
            target = GameObject.FindWithTag("p1");
        }
		else {
            target = GameObject.FindWithTag("p2");
        }
	}

	void Update () {
        if (Time.time < spawnTime + lifeTime) {
            dir = (target.transform.position - transform.position).normalized;
			transform.LookAt (target.transform);
            rb.velocity = new Vector3(dir.x, dir.y) * speed;
        } 
	}
}
