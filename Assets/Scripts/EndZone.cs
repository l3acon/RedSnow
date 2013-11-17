using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndZone : MonoBehaviour {

	public GameObject StartZone;
	public List<Transform> players;
	
	public int liftDelay; // time it takes for a player to get on a lift; the player behind has to wait at least this amount of time before being respawned
	private float timer;
	// Use this for initialization
	void Start () {
	
		
	}
	
	// Update is called once per frame
	protected void FixedUpdate()
	{
		timer += Time.fixedDeltaTime;
	}
	
	/*TODO: add a timer that starts after each player gets respawned at the start zone.
	 * The next player has to wait x amount of time before starting his/her next lap.
	 * author:kurtdoggydoggydog
	 * */
	void OnTriggerStay(Collider player)
	{
		Debug.Log("Trigger");
		if(players.Contains(player.transform)&& timer > liftDelay)
		{	
				timer = 0; // reset the timer
				//Debug.Log("Move Player: " + player.name);
				float height = StartZone.transform.localScale.y;
				player.transform.position = new Vector3(StartZone.transform.position.x,StartZone.transform.position.y-height/2,StartZone.transform.position.z);
		
		}
		
	}
	
}


