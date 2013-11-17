using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Author: kurtdog
 * this class is used to set the starting positions of all the players.l
 * */

public class StartZone : MonoBehaviour {
	
	public List<Transform> players; // a list of players, each player's transform should be set at
	
	// Use this for initialization
	void Start () {
		
		//Mesh mesh = GetComponent<MeshFilter>().mesh; // get the mesh of the startZone
		float width = transform.lossyScale.z;
		float height = transform.localScale.y;
		Vector3 startPosition = new Vector3(transform.position.x,transform.position.y-height/2, transform.position.z+width/2);
		float offset = 0;
		float increment = width/players.Count; // size of the z component of the startZone mesh/ # of players. Divide the z component of the zone to evenly space out the players
		Debug.Log ("width: " + width );
		Debug.Log ("increment: " + increment);
		Debug.Log ("StartZonePos: " + transform.position);
		
		foreach(Transform t in players) // for each of the players
		{
			Vector3 placement = startPosition - new Vector3(0,0,offset); // set the starting position	
			Debug.Log ("placement: " + placement);
			t.position = placement;
			offset += increment; // increment the offset
		}
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
