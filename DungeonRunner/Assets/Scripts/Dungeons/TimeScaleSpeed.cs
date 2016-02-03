using UnityEngine;
using System.Collections;

public class TimeScaleSpeed : MonoBehaviour {

	public float baseTimeScale = 1.0f; //the base time scale
	private float currentTimeScale; //the time scale while running through the dungeons
	
	// Use this for initialization
	void Start () {
		Time.timeScale = baseTimeScale + DataContainer.timeScaleMod;
		currentTimeScale = Time.timeScale;
	}

	/// Pauses/unpauses the game.
	public void PauseGame(){
		//If not paused, pauses
		if (!GameState.Paused) {
			Time.timeScale = 0;
			GameState.Paused = true;
		} else if (GameState.Paused) { //if paused, unpauses
			Time.timeScale = currentTimeScale;
			GameState.Paused = false;
		}
	}


	/// Changes the time scale.
	public void ChangeTimeScale(float alteration){
		currentTimeScale += alteration;
		if (currentTimeScale <= 0.0f) {
			currentTimeScale -= alteration;
			Debug.Log ("Time scale going too low.");
		} else {
			Time.timeScale = currentTimeScale;
		}
	}

}
