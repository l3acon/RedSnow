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
	

	BoxController bc;
	private Vector3 lastUp;// save the player's position.up vector as soon as they jump
	private Vector3 lastForward;// save the player's position.forward vector as soon as they jump
	private Vector3 aimerRight; // save the player's position.right vector as soon as the mousaim button is clicked
	private Vector3 aimerForward;// save the player's position.forward vector as soon as the mousaim button is clicked
	private bool inAirCurrentState = false;
	private float downRange = 2;
	private Vector3 aimer; // the aiming point, we move it around, then call "lookAt();
	public Vector3 aimerVector; // the vector from camera to "aimer" 
	private float aimerXOffset;
	private float aimerYOffset;
	//private Vector3 lastAimer;
	// Use this for initialization
	void Start () 
	{
		transform.position = new Vector3(target.transform.position.x,target.transform.position.y + height, target.transform.position.z - distance);
		offset = transform.position - target.transform.position; // in air - don't follow rotation
		bc = target.GetComponent<BoxController>();
		aimer = target.position;
		aimerXOffset = 0;
		aimerYOffset = 0;
		//lastAimer = target.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		getInput();
		MoveCamera();
	
		// TODO Camera should not go under the terrain when going down a deep slope
		//There could be a "collider" function that prevents the camera from going under the terrain
	}
	

	
	/* MoveCamera()
	 * All of the basic moving camera functionality belongs here.
	 */
	void MoveCamera()
	{
		inAirCurrentState = bc.inAir;

		
		// if on the ground update position based on current target vectors
		if(inAirCurrentState == false)
		{
			targetPosition = target.position + target.up*height - target.forward*distance;
			lastUp = target.up;
			lastForward = target.forward;
		}
		// otherwise, update position based on the data before we left the ground.
		else{
			targetPosition = target.position + lastUp*height - lastForward*distance;
		}
		//Debug.Log ("Forward: " + target.forward);
		
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime*damping);
		transform.LookAt(aimer);
		aimerVector = -((transform.position + new Vector3(0,1,0)) - aimer);
		//aimerVector.y = -aimerVector.y; // invert y axis
		
	}
	
	//Author: kurtdog
	void getInput()
	{
		//TODO: for gaming controller, aimer will look something like this...
		//aimer = target.position + (target.forward*verticalAxis+target.right*horizontalAxis);
		
		// if pressing Right Click
		if(Input.GetAxis("MouseAim") == 1)
		{
			float mouseX = Input.GetAxis("Mouse X");
			float mouseY = Input.GetAxis("Mouse Y");
			//Debug.Log ("mouseX: " + mouseX);
			//Debug.Log ("mouseY: " + mouseY);
			
	
			aimerXOffset += mouseX;
			aimerYOffset += mouseY;

			
		}
		else{
			
			// lerp back to the normal view, instead of instant. I kinda like the instant part though
			aimerXOffset = Mathf.Lerp(aimerXOffset,0,Time.deltaTime*damping);
			aimerYOffset = Mathf.Lerp(aimerYOffset,0,Time.deltaTime*damping);
			
			// instantly go back to your aim
			//aimerXOffset = 0;
			//aimerYOffset = 0;
			
			// if not aiming, update the aimers forward and right vector
			// this way as soon as we aim, the forward and right vectors will be based on the vectors just before we clicked the aim button
			aimerForward = target.forward;
			aimerRight = target.right;
			
		}
		
		//aimer = target.position + (target.forward*aimerYOffset + target.right*aimerXOffset);
		aimer = target.position + (aimerForward*aimerYOffset + aimerRight*aimerXOffset);
		
	}
	
	
}
