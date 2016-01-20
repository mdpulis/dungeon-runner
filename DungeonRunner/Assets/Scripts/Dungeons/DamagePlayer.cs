using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SynodicArc.DungeonRunner.StatusEffects;

public class DamagePlayer : MonoBehaviour {

	#region Constants
	//Tags
	public const string PLAYER = "Player";

	#endregion Constants

	public int InflictDamage;
	public int ScoreReduction;
	public List<StatusEffect> InflictStatusEffects;

	void OnTriggerEnter2D(Collider2D other){
		
		if (other.tag.Equals(PLAYER)) {
			//Since it's a player, we get the player parameters and movement, then do things there with them
			PlayerParameters playerParams = other.GetComponent<PlayerParameters> ();
			PlayerMovement playerMovement = other.GetComponent<PlayerMovement> ();

			//If the player is not in invincible status, then go ahead and do damage and other effects
			if(!playerParams.Invincible){
				Scoring.score -= ScoreReduction;
				playerParams.InflictDamage(InflictDamage, InflictStatusEffects); //Inflict damage and any potential status effects
				//Destroy(this.gameObject); //Destroy itself to prevent from being called multiple times
				
				playerMovement.Damage(1);
				//triggers the kick back from taking damage
			}

			
		}
		
	}
}
