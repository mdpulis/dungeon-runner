using UnityEngine;
using UnityEngine.Sprites;
using System.Collections;
using System.Collections.Generic;

public class SpawnScript : MonoBehaviour {



	//the ground to be spawned
	//public GameObject spawnGround;

	//List of spawnable Ground Game Objects
	public List<GameObject> spawnableGrounds = new List<GameObject>();
	public List<GameObject> spawnableSpawnerGrounds = new List<GameObject> ();
	//the variable by which the ground needs to be spawned. For example, each set of bricks spawns 40 away from each other
	public float changeSpawnPlacementBy;

	//Number of grounds spawned per spawner collider enter
	private const int NUM_GROUND_SPAWNS = 10; 
	


	void OnTriggerEnter2D(Collider2D other){

		if (other.tag == "Player") {
			//Increase Scoring.placement so that the ground is created in proper places
			Scoring.placement++;
			RandomSpawnGround(); //Spawn our ground

			Destroy(this.gameObject); //Destroy itself to prevent from being called multiple times

		}

	}


	//Randomizes the ground that is spawned	
	void RandomSpawnGround () {
		//Edit later to account for string algorithm
		for(int i = 0; i < NUM_GROUND_SPAWNS; i++){
			GameObject randomSpawnGround;

			//if i == 0, first spawn object, so let's make an object with another spawner on it
			if(i == 0){
				randomSpawnGround = spawnableSpawnerGrounds[Random.Range (0, spawnableSpawnerGrounds.Count)];
			}
			else{
				//Otherwise, we will choose from our range of selectable objects according to our Unity dragged and dropped grounds
				randomSpawnGround = spawnableGrounds[Random.Range(0, spawnableGrounds.Count)];
				}
			}

			//use i * 10 since each object is 10 away from each other
			GameObject gmobj = (GameObject) Instantiate(randomSpawnGround, 
			                                            new Vector3(Scoring.placement * changeSpawnPlacementBy + (i * 10), 0, 0), 
			                                            Quaternion.identity);
		}
		
	}
}
