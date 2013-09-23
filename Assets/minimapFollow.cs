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
	public Texture2D playerPic;
	
	void Start () {
		//Go to player!
		transform.position = new Vector3(target.position.x,target.transform.position.y+height,target.position.z);
	}
	
	
	void LateUpdate()
	{
		//Move with player
		transform.position = new Vector3(target.position.x,transform.position.y,target.position.z);
		
	}
	
	// gui stuff was found here: http://docs.unity3d.com/Documentation/ScriptReference/GUI.Label.html
	void OnGUI() {
		GUI.color = Color.red; // set the color
        GUI.Label(new Rect(100, 20, 100, 20), "P1"); // draw text
		//GUI.Label(new Rect(10, 20, 29, 37),playerPic ); // draw a texture
    }
}
