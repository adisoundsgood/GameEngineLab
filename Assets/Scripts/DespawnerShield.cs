using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnerShield : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
        if (col.CompareTag("enemyBullet") || col.CompareTag("missile")) {
            Destroy (col.gameObject); 
        }
	}
}
