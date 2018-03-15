using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour {

	public GameObject enemy;
	
	[SerializeField]
	private GameObject player1;
	[SerializeField]
	private GameObject player2;

	public int maxHP = 100;
	public int curHP;
	public Slider healthSlider;

	public GameObject winCanvas;


	void Awake() {
		curHP = maxHP;
	}

	public void gotHit(int dmg) {
		curHP -= dmg;
		healthSlider.value = curHP;

		if (curHP <= 0) {
			enemy.SendMessage("Death");
			player1.SendMessage("SetInvincible", true);
			player2.SendMessage("SetInvincible", true);
			winCanvas.SetActive(true);
		}
	}
}
