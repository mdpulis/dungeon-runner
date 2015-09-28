using UnityEngine;
using System.Collections;

public class DestroyerScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){

		//Case for player death/damage
		if (other.tag == "Player") {
			Debug.Log ("Attempting to destroy Player.");
			Debug.Break ();
			return;
		} else if (other.tag == "Spawner") {
			//We don't want to destroy our spawners, so let's avoid that by returning. 
			//They will auto destroy when the player hits them.
			Debug.Log ("Trying to destroy spawner.");
			return;
		}

		//Just some tests here to try avoiding too much destroying
		if(other.gameObject.transform.parent){
			if(other.gameObject.tag.Equals("InitialSegment") || other.gameObject.tag.Equals("DungeonSegment")){
				Destroy (other.gameObject);
			}else{
				Destroy (other.gameObject.transform.parent.gameObject);
			}
		}else{
			Destroy (other.gameObject);
		}


	}
}
