using UnityEngine;
using System.Collections;

public class DamagePlayer : MonoBehaviour {

	public int inflictDamage;
	public string statusEffectType;

	void OnTriggerEnter2D(Collider2D other){
		
		if (other.tag == "Player") {

			//If the player is not in invincible status, then go ahead and do damage and other effects
			if(!other.GetComponent<PlayerParameters>().invincible){
				Scoring.score -= 500;
				other.GetComponent<PlayerParameters>().InflictDamage(inflictDamage, statusEffectType);
				//Destroy(this.gameObject); //Destroy itself to prevent from being called multiple times
			}

			
		}
		
	}
}
