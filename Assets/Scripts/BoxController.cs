using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(Camera))]
public class BoxController : MonoBehaviour {
	

	
	/**
	 * TODO
	 * 
	 * Trick Track
	 * 
	 * Weapons Track

	 * 
	 *
	 * 
	 **/
	//public float defaultForce = 50f; // default 
	

	private float maxSpeed; // max speed value
	private float controlledSpeed; // speed value, portion of maxSpeed depending on joystick input.
	private float currentSpeed; // keep track of the speed each loop
	private float jumpForce;
	private float turnSpeed; // speed at which you turn on the ground
	private float crouchedTurnSpeed; // rotate slower when crouched
	private bool grinding;
	private Vector3 railVector;
	private BetterList<GameObject> railList;
	private RailController railCont;
	public GameObject[] allRails;
	
	public bool inAir;
	public float airTime;
	public int points;

	
	public Quaternion defaultOrientation;
	
	private Vector3 _lastVelocity;
	private float horizontalAxisAtJump = 0; // the value of the joystick right before the jump
	private float verticalAxisAtJump = 0;// the value of the joystick right before the jump
	private bool initialJump; // stating if we just started the jump, or if we've been in air for a bit
	private float loadedSpinTorque; // the torque the player will start spinning at once they jump
	private float loadedFlipTorque; // the torque the player will start flipping at once they jump
	private float airControl; // how well the player can control themselves in the air
	private float loadXTime = 0; // keeps track of how long the player has been crouched
	private float loadYTime = 0;
	
	
	void Awake()
	{
//		int i = 0;
//		railList.Add(GameObject.Find("rail0"));
//		allRails = railList.ToArray();
	}
	
	
	// Use this for initialization
	void Start () {
	 	defaultOrientation = rigidbody.transform.rotation;
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
	  * 
	  * 
	  * TODO; change this to a state machine? grining, inair, on ground, grabbing, other shit...?
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
		
	
		//Debug.Log("Grinding: " + grinding);
	}

	/*
	 * FixedUpdate, for physics or time related methods and changes.
	 * author: MrBacon
	 * */
	void FixedUpdate () 
	{

		//camera.eventMask = 0;
		// add a forward force, based on momentum, and controller input (leaning forward or back)
		Vector3 forwardForce;
		
		_lastVelocity = gameObject.rigidbody.velocity;
		
		if(grinding == true)
		{
			// move in the direction of the rails grinding axis.
			//Vector3 antiGravity = new Vector3(0,9.81f,0); // turn down gravity a bit to make it easier to stay on the rail
			//float railSpeed = controlledSpeed + 10; // set the speed at which you travel along the rail
			//forwardForce = new Vector3(0,0,0);
			//forwardForce = (_lastVelocity)*railSpeed + antiGravity;
			//Debug.Log("RailVector: " + forwardForce);
			//Debug.Log("ForceApplied: " + forwardForce);
			rigidbody.velocity = Vector3.Project(_lastVelocity, new Vector3(0,0,1));
		}
		else{
			forwardForce = (this.transform.forward)*controlledSpeed;
			rigidbody.AddForce(forwardForce);
		}
		 
		//rigidbody.transform.position += forwardForce * Time.deltaTime; 

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
		
		if(collision.collider.tag == "Rail")
		{
			grinding = true;
			inAir = false;
			GameObject rail = collision.collider.gameObject;
			//railVector = rail.transform.up;
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
			inAir = true;	
		}
		grinding = false;
	}
	void OnCollisionEnter(Collision collision)
	{
		ContactPoint contact = collision.contacts[0];
		if(collision.collider.tag == "Rail")
		{
			grinding = true;
			if (contact.normal.y <= -0.6)
			{
				gameObject.rigidbody.isKinematic = true;
				transform.position = contact.point - _lastVelocity.normalized * 30;
			}
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
			rigidbody.transform.Rotate(0,turnSpeed*horizontalAxis,0); // rotate on the horizontal axis, left and right 
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
		if(Input.GetKey(KeyCode.R)) //we should update this to refer to a global reset button (not just for PC)
		{
			//rigidbody.transform.rotation = new Vector3(rigidbody.transform.rotation.x,defaultOrientation.y,rigidbody.transform.rotation.z);
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


	
