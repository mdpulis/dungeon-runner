using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TownAreaLoader : MonoBehaviour {

	public GameObject mainCamera;
	public GameObject eventSystem;
	public Text areaDebugText;

	//Load the town buildings
	public void LoadArea(string areaSceneName){
		areaDebugText.text = "You have selected area: " + areaSceneName;
		//NOTE: Until we have real scenes to go to, this will cause double cameras and event systems due to DontDestroyOnLoad
		Application.LoadLevel (areaSceneName);
		DontDestroyOnLoad (mainCamera);
		//It might be better to destroy our event system? It affects wasd/controller UI inputs
		DontDestroyOnLoad (eventSystem);
	}

	//Start the dungeon sequence
	public void GoToDungeon(string dungeonSceneName){
		Application.LoadLevel (dungeonSceneName);
	}
}
