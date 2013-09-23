using UnityEngine;
using System.Collections;

/// <summary>
/// Follow camera.cs
/// author: ecmartz
/// Essentially we have no control over this camera. It is a rip off of Follow. We need the ability to aim.
/// 
/// Aim: 360 degrees without the camera going under the terrain
/// 	For this, we would need a crosshair empty object at which the camera can pan over to the player and
/// 	then perform a LookAt() to the crosshair object.
/// </summary>/

public class FollowCamera : MonoBehaviour {
	

	public float damping = 1.0f;
	
	private Vector3 offset;
	public GameObject target;
	//public Transform target;
	private float currentAngle;
	private float desiredAngle;
	private float angle;
	private Quaternion rotation;
	GameObject theCube;
	BoxController bc;
	
	// Use this for initialization
	void Start () {
		
		//offset = target.transform.position - transform.position; // on ground - follow rotation
		offset = transform.position - target.transform.position; // in air - don't follow rotation
		//target = target;
		
		theCube = GameObject.Find("Cube");
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//Debug.Log("cameraUPDate");
		//Debug.Log("inAir: " + bc.getInAir());
		
		
		//offset = transform.position - target.transform.position; // in air - don't follow rotation
		// update position relative to target, follow the player
		Vector3 desiredPosition = target.transform.position + offset;
   		Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping); // move smoothly
   	  	transform.position = position; // update position
		transform.LookAt(target.transform.position); // look at the player
		

		
		// An aiming feature would require the player to hold a mouse or controller button to aim, much like aiming
		// in Ocarina of Time
	}
	
	//There could be a "collider" function that prevents the camera from going under the terrain
	
}
