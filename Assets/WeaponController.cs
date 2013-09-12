using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public GameObject WeaponObject;
	public Rigidbody OurPlayer;
	public Vector3 weaponOffset;
	public float attackRate;

	private float nextAttack;
	private bool iAttack;


	void Start()	//instanciate our weapon and make it our child
	{
		GameObject clone = Instantiate(WeaponObject, OurPlayer.position + weaponOffset, Quaternion.identity) as GameObject;
		clone.transform.parent = OurPlayer.transform;

		nextAttack = attackRate;
	}

	void FiexedUpdate()
	{
		Debug.Log("fire!");

		if(iAttack && (Time.time < nextAttack) )
		{
			nextAttack = Time.time + attackRate;
			Attack();
		}
	}

	void Update()
	{
		iAttack = Input.GetButton("Fire1");
	}

	void Attack()
	{
		
	}

}
