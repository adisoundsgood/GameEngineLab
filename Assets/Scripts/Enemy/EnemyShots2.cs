using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShots2 : MonoBehaviour {

	public GameObject shot;
    public GameObject missile;
	public Transform bulletSpawner;
	public float fireRate;
    public float missileRate;
    private float nextFire = 0.0f;
    private float missileNextFire = 5f;


	void Update() {
        if ((Time.time > nextFire) && (Time.time - missileNextFire != 0f)) {
			nextFire = Time.time + fireRate;
			Instantiate (shot, bulletSpawner.position, bulletSpawner.rotation);
		}
        if (Time.time  > missileNextFire) {
            missileNextFire = Time.time + missileRate;
            Instantiate(missile, bulletSpawner.position, new Quaternion(90f,180f,0f,bulletSpawner.rotation.w));
        }
	}
}
