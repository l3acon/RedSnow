using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RocketLauncherController : WeaponController {

	public Rigidbody RocketObject;
	public Vector3 RocketOffset;
	public Transform RocketTransform;
	//public Vector3 RocketRotation;

	public float rocketForce = 2f;
	public List<Rigidbody> Rockets;

	void Start()	//instanciate our weapon and make it our child
	{
		Rockets = new List<Rigidbody>();
		GameObject clone = Instantiate(WeaponObject, OurPlayer.position + weaponOffset, Quaternion.identity) as GameObject;
		clone.transform.parent = OurPlayer.transform;

		nextAttack = attackRate;
		Setup();
	}


	override protected void Attack()
	{
		Rockets.Add(Instantiate(RocketObject, OurPlayer.position + RocketOffset, Quaternion.identity) as Rigidbody);
		Debug.Log("shooting");
	}

	void FixedUpdate()
	{
		AttackCheck();

		foreach (Rigidbody rocket in Rockets)
		{
			rocket.AddForce(rocketForce * rocket.rotation.eulerAngles);
		}

	}
}
