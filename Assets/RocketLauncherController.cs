using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RocketLauncherController : WeaponController {

	public Rigidbody RocketObject;
	public Vector3 RocketOffset;
	public Transform RocketTransform;

	public float rocketForce;
	public List<Rigidbody> Rockets;
	FollowCamera followCamera;
	public GameObject obj;

	void Start()	//instanciate our weapon and make it our child
	{
		followCamera = obj.GetComponent<FollowCamera>();
		
		Rockets = new List<Rigidbody>();
		GameObject myRocketLauncher = Instantiate(WeaponObject, OurPlayer.position + weaponOffset, Quaternion.identity) as GameObject;
		myRocketLauncher.transform.parent = OurPlayer.transform;

		nextAttack = attackRate;
		//Setup();
	}

	override protected void Attack()
	{
		Rigidbody tmp = Instantiate(RocketObject, OurPlayer.transform.position + RocketOffset, RocketObject.rotation) as Rigidbody;
		Debug.Log("aimerVector: " + followCamera.aimerVector);
		tmp.AddForce(rocketForce * followCamera.aimerVector);
		//tmp.AddForce(rocketForce * (OurPlayer.transform.forward));
		Rockets.Add(tmp);
	}

	void FixedUpdate()
	{
		//Debug.Log(RocketTransform.forward);
		AttackCheck();
		/*
		foreach (Rigidbody rocket in Rockets)
		{
			rocket.AddForce(rocketForce * rocket.rotation.eulerAngles);
		}
		*/
	}
}