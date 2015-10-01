using UnityEngine;
using System.Collections;
using System.Diagnostics; //Conflicts with Debug.Log, so we need to use UnityEngine.Debug.Log in this file

public class PlayerParameters : MonoBehaviour{

	public const int KNOCKBACK = 8;

	private HealthManagement healthManagement;

	public double invincibilityTime;
	public Stopwatch invincibilityStopwatch;
	public bool invincible = false;
	
	public int playerHealth;
	
	private static bool currentlyInvisible = false;

	// Use this for initialization
	void Start () {
		healthManagement = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<HealthManagement> ();
		invincibilityStopwatch = new Stopwatch ();
		playerHealth = DataContainer.maxPlayerHealth;
		invincibilityTime = 1 + DataContainer.additionalInvincibilityTime;
	}

	//Inflict damage, apply invincibility and status effects
	public void InflictDamage(int dmg, string statusEffect){
		invincible = true;
		playerHealth -= dmg;
		healthManagement.UpdateHealthVisible ();

		if (playerHealth <= 0) {
			UnityEngine.Debug.Log("Player Health reached 0. Now ending game.");
			healthManagement.DisplayDeath();
			Time.timeScale = 0;
		}

		//Start our invincibility timer by resetting it to 0 and getting back into it
		invincibilityStopwatch.Reset ();
		invincibilityStopwatch.Start ();

		StartCoroutine (FlashingEffect(0.20f));
		//Knockback effect. Needs some work. If you guys wanna look into this, please do.
		transform.position = new Vector3(Mathf.Lerp (
			transform.position.x,
			transform.position.x - KNOCKBACK,
			0.5f), transform.position.y, transform.position.z);
		UnityEngine.Debug.Log ("Rendering invisible!");
	}

	public IEnumerator FlashingEffect(float waitTime){
		//If you are currently invisible, go un-invisible, and vice versa
		if (currentlyInvisible) {
			this.GetComponent<SpriteRenderer> ().color = (Color32) ColorControls.HexToColor("FFFFFFFF");
			currentlyInvisible = false;
		} else {
			this.GetComponent<SpriteRenderer> ().color = (Color32) ColorControls.HexToColor("FFFFFF11");
			currentlyInvisible = true;
		}

		UnityEngine.Debug.Log ("Stopwatch: " + TimeSpanUtil.ConvertMillisecondsToSeconds (invincibilityStopwatch.ElapsedMilliseconds));
		UnityEngine.Debug.Log ("Invincibility Time: " + invincibilityTime);
		

		//If our invincibility time is up, then we turn ourselves back to full color and vulnerable again
		if (TimeSpanUtil.ConvertMillisecondsToSeconds(invincibilityStopwatch.ElapsedMilliseconds) > invincibilityTime) {
			UnityEngine.Debug.Log ("Now ending invincibility.");
			invincible = false;
			this.GetComponent<SpriteRenderer> ().color = (Color32) ColorControls.HexToColor("FFFFFFFF");
			currentlyInvisible = false;
		} else {
			yield return new WaitForSeconds(waitTime);
			StartCoroutine (FlashingEffect(0.20f));
		}
	}


	//Apply status effect. Ideally, we will create a StatusEffect object class at some point to handle this instead of string passing
	public void ApplyStatusEffect(string statusEffect){

	}
}
