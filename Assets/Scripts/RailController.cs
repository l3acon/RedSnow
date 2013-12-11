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
	public Vector3[] railpos;											// Create an arbitrarily sized array of Vector3
	private BetterList<float> rldists = new BetterList<float>();		// List of distances between waypoints
	private float[] rdists;
	private BetterList<Vector3> arrayRvec = new BetterList<Vector3>();
	public Vector3[] railvecs;
	
	public Vector3 testpos;
	public bool onRail;													// Is the player on the rail?
	public Vector3 railVec;												// Rail vector player is currently on
	public float colliderSize;											// Rail collider area (xyz)
	// Initialize the size of the rail by iterating through the waypoints
	void Start () {
		this.gameObject.tag = "Rail";									// Makes this object a Rail (for the lazy)
		int i;
		for(i = 0; i < this.transform.childCount; i++)
		{
			arrayRWP.Add(this.transform.GetChild(i).position);			// Add child to the list iteratively
			//this.transform.GetChild(i).gameObject.AddComponent()
			if(i < this.transform.childCount - 1)
			{
				rldists.Add(Vector3.Distance(this.transform.GetChild(i).position, this.transform.GetChild(i+1).position));
				arrayRvec.Add(this.transform.GetChild(i+1).position - this.transform.GetChild(i).position);
			}
		}
		railpos = arrayRWP.ToArray();									// Convert the list into a vector array
		arrayRWP.Release();
		rdists = rldists.ToArray();
		rldists.Release();
		railvecs = arrayRvec.ToArray();
		arrayRvec.Release();

		for(i = 0; i < this.transform.childCount - 1; i++)
		{
			makeColliders(i);	
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 temp;
//		int i;
//		for(i = 0; i < (this.transform.childCount - 1); i++)
//		{
//			onRail = Physics.Linecast(railpos[i],railpos[i+1]);
//			if(onRail)
//			{
//				Debug.Log("onRail "+GetInstanceID());
//				set_RVector(railpos[i]);
//			}
//			Debug.DrawLine(railpos[i],railpos[i+1]);
//		};

	}

	public void set_RVector(Vector3 arg)
	{
		railVec = arg;
	}
	public Vector3 get_RVector() { return railVec; }
	
	public GameObject get_gobj() { return this.gameObject; }
	
	/* author: ecmartz
	 * Function used to create colliders between all the waypoints on the rail
	 */
	void makeColliders(int arg)
	{
		BoxCollider bc;
		Vector3 temp;
		bc = this.transform.GetChild(arg).gameObject.AddComponent("BoxCollider") as BoxCollider;
		
		temp = this.transform.GetChild(arg+1).transform.localPosition - this.transform.GetChild(arg).transform.localPosition;
		
		bc.center = new Vector3(temp.x/2.0f, temp.y/2.0f, temp.z/2.0f);
		bc.size = new Vector3(temp.x + colliderSize, temp.y + colliderSize, temp.z + colliderSize);
		bc.tag = "Rail";
	}
	
}
