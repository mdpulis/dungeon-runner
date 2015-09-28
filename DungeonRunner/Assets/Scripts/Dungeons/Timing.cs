using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Diagnostics;

public class Timing : MonoBehaviour {

	public Text timerText;
	public Stopwatch stopwatch = new Stopwatch();
	private TimeSpan ts;

	// Use this for initialization
	void Start () {
		//stopwatch = new Stopwatch ();
		stopwatch.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		ts = stopwatch.Elapsed;
		/*
		string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
		                                   ts.Hours, ts.Minutes, ts.Seconds,
		                                   ts.Milliseconds / 10);
		*/
		string elapsedTime = String.Format ("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
		timerText.text = elapsedTime;
	}
}
