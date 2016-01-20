using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// Event Manager for all things that happen while playing the game
public class DungeonEventManager : MonoBehaviour {

	public delegate void PassListGameObject(List<GameObject> lgo);

	public static event PassListGameObject onGameReady;
	//public PassListGameObject 

	#region Functions
	#region PassListGameObject
	public static void RaiseAreaLoadCompleted(List<GameObject> listGO){
		if (onGameReady != null)
			onGameReady (listGO);
	}
	#endregion PassListGameObject







	#endregion Functions
}
