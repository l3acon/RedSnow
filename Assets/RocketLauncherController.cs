using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RocketLauncherController : WeaponController {

	public Rigidbody RocketObject;
	public Vector3 RocketOffset;
	public Transform RocketTransform;
	//public Vector3 RocketRotation;

	public List<Rigidbody> Rockets;

	void Start()
	{
		Rockets = new List<Rigidbody>();
	}

	void Attack()
	{
		Debug.Log("fire!");
		Rockets.Add(Instantiate(RocketObject, RocketObject.position + RocketOffset, Quaternion.identity) as Rigidbody);
		
	}

	void FixedUpdate()
	{
		if(iAttack && (Time.time < nextAttack) )
		{
			nextAttack = Time.time + attackRate;
			Attack();
		}		
	}
}
