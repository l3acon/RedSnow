using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public GameObject WeaponObject;
	public Rigidbody OurPlayer;
	public Vector3 weaponOffset;
	public float attackRate;

	public float nextAttack;
	protected bool iAttack;



	protected virtual void Attack() {}

	protected void AttackCheck()
	{
		iAttack = Input.GetButton("Fire1");
		if(iAttack && (Time.time > nextAttack) )
		{
			nextAttack = Time.time + attackRate;
			

			Attack();
		}
	}


	protected void Setup()
	{
		Debug.Log("dome");
		GameObject clone = Instantiate(WeaponObject, OurPlayer.position + weaponOffset, Quaternion.identity) as GameObject;
		clone.transform.parent = OurPlayer.transform;

		nextAttack = attackRate;

	}

}
