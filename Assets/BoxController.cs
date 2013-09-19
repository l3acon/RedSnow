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
	//public float defaultForce = 50f; // default 
	
	public float speed;
	public float maxSpeed;
	public float jumpForce;
	public float rotationSpeed;
	public static bool inAir;
	public float airTime;
	
	public Quaternion defaultOrientation;
	
	// Use this for initialization
	void Start () {
	 	defaultOrientation = rigidbody.transform.rotation;
		// set default values
		speed = 10f;
		maxSpeed = 15f;
		jumpForce = 200f;
		rotationSpeed = 5f;
		inAir = false;
		airTime = 0;
	}
	

	/*
	  * Update is called once per frame, main script functionality
	  * author: kurtdog
	  * */
	void Update()
	{
		// methods in this section will only be called if you're on the ground
		if(inAir == false)
		{
			GroundInput(); // get player controls when on the ground
		}
		// methods to be called when in the air
		// only call them if airTime > threshold, i.e. if you've been in the air greater than "Threshold" time. 
			//This gets rid of the inAir variable being true when you hit little bumps.
		else if (airTime > .1)
		{	
			AirInput(); // get player controls when in the air
		}
		
		
		inAir = true; // reset collision variable.
	}

	/*
	 * FixedUpdate, for physics or time related methods and changes.
	 * author: MrBacon
	 * */
	void FixedUpdate () 
	{
		//camera.eventMask = 0;
		// add a constant forward force
		Vector3 forwardForce = (this.transform.forward)*speed;
		rigidbody.transform.position += forwardForce * Time.deltaTime; 
		
		airTime += Time.deltaTime; // count the amount of time we've been in the air
		
		//Debug.Log ("Speed: "+ speed);
		//Debug.Log("transoform.forward= " + this.transform.forward);
		//Debug.Log("ForwardForce= " + forwardForce);
		
		// check for inAir = true,false
		//RaycastHit hitInfo;
		//bool temp = rigidbody.SweepTest(this.transform.forward,out hitInfo);
		//inAir = temp;
		
	}
	
	/**
	  * Here we put all the methods and fuctionality that we only want to
	  * be executed when collisions occur.
	  * author: kurtdog
	  **/
	void OnCollisionStay(Collision collision)
	{
//		float verticalAxis = Input.GetAxis("Vertical");
		
		//camera.eventMask = 1;
		// if it was a terrain collision, a.k.a. you're on the ground
		if(collision.collider.name == "Terrain")
		{
			inAir = false;
		}
	}
	
	
	/**
	  * get all input from player related to controlling behavior in the air
	  * author: kurtdog
	  * */
	public void AirInput()
	{
		Debug.Log("AirInput");
		Debug.Log ("Time: " + airTime);
		/**
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
	
	
	/**
  * get all input from player related to controlling behavior on the ground
  * author: kurtdog
  * */
	public void GroundInput()
	{
		Debug.Log("GroundInput");
		airTime = 0; // we're on the ground so reset the airTime timer;
		/**
		 * axis is float, 1 to -1. This way we can have multiple inputs.
		 * the axis is referenced from the point of the joystick
		 **/
		float verticalAxis = Input.GetAxis("Vertical");
		float horizontalAxis = Input.GetAxis("Horizontal");
		//Debug.Log("verticalAxis: " + verticalAxis);
		//Debug.Log("horizontalAxis: " + horizontalAxis);

		rigidbody.transform.Rotate(0,rotationSpeed*horizontalAxis,0); // rotate on the horizontal axis, left and right 
		//rigidbody.transform.Rotate(rotationSpeed*verticalAxis,0,0); // rotate on the vertical axis, up and down
	
		
			//Debug.Log ("Colliding");
			// Jump
			if(Input.GetAxis("Jump") > .001)// if pressing jump key
			{
				//Debug.Log("Jumping");
				rigidbody.AddForce(new Vector3(0,jumpForce,0));
			}
			
			// reset button
			if(Input.GetKey(KeyCode.R)) //we should update this to refer to a global reset button (not just for PC)
			{
				rigidbody.transform.rotation = defaultOrientation;
			}
			
			// speed up and slow down
			// adds a force in the x direction, "verticalAxis" ranges from -1 to 1 depending on player joystick input.
			Vector3 controlledSpeed = (this.transform.forward)*verticalAxis;
			rigidbody.transform.position += controlledSpeed * Time.deltaTime;
			Debug.Log("controlledSpeed: " + controlledSpeed);
		
			//Vector3 forwardForce = (this.transform.forward)*speed;
			//rigidbody.transform.position += forwardForce * Time.deltaTime; 
		
	}
	
	public bool getInAir()
	{
		return inAir;
	}
		
	
}
