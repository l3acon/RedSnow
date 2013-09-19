using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RocketLauncherController : MonoBehaviour {

	public Rigidbody RocketObject;
	public Vector3 RocketOffset;
	public Transform RocketTransform;
	//public Vector3 RocketRotation;

	public GameObject WeaponObject;
	public Rigidbody OurPlayer;
	public Vector3 weaponOffset;
	public float attackRate;

	public float rocketForce = 2f;
	protected float nextAttack = 3f;
	protected bool iAttack;
	public List<Rigidbody> Rockets;

	void Start()	//instanciate our weapon and make it our child
	{
		Rockets = new List<Rigidbody>();
		GameObject clone = Instantiate(WeaponObject, OurPlayer.position + weaponOffset, Quaternion.identity) as GameObject;
		clone.transform.parent = OurPlayer.transform;

		nextAttack = attackRate;
	}

	void Update()
	{
		iAttack = Input.GetButton("Fire1");
	}

	void Attack()
	{
		Rockets.Add(Instantiate(RocketObject, OurPlayer.position + RocketOffset, Quaternion.identity) as Rigidbody);
	}

	void FixedUpdate()
	{
		if(iAttack && (Time.time > nextAttack) )
		{
			nextAttack = Time.time + attackRate;
			Attack();
		}

		foreach (Rigidbody rocket in Rockets)
		{
			rocket.AddForce(rocketForce * rocket.rotation.eulerAngles);
		}
	}
}
