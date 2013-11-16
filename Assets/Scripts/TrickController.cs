using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class TrickController : MonoBehaviour
{
	public GameObject player;
	
	Vector3 startPostition;
	Quaternion startingRotation;
	float deltaHeight;
	float deltaDistance;
	float airTime;
	float avgAirTime;
	float flipRotation;
	int flipCount;
	float spinRotation;
	int spinCount;
	List<float> airTimes = new List<float>();

	// GameObject theCube Kurt's code
	public BoxController bc;	// POINT THIS TO THE PLAYER
	AIcontroller aic;
	bool inAirLastState; // the last state of inAir
	bool inAirCurrentState; // the current state of inAir
	bool performingTrick;
	int points;
	float lastFlipRotation; // the flip rotation angle at the last state
	float lastSpinRotation; // the flip rotation angle at the last state
	Vector3 lastSpinAngle;
	
	void Start()
	{
		//theCube = GameObject.Find("Cube"); Kurt's code
		//aic = player.GetComponent<AIcontroller>();
		
		//startPostition = bc.transform.position;
		performingTrick = false;
		inAirLastState = false;
		inAirCurrentState = false;
		deltaHeight = 0;
		deltaDistance =0;
		airTime = 0;
		flipRotation = 0;
		flipCount = 0;
		spinRotation = 0;
		spinCount = 0;
		startPostition = player.transform.position;
		startingRotation = player.transform.rotation;
	
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
			avgAirTime = manageAirTime(airTime);
			points += tallyPoints(deltaHeight, deltaDistance, getAvgAirTime(), airTime, flipCount, spinCount);
			/*
			Debug.Log("Height: " + deltaHeight);
			Debug.Log("Distance: " + deltaDistance);
			Debug.Log("flips: " + flipCount);
			Debug.Log("spins: " + spinCount);
			Debug.Log("POINTS: " + points);
			*/
			Debug.Log("airTime: " + airTime);
			Debug.Log("avgAirTime: " + getAvgAirTime());
			
			
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
			
			// keep track of air time, and average air time.
			if(bc.airTime > airTime)
			{
				airTime = bc.airTime;
			}
			
			// keep track of flip rotation and flips completed

			//Debug.Log("xRot: " + deltaRotation(bc.transform.rotation.eulerAngles.x, lastFlipRotation));
			flipRotation += deltaFlipRotation(bc.transform.rotation.eulerAngles.x, lastFlipRotation);
			spinRotation += deltaSpinRotation(bc.transform.rotation.eulerAngles.x,bc.transform.rotation.eulerAngles.y, lastSpinRotation);
			
			//spinRotation += Vector3.Angle(,bc.transform.forward);
			
			flipCount = Mathf.RoundToInt(flipRotation/360);
			spinCount = Mathf.RoundToInt(spinRotation/360);
			
			
			//Debug.Log("angles: " + bc.transform.eulerAngles);
			

			//Debug.Log ("flipRot: "+ flipRotation);
			//Debug.Log ("spinRot: "+ spinRotation);
			

		}
		
		inAirLastState = inAirCurrentState;
		lastFlipRotation = bc.transform.localRotation.eulerAngles.x; // the angle of rotation around x at the last game state
		lastSpinRotation = bc.transform.localRotation.eulerAngles.y; // the angle of rotation around y at the last game state
		lastSpinAngle = bc.transform.forward; // get teh forward vector
	}
	
	public float manageAirTime(float aTime)
	{
		float threshold = 1f; // the airTIme has to be greater than this to count
		int maxListSize = 3; // 25
		
		// if this air time was greater than the threshold
		if(aTime > threshold)
		{
			if(airTimes.Count < maxListSize)
			{
				airTimes.Add(aTime);	
			}
			else
			{
				Debug.Log("Size: " + airTimes.Count + "removing: " + airTimes.Min());
				airTimes.Remove(airTimes.Min()); // remove the smallest item
				Debug.Log("Size: " + airTimes.Count);
				airTimes.Add(aTime); // add in this one
			}
		}
		
		float averageAirTime = (float)airTimes.Sum()/airTimes.Count;
		//Debug.Log("airTimes: " + airTimes + "\navg: " + averageAirTime);
		return averageAirTime;
	}
	
	public int tallyPoints(float height, float distance, float avgAirTime, float airTime, int flipCount, int spinCount)
	{
		int puntos = 0;
		float heightWeight = .2f;
		float distanceWeight = .2f;
		float airTimeWeight = .2f;
		float flipCountWeight = 100f;
		float spinCountWeight = 100f;
		bool trickWasPerformed = false;
		if(spinCount > 0 || flipCount > 0) // or a grab was performed
		{
			trickWasPerformed = true;
		}
		// we want a basic point ++ based on airTime, height, and distance IF  the player performed at least 1 trick.
		if(trickWasPerformed == true)
		{	
			puntos += (int)(height*heightWeight + distance*distanceWeight + airTime*airTimeWeight + flipCount*flipCountWeight + spinCount*spinCountWeight);
		
			// add extra points based on completing a lot of tricks, in a short amount of air time
			float timeWeight = avgAirTime/airTime; // the weighting is based off of average air time. Doing more tricks, with less airTime, = more points
			puntos += (int)(timeWeight * (flipCount + spinCount));
			
			
		}
		
		
		// add a constant point count for time spent during a grab.
		
		return puntos;
	}
	
	
	// get the difference in rotation, accounting for when a player roates from an angle of 360 to 0 or -360 to 0
	public float deltaFlipRotation(float start, float end)
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
	
		// get the difference in rotation, accounting for when a player roates from an angle of 360 to 0 or -360 to 0
	public float deltaSpinRotation(float startX, float startY, float end)
	{
		// get abs of each
		float sX = Mathf.Abs(startX);
		float s = Mathf.Abs(startY); 
		float e = Mathf.Abs(end);
		
		//Debug.Log("start: " + sX + " , " + s + " , " + e);
		
		
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
		
		if(delta >= 170)
		{
			delta -= 180;	
		}
		
		//Debug.Log("end: " + sX + " , " + s + " , " + e);
		//Debug.Log ("Delta: " + delta);
		return delta;
	}
	
	public float getAvgAirTime()
	{
		return avgAirTime;	
	}
	
	//@author: kurt
	void OnGUI() {
		
		GUI.color = Color.red; // set the color
		string score = "SCORE: " + points;
		string spins = "Spins: " + spinCount;
		string flips = "Flips: " + flipCount;
        GUI.Label(new Rect(10, 30, 100, 20),score); // draw text
		GUI.Label (new Rect(10, 50, 100, 20), spins);
		GUI.Label (new Rect(10, 70, 100, 20), flips);
		//GUI.Label(new Rect(825, 30, 29, 37), playerPic ); // draw a texture
		//Debug.Log("screenPoint: " + camera.ViewportToScreenPoint(this.transform.position));
    }
	
}

