using UnityEngine;
using UnityEngine.Sprites;
using System.Collections;

public class SpawnScript : MonoBehaviour {

	//the ground to be spawned
	public GameObject spawnGround;
	//the variable by which the ground needs to be spawned. For example, each set of bricks spawns 40 away from each other
	public float changeSpawnPlacementBy;

	void OnTriggerEnter2D(Collider2D other){

		if (other.tag == "Player") {
			//Increase Scoring.placement so that the ground is created in proper places
			Scoring.placement++;

			//Create gmobj reference in case we need to use it later
			GameObject gmobj = (GameObject) Instantiate(spawnGround, 
			                                            new Vector3(Scoring.placement * changeSpawnPlacementBy, 0, 0), 
			                                            Quaternion.identity);

			Destroy(this.gameObject); //Destroy itself to prevent from being called multiple times

		}

	}
}
