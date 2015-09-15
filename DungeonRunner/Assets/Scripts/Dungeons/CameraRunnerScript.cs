using UnityEngine;
using System.Collections;

public class CameraRunnerScript : MonoBehaviour {

	public Transform player;

	//used for altering the distances on the screen by x and y
	public int alterByX;
	public int alterByY;

	void Update () {
		transform.position = new Vector3 (player.position.x + alterByX, alterByY, -10);
	}
}
