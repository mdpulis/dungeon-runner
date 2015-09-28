using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManagement : MonoBehaviour {

	public Text hpText;
	private GameObject player;
	
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		hpText.text = "HP: " + player.GetComponent<PlayerParameters> ().playerHealth;
	}

	public void UpdateHealthVisible(){
		hpText.text = "HP: " + player.GetComponent<PlayerParameters> ().playerHealth;
	}

	public void DisplayDeath(){
		hpText.text = "<b>YOU DIED</b>";
	}

}
