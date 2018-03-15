using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour {

    private bool titleSceneFlag; // To determine if the Title Scene is loaded from itself
    private AudioSource source;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(transform.gameObject);
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneChange;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneChange;
    }

    private void OnSceneChange(Scene scene, LoadSceneMode mode) {
        if (scene.name == "MainMenu") {
            if (!titleSceneFlag) {
                AudioClip titleClip = Resources.Load<AudioClip> ("Audio/Music/");

                source.clip = titleClip;

                source.Play ();
            }
            titleSceneFlag = true;
        }

        if (scene.name == "Level1") {
            titleSceneFlag = false;
            AudioClip introClip = Resources.Load<AudioClip>("Audio/Music/BackgroundBEat");


            if (source && introClip) {
                source.clip = introClip;
                source.PlayOneShot(introClip);
            }

        }
    }
}
