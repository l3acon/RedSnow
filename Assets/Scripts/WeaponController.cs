using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {
	
		
	public GameObject cameraObject;
	public GameObject WeaponObject;
	Weapon weapon;
	AIcontroller bot;
	FollowCamera followCamera;
	
	public GameObject OurPlayer;
	public bool human; // indiciating whether or not the player is a bot/AI
	//public Rigidbody OurPlayer;
	//public Vector3 weaponOffset;
	//public float nextAttack;
	//protected bool iAttack;
	
	//List<Rigidbody> projectiles;



	//protected virtual void Attack() {}
	
	//protected virtual void Attack(Vector3 target) {}

	protected void start()
	{
		followCamera = cameraObject.GetComponent<FollowCamera>();
		weapon = WeaponObject.GetComponent<Weapon>();
		if(human == false)
		{
			bot = OurPlayer.GetComponent<AIcontroller>();
		}
		
		//projectiles = new List<Rigidbody>();
		//GameObject weaponObject = Instantiate(WeaponObject, OurPlayer.transform.position + weaponOffset, Quaternion.identity) as GameObject;
		//WeaponObject.transform.position = OurPlayer.transform.position+weaponOffset; // move the weapon to the desired offset from the player
		//WeaponObject.transform.parent = OurPlayer.transform; // set the weapon object's transform to be a child of our player's transform.

		//nextAttack = attackRate;
		//Setup();
	}

	
	protected void AttackCheck()
	{
		
		if(Input.GetButton("Fire1") && (Time.fixedDeltaTime > 1/weapon.attackRate) ) // if attack rate was 3 times per second. we need Time.time to be > 1/3 a second
		{
			weapon.Attack();
		}
	}
	
	// attack method to be used by the bots
	// should we make this it's own controller?
	/*
	public void BotAttackCheck(Vector3 target)
	{
		if(Time.fixedDeltaTime > 1/weapon.attackRate) 
		{
			//Attack(target);
		}
	}
	 */
	
	/*
	protected void Setup()
	{
		GameObject clone = Instantiate(WeaponObject, OurPlayer.transform.position + weaponOffset, Quaternion.identity) as GameObject;
		clone.transform.parent = OurPlayer.transform;

		//nextAttack = attackRate;
	}
	*/

	
	
}
