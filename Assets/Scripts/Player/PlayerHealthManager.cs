using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour {

    public int lives;

	[SerializeField]
	private Text livesText;

	void Update() {
		livesText.text = "Lives: " + lives;
	}

    public int getLives() {
        return lives;
    }

    public void gotHit () {
        lives--;

        if (lives <= 0) {
            Debug.Log("You suck!");
        }
    }
}
