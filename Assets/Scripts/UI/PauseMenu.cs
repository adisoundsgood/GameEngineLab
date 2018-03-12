using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public GameObject pauseCanvas;
	//public GameObject ControlsText;

	private bool paused = false;
	//private bool showControls = false;

	void Start() {
		pauseCanvas.SetActive (false);
		//ControlsText.SetActive (false);
	}

	void Update() {
		if (Input.GetButtonDown("Pause")) {
			paused = !paused;
		}

		if (paused) {
			pauseCanvas.SetActive (true);
			Time.timeScale = 0;
		}

		if (!paused) {
			pauseCanvas.SetActive (false);
			Time.timeScale = 1;
		}
	}

	public void Resume() {
		paused = false;
	}

/*
	public void Controls() {
		if (paused && showControls == false) {
			showControls = true;
			ControlsText.SetActive (true);
		}
		else if (paused && showControls == true){
			showControls = false;
			ControlsText.SetActive (false);
		}
		else {
			ControlsText.SetActive (false);
		}
	}
*/

}
