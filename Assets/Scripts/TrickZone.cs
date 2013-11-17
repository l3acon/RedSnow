using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TrickZone : MonoBehaviour {
	
	public List<Tricks> tricks;
	
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter()
	{
		if(tricks.Count > 0)
		{
			int trick = Random.Range (0,tricks.Count);
			Debug.Log(tricks[trick]);
		}
		else{
			Debug.Log("ERROR: There are no tricks assigned to this trick zone");	
		}
	}
	
	
	public enum Tricks{backflip,frontflip,spin360,spin720,misty,crazyShit}
}
