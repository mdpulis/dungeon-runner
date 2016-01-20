using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManagement : MonoBehaviour {

	public Text hpText;
	public Image hpBar;
	private GameObject player;
	
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		hpText.text = "HP: " + player.GetComponent<PlayerParameters> ().PlayerHealth;
	}

	public void UpdateHealthVisible(){
		hpText.text = "HP: " + player.GetComponent<PlayerParameters> ().PlayerHealth;
		hpBar.fillAmount = (float)player.GetComponent<PlayerParameters> ().PlayerHealth / DataContainer.maxPlayerHealth;
	}

	public void DisplayDeath(){
		hpText.text = "<b>YOU DIED</b>";
	}



}
