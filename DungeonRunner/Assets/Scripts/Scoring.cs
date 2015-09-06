using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Scoring : MonoBehaviour {

	public static int score = 0;
	public static int placement = 0;

	public Text scoreText;
	

	// Fixed update only updates on fixed frames
	void FixedUpdate () {
		score += 1;

		scoreText.text = "" + score;
	}
}
