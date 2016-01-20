using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


public class Scoring : MonoBehaviour {
	public GameObject player;
	public static int score = 0;
	public static float lastPosition = 0;
	public static int countForward = 0;
	public static int countStop = 0;
	public static int placement = 0;
	public Text scoreText;
	
	// Fixed update only updates on fixed frames
	void FixedUpdate () {
		//sets the score to that of the player's X coordinate.
		//if player is moving, score increments up
		//if player is not moving, score increments down
		if (player.transform.localPosition.x >= (lastPosition + 0.1)) {
			//Debug.Log (lastPosition + " - " + player.transform.localPosition.x);
			countForward += 1;
			//if forward has triggered 10 times in a row, incriment score by 1
			if (countForward >= 10) {
				score += 1;
				//resets forward and non forward counters
				countForward = 0;
				countStop = 0;
			}
		} else if (player.transform.localPosition.x >= (lastPosition - 0.5) && player.transform.localPosition.x < (lastPosition + 0.05)) { //the -0.5 and + 0.05 are present to handle floating point errors and engin jitter
			countStop += 1;
			//if not moving has triggered 30 times in a row, deincriment score by 1
			if (countStop >= 15) {
				Debug.Log("player stoped moving");
				score -= 1;
				//resets forward and non forward counters
				countStop = 0;
				countForward = 0;
			}
		} else {
			//Debug.Log("Player moved backwards");
			//score -= 1000;
		}
		if (score < 0) {
			score = 0;
		}
		//records curent position for checking on next call.
		lastPosition = player.transform.localPosition.x;
		//prints score to visable text field.
		scoreText.text = String.Format("{0:000000000000}", score);
	}
}
