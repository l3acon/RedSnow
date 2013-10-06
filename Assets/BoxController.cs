using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(Camera))]
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
	

	public float maxSpeed;
	public float controlledSpeed; // speed value accounting for momentum and user input (leaning forward or back)
	public float jumpForce;
	public float turnSpeed; // speed at which you turn on the ground
	public float crouchedTurnSpeed; // rotate slower when crouched
	public bool inAir;
	public float airTime;
	public int points;
	
	public Quaternion defaultOrientation;
	
	
	private float horizontalAxisAtJump = 0; // the value of the joystick right before the jump
	private float verticalAxisAtJump = 0;// the value of the joystick right before the jump
	private bool initialJump; // stating if we just started the jump, or if we've been in air for a bit
	private float loadedSpinTorque; // the torque the player will start spinning at once they jump
	private float loadedFlipTorque; // the torque the player will start flipping at once they jump
	private float airControl; // how well the player can control themselves in the air
	private float loadXTime = 0; // keeps track of how long the player has been crouched
	private float loadYTime = 0;
	
	// Use this for initialization
	void Start () {
	 	defaultOrientation = rigidbody.transform.rotation;
		// set default values
		maxSpeed = 10f;
		controlledSpeed = 0;
		jumpForce = 500f;
		turnSpeed = 5f;
		crouchedTurnSpeed = .5f;
		inAir = false;
		airTime = 0;
		points = 0;
		loadedSpinTorque = 0;
		loadedSpinTorque = 0;
		airControl = 5;
	
		
		// 0 friction in the forward direction,
		// friction in all other directions.
		// THIS IS ALL FUCKY......................
		collider.material.frictionDirection2 = rigidbody.transform.forward;
        collider.material.dynamicFriction2 = 0;
        collider.material.dynamicFriction = .7f;//1;
		collider.material.bounciness = .2f;
		//collider.material.fr
	
	}
	
	private void loadedJump()
	{
		Debug.Log ("Spin: " + loadedSpinTorque);
		Debug.Log ("Flip: " + loadedFlipTorque);
		if(loadedSpinTorque > 0)
		{
			rigidbody.AddTorque(loadedSpinTorque*horizontalAxisAtJump*transform.up);
		}
		else{
			rigidbody.AddTorque(loadedSpinTorque*horizontalAxisAtJump*transform.up*-1);
		}
		if(loadedFlipTorque > 0)
		{
			rigidbody.AddTorque(loadedFlipTorque*verticalAxisAtJump*transform.right);
		}
		else{
			rigidbody.AddTorque(loadedFlipTorque*verticalAxisAtJump*transform.right*-1);
		}
		initialJump = false;
	}
	
	/*
	  * Update is called once per frame, main script functionality
	  * author: kurtdog
	  * */
	void Update()
	{
	 
		//Debug.Log("inAir = " +inAir);
		
		if(inAir == false)
		{
			GroundInput(); // get player controls when on the ground
			initialJump = true;
		}
		// methods to be called when in the air
		// only call them if airTime > threshold, i.e. if you've been in the air greater than "Threshold" time. 
			//This gets rid of the inAir variable being true when you hit little bumps.
		else if (airTime > .1)
		{	
			// when we first jump, apply an initial torque
			if(initialJump == true)
			{
				loadedJump();
			}
			// after that, when in the air, allow the player to add smaller torques
			AirInput(); // get player controls when in the air
		}
		
	
		
		 // control the camera
		
		//inAir = true; // reset collision variable.
	}

	/*
	 * FixedUpdate, for physics or time related methods and changes.
	 * author: MrBacon
	 * */
	void FixedUpdate () 
	{

		//camera.eventMask = 0;
		// add a forward force, based on momentum, and controller input (leaning forward or back)
		Vector3 forwardForce = (this.transform.forward)*controlledSpeed;
		//rigidbody.transform.position += forwardForce * Time.deltaTime; 
		rigidbody.AddForce(forwardForce);
		//Debug.Log("ForwardForce= " + forwardForce);
		
		
		airTime += Time.deltaTime; // count the amount of time we've been in the air
		
		//Debug.Log ("Speed: "+ speed);
		//Debug.Log("transoform.forward= " + this.transform.forward);

		
		// check for inAir = true,false
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
		//Debug.Log("Collision");
		if(collision.collider.name == "Terrain")
		{
			inAir = false;
		}
	}
	void OnCollisionExit(Collision collision)
	{
			//Debug.Log("no longer colliding");
			inAir = true;	
	}
	
	
	
	/**
	  * get all input from player related to controlling behavior in the air
	  * author: kurtdog
	  * */
	public void AirInput()
	{
		//Debug.Log("AirInput");
		//Debug.Log ("Time: " + airTime);
		/**
		 * axis is float, 1 to -1. This way we can have multiple inputs.
		 * the axis is referenced from the point of the joystick
		 **/
		float verticalAxis = Input.GetAxis("Vertical");
		float horizontalAxis = Input.GetAxis("Horizontal");
		//Debug.Log("verticalAxis: " + verticalAxis);
		//Debug.Log("horizontalAxis: " + horizontalAxis);

		//rigidbody.transform.Rotate(0,turnSpeed*horizontalAxis,0); // rotate on the horizontal axis, left and right 
		//rigidbody.transform.Rotate(turnSpeed*verticalAxis,0,0); // rotate on the horizontal axis, up and down
		
		 // add a torque, instead of just a constant rotation - more realistic
		rigidbody.AddTorque(airControl*horizontalAxis*transform.up);
		rigidbody.AddTorque(airControl*verticalAxis*transform.right);
		
	}
	
	
	/**
  * get all input from player related to controlling behavior on the ground
  * author: kurtdog
  * */
	public void GroundInput()
	{
		//Debug.Log("GroundInput");
		airTime = 0; // we're on the ground so reset the airTime timer;
		/**
		 * axis is float, 1 to -1. This way we can have multiple inputs.
		 * the axis is referenced from the point of the joystick
		 **/
		float verticalAxis = Input.GetAxis("Vertical");
		float horizontalAxis = Input.GetAxis("Horizontal");
		
		// when on the ground, update these vars, they're sent to air input to calculate torque for spins and flips
		horizontalAxisAtJump = horizontalAxis;
		verticalAxisAtJump = verticalAxis;
		
		
		// if crouched
		if(Input.GetButton("Jump"))
		{
			// rotate slowely
			loadTorque();
			rigidbody.transform.Rotate(0,crouchedTurnSpeed*horizontalAxis,0); // rotate on the horizontal axis, left and right 
			//Debug.Log("MOVE SLOWLY");
		}
		// otherwise
		else{
			// move normally
			rigidbody.transform.Rotate(0,turnSpeed*horizontalAxis,0); // rotate on the horizontal axis, left and right 
			controlledSpeed = maxSpeed*verticalAxis;
		}

		
		// if Key is Released
		if(Input.GetButtonUp("Jump"))// if pressing jump key
		{
			//Debug.Log("Jumping");
			rigidbody.AddForce(new Vector3(0,jumpForce,0));
			loadXTime = 0;
			loadYTime = 0;
		}
		
		// reset button
		if(Input.GetKey(KeyCode.R)) //we should update this to refer to a global reset button (not just for PC)
		{
			rigidbody.transform.rotation = defaultOrientation;
		}
		
		// speed up and slow down
		// adds a force in the x direction, "verticalAxis" ranges from -1 to 1 depending on player joystick input.
		//Vector3 controlledSpeed = (this.transform.forward)*verticalAxis;
		
	
		//Vector3 forwardForce = (this.transform.forward)*speed;
		//rigidbody.transform.position += forwardForce * Time.deltaTime; 
	
	}
	
	private void loadTorque()
	{
		
		loadXTime += Time.fixedDeltaTime*Input.GetAxis("Horizontal");
		loadYTime += Time.fixedDeltaTime*Input.GetAxis("Vertical");	
		//Debug.Log ("loadXTime: " + loadXTime);
		//Debug.Log ("loadYTime: " + loadYTime);
		
		loadedSpinTorque = loadXTime*100;
		loadedFlipTorque = loadYTime*100;
		
	}
	
	public bool getInAir()
	{
		return inAir;
	}
		
	
}


	
