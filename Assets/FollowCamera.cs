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

[RequireComponent(typeof(Rigidbody))]

public class FollowCamera : MonoBehaviour {
	

	public float damping = 1.0f;	//Damping factor: How fast the camera adjusts
	public Transform target;		//Target: Who is the camera following?
	public float height = 5.0f;		//Height: How high above the camera is from the target?
	public float distance = 7.0f;	//Distance: How far away is the camera from the target?
	
	private Vector3 offset;			//Offset: Initialization offset position
	private Vector3 targetPosition;
	
	// Use this for initialization
	void Start () 
	{
		transform.position = new Vector3(target.transform.position.x,target.transform.position.y + height, target.transform.position.z - distance);
		offset = transform.position - target.transform.position; // in air - don't follow rotation
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		MoveCamera();
		
		// An aiming feature would require the player to hold a mouse or controller button to aim, much like aiming
		// in Ocarina of Time
		
		// TODO Camera should not go under the terrain when going down a deep slope
		//There could be a "collider" function that prevents the camera from going under the terrain
	}
	
	

	
	/* MoveCamera()
	 * All of the basic moving camera functionality belongs here.
	 */
	void MoveCamera()
	{
		targetPosition = target.position + target.up*height - target.forward*distance;
		
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime*damping);
		transform.LookAt(target);
	}
	
}
