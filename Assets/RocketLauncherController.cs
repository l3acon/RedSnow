using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RocketLauncherController : WeaponController {

	public Rigidbody RocketObject;
	public Vector3 RocketOffset;
	//public Vector3 RocketRotation;

	private List<Transform> RocketTransforms;


	void Attack()
	{
		Instantiate(RocketObject, RocketObject.position + RocketOffset, Quaternion.identity);
		RocketObject.AddForce(OurPlayer.transform.rotation.eulerAngles);

	}

	void FixedUpdate()
	{
		Debug.Log("fire!");

		if(iAttack && (Time.time < nextAttack) )
		{
			nextAttack = Time.time + attackRate;
			Attack();
		}		
	}
}
