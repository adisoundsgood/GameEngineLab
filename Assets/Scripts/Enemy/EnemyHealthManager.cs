using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour {

	public GameObject[] enemy;
	
	[SerializeField]
	private GameObject player1;
	[SerializeField]
	private GameObject player2;

	public float maxHP = 100f;
	public float curHP;
	public Slider healthSlider;

	public GameObject winCanvas;


	void Awake() {
		curHP = maxHP;
		enemy = GameObject.FindGameObjectsWithTag ("enemy");
	}

	public void gotHit(float dmg) {
		curHP -= dmg;
		healthSlider.value = curHP;

		if (curHP <= 0) {
			for (int i = 0; i < enemy.Length; i++) {
				enemy[i].SendMessage("Death");
			}

			if (player1) {
				player1.SendMessage ("SetInvincible", true);
			}

			if (player2) {
				player2.SendMessage ("SetInvincible", true);
			}

			winCanvas.SetActive(true);
		}
	}
}
