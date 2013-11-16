using UnityEngine;
using System.Collections;

/* author: ecmartz
 * RailController.cs
 * Attach this script to a gameobject with the children being waypoints.
 * Example: Rail1 has children Waypt1, Waypt2, Waypt3. Attach this script to Rail1
 * 
 */

public class RailController : MonoBehaviour {
	
	private BetterList<Vector3> arrayRWP = new BetterList<Vector3>();	// Create an arbitrarily sized list of waypoint vectors
	private Vector3[] railline;											// Create an arbitrarily sized array of Vector3
	
	public bool onRail;													// Is the player on the rail?
	public Vector3 railVec;												// Rail vector player is currently on
	// Initialize the size of the rail by iterating through the waypoints
	void Start () {
		int i;
		for(i = 0; i < this.transform.childCount; i++)
		{
			arrayRWP.Add(this.transform.GetChild(i).position);			// Add child to the list iteratively
		}
		railline = arrayRWP.ToArray();									// Convert the list into a vector array
	}
	
	// Update is called once per frame
	void Update () 
	{
		int i;
		for(i = 0; i < (this.transform.childCount - 1); i++)
		{
			onRail = Physics.Linecast(railline[i],railline[i+1]);
			if(onRail)
			{
				set_RVector(railline[i]);
			}
			Debug.DrawLine(railline[i],railline[i+1]);
		}
	}

	public void set_RVector(Vector3 arg)
	{
		railVec = arg;
	}
	public Vector3 get_RVector() { return railVec; }
}
