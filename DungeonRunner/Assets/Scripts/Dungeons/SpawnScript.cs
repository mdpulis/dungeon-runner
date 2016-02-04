using UnityEngine;
using UnityEngine.Sprites;
using System.Collections;
using System.Collections.Generic;

public class SpawnScript : MonoBehaviour {

	//the ground to be spawned
	//public GameObject spawnGround;

	//List of spawnable Ground Game Objects
	public List<GroundPiece> spawnableGrounds = new List<GroundPiece>();
	public List<GroundPiece> spawnableSpawnerGrounds = new List<GroundPiece> ();
	//the variable by which the ground needs to be spawned. For example, each set of bricks spawns 40 away from each other
	public float changeSpawnPlacementBy;

	//Number of grounds spawned per spawner collider enter
	private const int NUM_GROUND_SPAWNS = 10;

	void OnTriggerEnter2D(Collider2D other){

		if (other.tag == "Player") {
			//Increase Scoring.placement so that the ground is created in proper places
			Scoring.placement++;
			Debug.Log ("Line before calling RandomSpawnGround");
			RandomSpawnGround(); //Spawn our ground

			Destroy(this.gameObject); //Destroy itself to prevent from being called multiple times

		}

	}


	//Randomizes the ground that is spawned
	void RandomSpawnGround () {
		//Edit later to account for string algorithm
		GroundPiece randomSpawnGround, randomSpawnGroundTemp;
		Debug.Log ("Successfully called RandomSpawnGround and declared GameObjects");
		//first spawn object, so let's make an object with another spawner on it
		randomSpawnGround = spawnableSpawnerGrounds[Random.Range (0, spawnableSpawnerGrounds.Count)];
		Debug.Log ("count " + spawnableSpawnerGrounds.Count);

		//use i * 10 since each object is 10 away from each other
		GameObject zomg = (GameObject)Instantiate (randomSpawnGround.gameObject,
			new Vector3 (Scoring.placement * changeSpawnPlacementBy, 0, 0),
			Quaternion.identity);

		randomSpawnGroundTemp = randomSpawnGround;
		//Debug.Log (randomSpawnGround.name + " " + randomSpawnGroundTemp.name);
		if (randomSpawnGround.endType == Height.low) {
			Debug.Log ("successfully measured elevation of " + randomSpawnGround.GetComponent<GroundPiece> ().name);
		} else {
			Debug.Log ("skipped measure of elevation");
		}

		Debug.Log ("Successfully instantiated GameObjects");
		for (int i = 1; i < NUM_GROUND_SPAWNS; i++) {
			//used to loop until the piece fits. Start at 1 because 0 is spawn ground, handled above.
			bool groundPieceFits = false;

			do {
				//Otherwise, we will choose from our range of selectable objects according to our Unity dragged and dropped grounds
				randomSpawnGround = spawnableGrounds [Random.Range (0, spawnableGrounds.Count)];
				//check if the piece fits. If it does, set the comparison (temp) ground to current and switch flag to end loop.
				Debug.Log ("Successfully reached CheckIfPieceFits in the do/while loop");
				if (CheckIfPieceFits (randomSpawnGroundTemp.GetComponent<GroundPiece>(), randomSpawnGround.GetComponent<GroundPiece>())) {
					Debug.Log ("Successfully completed CheckIfPieceFits");
					groundPieceFits = true;
					randomSpawnGroundTemp = randomSpawnGround;
				}
			} while (groundPieceFits = false);

			//use i * 10 since each object is 10 away from each other
			GameObject gmobj = (GameObject)Instantiate (randomSpawnGround.gameObject,
				new Vector3 (Scoring.placement * changeSpawnPlacementBy + (i * 10), 0, 0),
				Quaternion.identity);
		}
	}

	//check if the next random piece will fit with the current piece
	bool CheckIfPieceFits (GroundPiece currentPiece, GroundPiece nextPiece) {
		Debug.Log ("Successfully reached CheckIfPieceFits " + currentPiece.name);
		if (currentPiece.endType == Height.low && nextPiece.startType == Height.high) {
			return false;
		} else {
			return true;
		}
	}
}