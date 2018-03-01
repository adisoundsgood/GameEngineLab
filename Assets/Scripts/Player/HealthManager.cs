using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {


    public int lives;

	// Use this for initialization
	void Start () {
        Debug.Log("Lives Left: " + lives);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int getLives() {
        return lives;
    }

    public void gotHit () {
        lives--;
        Debug.Log("Lives Left: " + lives);
        if (lives < 0) {
            Debug.Log("You suck!");
        }
    }
}
