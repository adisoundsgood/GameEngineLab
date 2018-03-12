using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {

	public GameObject winCanvas;
	public GameObject loseCanvas;

	void Start() {
		winCanvas.SetActive(false);
		loseCanvas.SetActive(false);
	}
}
