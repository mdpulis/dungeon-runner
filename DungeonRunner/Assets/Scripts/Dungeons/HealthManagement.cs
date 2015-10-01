using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManagement : MonoBehaviour {

	public Text hpText;
	public Image hpBar;
	private GameObject player;
	
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		hpText.text = "HP: " + player.GetComponent<PlayerParameters> ().playerHealth;
	}

	public void UpdateHealthVisible(){
		hpText.text = "HP: " + player.GetComponent<PlayerParameters> ().playerHealth;
		hpBar.fillAmount = (float)player.GetComponent<PlayerParameters> ().playerHealth / DataContainer.maxPlayerHealth;
	}

	public void DisplayDeath(){
		hpText.text = "<b>YOU DIED</b>";
	}



}
