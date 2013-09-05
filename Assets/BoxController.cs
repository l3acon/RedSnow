using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Camera))]
public class BoxController : MonoBehaviour {
	

	
	/**
	 * TODO
	 * 
	 * Trick Track
	 * 1) Fix camera so it only moves when player is on the ground. (we want the player to be able to spin when in the air)
	 * 2) Add force when player is on ground and leans forward
	 * 3) Edit terrain to have a slight slope all the way down, use the height variable when creating it.
	 * 4) Get controller working for input, xbox or playstation
	 * 5) start working on point system
	 * 
	 * Weapons Track
	 * 1) Get basic projectile working
	 * 2) Collision detection with game objects, create other boxes, try and shoot them.
	 * 3) import weapon models
	 * 4) point system for kills
	 * 
	 *
	 * 
	 **/
	public float defaultForce = 10f; // default 
	public float speed = 10f;
	public float jumpForce = 200f;
	public float rotationSpeed= 10f;
	public bool inAir = true;
	
	public Quaternion defaultOrientation;
	
	// Use this for initialization
	void Start () {
	 	defaultOrientation = rigidbody.transform.rotation;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		inAir = true;
		//camera.eventMask = 0;
		// add a constant forward force		
		Vector3 forwardForce = this.transform.forward*speed;
		rigidbody.transform.position += forwardForce * Time.deltaTime; 
	
		Rotate(); // rotate the character based on player input
	
	}
	
	/**
	  * Here we put all the methods and fuctionality that we only want to
	  * be executed when collisions occur.
	  **/
	void OnCollisionStay(Collision collision)
	{
		float verticalAxis = Input.GetAxis("Vertical");
		inAir = false;
		//camera.eventMask = 1;
		// if it was a terrain collision, a.k.a. you're on the ground
		if(collision.collider.name == "Terrain")
		{
			//Debug.Log ("Colliding");
			// Jump
			if(Input.GetAxis("Jump") > .001)// if pressing jump key
			{
				Debug.Log("Jumping");
				rigidbody.AddForce(new Vector3(0,jumpForce,0));
			}
			
			// reset button
			if(Input.GetKey(KeyCode.R)) //we should update this to refer to a global reset button (not just for PC)
			{
				rigidbody.transform.rotation = defaultOrientation;
			}
			
			// speed up and slow down
			// adds a force in the x direction, "verticalAxis" ranges from -1 to 1 depending on player joystick input.
			rigidbody.AddForce(new Vector3(speed*verticalAxis,0,0));
		}
	}
	
	public void Rotate()
	{
		/**
		 * rotation
		 * axis is float, 1 to -1. This way we can have multiple inputs.
		 * the axis is referenced from the point of the joystick
		 **/
		float verticalAxis = Input.GetAxis("Vertical");
		float horizontalAxis = Input.GetAxis("Horizontal");
		//Debug.Log("verticalAxis: " + verticalAxis);
		//Debug.Log("horizontalAxis: " + horizontalAxis);

		rigidbody.transform.Rotate(0,rotationSpeed*horizontalAxis,0); // rotate on the horizontal axis, left and right 
		rigidbody.transform.Rotate(rotationSpeed*verticalAxis,0,0); // rotate on the horizontal axis, up and down
	
		
	}
	
}
