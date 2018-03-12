using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour {

    public int lives;

	[SerializeField]
	private Text livesText;

	public GameObject loseCanvas;


	void Update() {
		if (lives >= 0) {
			livesText.text = "Lives: " + lives;
		}
		else if (lives < 0) {
			livesText.text = "Lives: 0";
		}
	}

    public int getLives() {
        return lives;
    }

    public void gotHit () {
        lives--;

        if (lives <= -2) {
			loseCanvas.SetActive(true);
        }
    }
}
