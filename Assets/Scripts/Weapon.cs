using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {
	
	public Transform Player; // player holding the weapon
	public Vector3 weaponOffset;
	public Rigidbody ProjectileObject; // item that the weapon fires
	public Vector3 projectileOffset; // the offset between the weapon and the projectile

	public float force;
	
	//public Vector3 weaponOffset;
	public float attackRate; // should be times/second
	public float damage;
	public int ammo;
	/*
	 * public bool exlosive;
	 * public bool homing;
	 * //other cool stuff?
	 * 
	 * */
	List<Rigidbody> projectiles;
	
	public void Start()
	{
		transform.position = Player.position + weaponOffset;
		projectiles = new List<Rigidbody>();
	}
	
	public void FixedUpdate()
	{
		transform.position = Player.position + weaponOffset;

	}
	
	public void Attack()
	{
		//ProjectileObject.gameObject();
		Rigidbody projectile = (Rigidbody)Instantiate(ProjectileObject, transform.position + projectileOffset, ProjectileObject.transform.rotation) as Rigidbody;
		projectile.AddForce(force*this.transform.forward);
		projectiles.Add(projectile);
		//Debug.Log("aimerVector: " + followCamera.aimerVector);
		//tmp.AddForce(rocketForce * followCamera.aimerVector);

		//tmp.AddForce(rocketForce * (OurPlayer.transform.forward));
		//projectiles.Add(projectile);
	}
	
}
