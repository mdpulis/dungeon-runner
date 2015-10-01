using UnityEngine;
using System.Collections;

public class TimeScaleSpeed : MonoBehaviour {

	public float baseTimeScale = 1.0f;
	
	// Use this for initialization
	void Start () {
		Time.timeScale = baseTimeScale + DataContainer.timeScaleMod;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
