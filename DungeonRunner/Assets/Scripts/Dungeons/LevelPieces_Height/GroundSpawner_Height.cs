using UnityEngine;
using System.Collections;

public class GroundSpawner_Height : MonoBehaviour {

	//enum to store low, medium, and high elevation for the elevation the piece starts and ends at.
	public enum Height{low, med, high};

	//start and end elevation variable declaration
	public Height startType;
	public Height endType;
}