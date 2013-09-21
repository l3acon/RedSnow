using UnityEngine;
using System.Collections;


/// <summary>
/// Minimap follow.cs
/// Script that makes the minimap follow the player. Very simple.
/// author: ecmartz
/// </summary>
public class minimapFollow : MonoBehaviour 
{
	public Transform target;
	public float height = 5.0f;
	
	void Start () {
		//Go to player!
		transform.position = new Vector3(target.position.x,target.transform.position.y+height,target.position.z);
	}
	
	
	void LateUpdate()
	{
		//Move with player
		transform.position = new Vector3(target.position.x,transform.position.y,target.position.z);
	}
}
