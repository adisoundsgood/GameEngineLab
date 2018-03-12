using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	public void Restart() {
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}

	public void NextLevel() {
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
	}

	public void Title() {
		SceneManager.LoadScene ("title", LoadSceneMode.Single);
	}

	public void Quit() {
		Application.Quit ();
	}
}
