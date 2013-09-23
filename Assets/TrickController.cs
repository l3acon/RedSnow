using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TrickController : MonoBehaviour
{
	public GameObject theCube;
	
	Vector3 startPostition;
	Quaternion startingRotation;
	float deltaHeight;
	float deltaDistance;
	float airTime;
	float flipRotation;
	float flipCount;
	float spinRotation;
	float spinCount;
	

	// GameObject theCube Kurt's code
	BoxController bc;
	bool inAirLastState; // the last state of inAir
	bool inAirCurrentState; // the current state of inAir
	bool performingTrick;
	int points;
	float lastFlipRotation; // the flip rotation angle at the last state
	float lastSpinRotation; // the flip rotation angle at the last state
	
	void Start()
	{
		//theCube = GameObject.Find("Cube"); Kurt's code
		bc = theCube.GetComponent<BoxController>();
			
		//startPostition = bc.transform.position;
		performingTrick = false;
		inAirLastState = false;
		inAirCurrentState = false;
		points = 0;
		deltaHeight = 0;
		deltaDistance =0;
		airTime = 0;
		flipRotation = 0;
		flipCount = 0;
		spinRotation = 0;
		spinCount = 0;
		startPostition = bc.transform.position;
		startingRotation = bc.transform.rotation;
	
		//startHeight = bc.transform.position.y;
	}
	
	void Update()
	{
		inAirCurrentState = bc.inAir;
		//Debug.Log("inAir: " + bc.inAir);
		//Debug.Log("current: " + inAirCurrentState);
		//Debug.Log("last: " + inAirLastState);
		// if we were on the ground, but are now in the air
		if(inAirLastState == false && inAirCurrentState == true)
		{
			performingTrick = true; // we've started a new trick.
		}
		// if we were in the air, but are now on the ground
		if(inAirLastState == true && inAirCurrentState == false)
		{
			Debug.Log("Height: " + deltaHeight);
			Debug.Log("Distance: " + deltaDistance);
			Debug.Log("airTime: " + airTime);
			Debug.Log("flips: " + flipCount);

			performingTrick = false; // we're done performing a trick.	
			this.Start(); // reset all variables
		}
		
		// main functionality for the trick
		if(performingTrick)
		{

			// keep track of max height
			float currentHeight = Mathf.Abs(bc.transform.position.y - startPostition.y);
			if(currentHeight > deltaHeight)
			{
				deltaHeight = currentHeight;
			}
			
			// keep track of max distance traveled in air
			float currentDistance = Vector3.Magnitude(bc.transform.position - startPostition);
			if(currentDistance > deltaDistance)
			{
				deltaDistance = currentDistance;
			}
			
			// keep track of air time
			if(bc.airTime > airTime)
			{
				airTime = bc.airTime;
			}
			
			// keep track of flip rotation and flips completed

			//Debug.Log("xRot: " + deltaRotation(bc.transform.rotation.eulerAngles.x, lastFlipRotation));
			flipRotation += deltaRotation(bc.transform.rotation.eulerAngles.x, lastFlipRotation);
			spinRotation += deltaRotation(bc.transform.rotation.eulerAngles.y, lastSpinRotation);
			
			flipCount = Mathf.RoundToInt(flipRotation/360);
			spinCount = Mathf.RoundToInt(spinRotation/360);
			
			
			//Debug.Log("flips: " + flipCount);
			//Debug.Log("spins: " + spinCount);
			//Debug.Log ("flipRot: "+ flipRotation);
			//Debug.Log ("spinRot: "+ spinRotation);
			

		}
		
		inAirLastState = inAirCurrentState;
		lastFlipRotation = bc.transform.localRotation.eulerAngles.x; // the angle of rotation around x at the last game state
		lastSpinRotation = bc.transform.localRotation.eulerAngles.y; // the angle of rotation around y at the last game state
	}
	
	// get the difference in rotation, accounting for when a player roates from an angle of 360 to 0 or -360 to 0
	public float deltaRotation(float start, float end)
	{
		// get abs of each
		float s = Mathf.Abs(start); 
		float e = Mathf.Abs(end);
		
		//if s is almost at 360 and e is below 100
		if((s > 300 && e < 100 ))
		{
			s = 360-s;
		}
		// vise versa
		if((s < 100 && e > 300))
		{
			e = 360 -e;
		}
		
		
		float delta = Mathf.Abs(s-e);
		return delta;
	}
	
}

