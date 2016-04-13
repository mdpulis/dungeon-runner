using UnityEngine;
using UnityEngine.Sprites;
using System.Collections;
using System.Collections.Generic;

public class SpawnScript : MonoBehaviour {
	/*****turns debugging printouts on or off*/																											public bool debugOn = false;
	/*****List of spawnable Ground Game Objects.*/
	public List<GroundPiece> spawnableGrounds = new List<GroundPiece>();
	public List<GroundPiece> spawnableSpawnerGrounds = new List<GroundPiece> ();
	public List<GroundPiece> genericDungeonSpawnableGrounds = new List<GroundPiece> ();
	/*****the variable by which the ground needs to be spawned. For example, each set of bricks spawns 40 away from each other.*/
	public float changeSpawnPlacementBy;
	/*****list of ground pieces that will be generated.*/
	private List<int> randomlySpawnedGround = new List<int> ();
	/*****when the game manager is loaded, level generation begins.*/
	void Awake(){																																		if (debugOn == true) {Debug.Log ("Successfully reached Awake() in SpawnScript");}
		Scoring.placement++;																															if (debugOn == true) {Debug.Log ("Incremented Scoring.placement");}
		int maxSpeed = (int)GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement> ().MaxSpeed;										if (debugOn == true) {Debug.Log ("set maxSpeed from PlayerMovement");}
		RandomSpawnGround(maxSpeed); /*****Spawn our ground.*/ 																							if (debugOn == true) {Debug.Log ("Generated all ground, destroying object");}
		Destroy(this.gameObject); /*****Destroy itself to prevent from being called multiple times.*/
	}

	/*****Randomizes the ground that is spawned. Places the pieces.*/
	void RandomSpawnGround (int playerSpeed) {																											if (debugOn == true) {Debug.Log ("Reached RandomSpawnGround");}
		int NUM_GROUND_SPAWN = CalculateLevelLength(playerSpeed);																						if (debugOn == true) {Debug.Log ("Finished CalculateLevelLength");}
		randomlySpawnedGround = CreateGroundList(NUM_GROUND_SPAWN);																						if (debugOn == true) {Debug.Log ("Finished CreateGroundList");}
		/*GameObject spawnSpawn = (GameObject)Instantiate (spawnableSpawnerGrounds[randomlySpawnedGround[0]].gameObject,
			new Vector3 (Scoring.placement * changeSpawnPlacementBy, 0, 0),
			Quaternion.identity);*/																														if (debugOn == true) {Debug.Log ("placed spawner piece");}
		for (int i = 1; i < randomlySpawnedGround.Count; i++) {																							if (debugOn == true) {Debug.Log ("reached for loop to place pieces of ground. i = " + i);}
			/*****use i * 10 since each object is 10 away from each other.*/
			GameObject spawnGround = (GameObject)Instantiate
				(spawnableGrounds[randomlySpawnedGround[i]].gameObject,
				new Vector3 (Scoring.placement * changeSpawnPlacementBy + (i * 10), 0, 0),
				Quaternion.identity);																													if (debugOn == true) {Debug.Log ("Successfully placed: " + spawnableGrounds [randomlySpawnedGround [i]].name);}
		}
	}

	/*****creates the list of ground pieces to be placed*/
	List<int> CreateGroundList(int levelLength) {																										if (debugOn == true) {Debug.Log ("Reached CreateGroundList");}
		List<int> groundList = new List<int> ();																										if (debugOn == true) {Debug.Log ("set groundList");}
		/*int spawnPieceRandomNumber = Random.Range (0, spawnableSpawnerGrounds.Count);*/
		int groundPieceRandomNumber;																													if (debugOn == true) {Debug.Log ("set spawnPieceRandomNumber, declared groundPieceRandomNumber");}
		/*groundList.Add(spawnPieceRandomNumber);*/																											if (debugOn == true) {Debug.Log ("Added spawnPieceRandomNumber to groundList");}
		GroundPiece randomSpawnGround = spawnableSpawnerGrounds[0];
		GroundPiece randomSpawnGroundTemp = randomSpawnGround;																							if (debugOn == true) {Debug.Log ("set randomSpawnGround and randomSpawnGroundTemp to a ground piece");}
		for (int i = 0; i < levelLength; i++) {																											if (debugOn == true) {Debug.Log ("Reached for loop. i = " + i);}
			bool groundPieceFits = false;																												if (debugOn == true) {Debug.Log ("Set groundPieceFits to false");}
			int j = 0;
			while (groundPieceFits == false) {																											if (debugOn == true) {Debug.Log ("Reached while loop. j = " + j++);}
				/*****Otherwise, we will choose from our range of selectable objects according to our Unity dragged and dropped grounds.*/
				groundPieceRandomNumber = Random.Range (0, spawnableGrounds.Count);																		if (debugOn == true) {Debug.Log ("set random value to groundPieceRandomNumber");}
				randomSpawnGround = spawnableGrounds [groundPieceRandomNumber];																			if (debugOn == true) {Debug.Log ("set randomSpawnGround to a random piece");}
				/*****check if the piece fits. If it does, set the comparison (temp) ground to current and switch flag to end loop.*/
				if (CheckIfPieceFits (randomSpawnGroundTemp, randomSpawnGround)) {																		if (debugOn == true) {Debug.Log ("Passed if statement in while loop");}
					groundPieceFits = true;																												if (debugOn == true) {Debug.Log ("set groundPieceFits to true to exit while loop");}
					randomSpawnGroundTemp = randomSpawnGround;																							if (debugOn == true) {Debug.Log ("Set randomSpawnGroundTemp to new piece");}
					groundList.Add (groundPieceRandomNumber);																							if (debugOn == true) {Debug.Log ("Successfully completed CheckIfPieceFits, set do/while flag to true, adding piece to list: " + randomSpawnGround.name);}
				} else {																																if (debugOn == true) {Debug.Log ("CheckIfPieceFits returned False, repeating while loop");}
					groundPieceFits = false;
				}
			}
		}
		return groundList;
	}

	/*****check if the next random piece will fit with the current piece.*/
	bool CheckIfPieceFits (GroundPiece currentPiece, GroundPiece nextPiece) {																			if (debugOn == true) {Debug.Log ("Successfully reached CheckIfPieceFits with piece: " + nextPiece.name);}
		if (currentPiece.endType.Equals(Height.low) &&
		   (nextPiece.startType.Equals(Height.high) || nextPiece.startType.Equals(Height.medium))) {													if (debugOn == true) {Debug.Log ("CheckIfPieceFits returning false");}
			return false;
		} else {																																		if (debugOn == true) {Debug.Log ("CheckIfPieceFits returning true");}
			return true;
		}
	}

	/*****returns length of level (# of pieces) based on player speed.*/
	int CalculateLevelLength (int speed) {																												if (debugOn == true) {Debug.Log ("Reached CalculateLevelLength");}
		int levelLength = (int) speed*4;																												if (debugOn == true) {Debug.Log ("Level Length: " + levelLength);}
		return levelLength;
	}

	/*****returns the list of grounds. Used for sending it to another player eventually.*/
	public List<int> getLevelGround {
		get { return randomlySpawnedGround; }
	}
}