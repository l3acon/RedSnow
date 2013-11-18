using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(Camera))]
public class AIcontroller : MonoBehaviour {
	

	
	/**
	 * STATE MACHINE
	 * 
	 *-movement, waypoints(arrows) will be placed across the map, indicating direction vectors
	 *	get the closest waypoint
	 *		move in that direction
	 *	check for trick zones and other players
	 *		move accordingly, to set yourself up for tricks, or to engage players
	 *	
	 *-combat
	 *	if any other player is within a certain distance, shoot at them with a variable rate and accuracy
	 *
	 *-performTricks, we'll use trick zones, rectangles placed before the ramps and rails to indicate a trick should be performed. They'll also have a uniform direction vector
	 *	ramp trick zone
	 *		once you've entered a ready state (you've moved into place), load a random trick.
	 *	rail trick zone
	 *
	 * 
	 **/
	//public float defaultForce = 50f; // default 
	public List<GameObject> waypoints;
	public List<GameObject> targets;
	public int range; // if targets are within this range, shoot them
	GameObject waypoint; // current waypoint
	GameObject target; // current target

	

	private float maxSpeed; // max speed value
	private float controlledSpeed; // speed value, portion of maxSpeed depending on joystick input.
	private float currentSpeed; // keep track of the speed each loop
	private float jumpForce;
	private float turnSpeed; // speed at which you turn on the ground
	private float crouchedTurnSpeed; // rotate slower when crouched
	private bool grinding;
	private Vector3 railVector;
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
	
	private float xRot = 0;
	private float lastFlipRotation;
	// Use this for initialization
	void Start () {
		
		// add in all the waypoints on the map

		
	 	defaultOrientation = rigidbody.transform.rotation;
		lastFlipRotation = defaultOrientation.eulerAngles.x;
		Debug.Log ("DefaultX: " + defaultOrientation.x);
		// set default values
		maxSpeed = 10f;
		controlledSpeed = 10;
		jumpForce = 500f;
		turnSpeed = 5f;
		crouchedTurnSpeed = 0;//.5f;
		inAir = false;
		grinding = false;
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
		//Debug.Log ("Spin: " + loadedSpinTorque);
		//Debug.Log ("Flip: " + loadedFlipTorque);
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
	  * 
	  * 
	  * TODO; change this to a state machine? grining, inair, on ground, grabbing, other shit...?
	  * */
	void Update()
	{
	 
		
		
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
		
	
		combat();
	}

	/*
	 * FixedUpdate, for physics or time related methods and changes.
	 * author: MrBacon
	 * */
	void FixedUpdate () 
	{

		//camera.eventMask = 0;
		// add a forward force, based on momentum, and controller input (leaning forward or back)
		Vector3 forwardForce ;
		if(grinding == true)
		{
			// move in the direction of the rails grinding axis.
			Vector3 antiGravity = 10*this.transform.up; // turn down gravity a bit to make it easier to stay on the rail
			float railSpeed = controlledSpeed + 10; // set the speed at which you travel along the rail
			forwardForce = (railVector)*railSpeed + antiGravity;
			//Debug.Log("RailVector: " + railVector);
			//Debug.Log("ForceApplied: " + forwardForce);
		}
		else{

			forwardForce = (this.transform.forward)*controlledSpeed;
		}
		 
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
		
		if(collision.collider.name == "Rail")
		{
			grinding = true;
			GameObject rail = collision.collider.gameObject;
			railVector = rail.transform.up;
		}
	}
	void OnCollisionExit(Collision collision)
	{
			//Debug.Log("no longer colliding");
		if(collision.collider.name == "Terrain")
		{
			inAir = true;	
		}
		if(collision.collider.name == "Rail")
		{
			grinding = false;
		}
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
	 * get the closest waypoint to the player
	 * author: kurtdog
	 * */
	public GameObject getClosestWaypoint()
	{
		GameObject startingWaypoint = waypoints[0];
		GameObject currentWaypoint = startingWaypoint; // initialize currentWaypoint as the first waypoing
		float minDistance = Vector3.Distance(startingWaypoint.gameObject.transform.position,transform.position); // initialize minDistance as the distance between the player and the first waypoint
		
		
		// for all the waypoints
		foreach(GameObject wp in waypoints)
		{
			// get the distance to that waypoint
			float distance = Vector3.Distance(wp.transform.position,transform.position);
			//if the distance is less than minDistance, update minDistance and currentWaypoint
			if(distance < minDistance)
			{
				minDistance = distance;
				currentWaypoint = wp;
			}
		}
		
		return currentWaypoint;
	}
	
		/*
	 * get the closets target, if they're within range, shoot them.
	 * 
	 * author: kurtdog
	 * */
	private void combat()
	{
		GameObject startingTarget = targets[0];
		GameObject currentTarget = startingTarget; // initialize currentWaypoint as the first waypoing
		float minDistance = Vector3.Distance(startingTarget.gameObject.transform.position,transform.position); // initialize minDistance as the distance between the player and the first waypoint
		
		
		// for all the waypoints
		foreach(GameObject tg in targets)
		{
			// get the distance to that waypoint
			float distance = Vector3.Distance(tg.transform.position,transform.position);
			//if the distance is less than minDistance, update minDistance and currentWaypoint
			if(distance < minDistance)
			{
				minDistance = distance;
				currentTarget = tg;
			}
		}
		
		if(minDistance < range)
		{
			//attack();
//			wc.BotAttackCheck(currentTarget.transform.position);
			
		}
	}
	
	/*
	 * 
	 * 
	 * */
	 private void attack()
	{
		//TODO: add in accuracy, attack rate, etc. based on difficulty
		
		//RocketLauncherController
		
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
		waypoint = getClosestWaypoint();
		//Debug.Log("waypoint: " + waypoint.gameObject.name);
		float verticalAxis = 1;//Input.GetAxis("Vertical");
		//float horizontalAxis = //Input.GetAxis("Horizontal");
		
		// when on the ground, update these vars, they're sent to air input to calculate torque for spins and flips
		//horizontalAxisAtJump = horizontalAxis;
		//verticalAxisAtJump = verticalAxis;
		
		controlledSpeed = maxSpeed*verticalAxis;
		
		// if crouched
		if(Input.GetButton("Jump"))
		{
			// no transform.rotate
			loadTorque(); // start loading torque
			controlledSpeed = currentSpeed + 5; // the speed before we crouched + a small boost will equal our crouch speed
			//rigidbody.transform.Rotate(0,crouchedTurnSpeed*horizontalAxis,0); // rotate on the horizontal axis, left and right 
			
		}
		// otherwise
		else{
			// move normally
			//rotate in the direciton of the waypoint
			//Vector3 waypointForward = new Vector3(0,waypoint.transform.eulerAngles.y,0);
			Vector3 AIForward = new Vector3(0,this.transform.eulerAngles.y,0);
			
			
			float angle = waypoint.transform.eulerAngles.y - this.transform.eulerAngles.y;
			//Debug.Log("Angle: " + angle);
			Vector3 rotation = Vector3.Lerp(new Vector3(0,0,0),new Vector3(0,angle,0),1f*Time.deltaTime);
			this.transform.Rotate(rotation); 
			currentSpeed = controlledSpeed; // when on the ground, update and keep track of "currentSpeed"
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
		//Debug.Log ("UP:" + this.transform.up);
		//Debug.Log ("difference: "+ (this.transform.localRotation.x - defaultOrientation.x));
		
		xRot += TrickController.deltaFlipRotation(this.transform.rotation.eulerAngles.x, lastFlipRotation);
		Debug.Log ("Xrot: " + xRot);
		lastFlipRotation = this.transform.rotation.eulerAngles.x;
		if(xRot > 180) //we should update this to refer to a global reset button (not just for PC)
		{
			Debug.Log("RESET");
			//rigidbody.transform.rotation = new Vector3(rigidbody.transform.rotation.x,defaultOrientation.y,rigidbody.transform.rotation.z);
			rigidbody.transform.rotation = defaultOrientation;
			xRot = 0;
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


	
