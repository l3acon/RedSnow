using UnityEngine;
using System.Collections;

public class GameMonitor : MonoBehaviour {
	
	public float gameTime;
	
	// Use this for initialization
	void Start () {
	
		gameTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
		gameTime+= Time.fixedDeltaTime;
	}
	
		void OnGUI() {
		
		GUI.color = Color.red; // set the color
		string time = "Time: " + (int)gameTime;
        GUI.Label(new Rect(10, 10, 100, 20),time); // draw text
		//GUI.Label(new Rect(825, 30, 29, 37), playerPic ); // draw a texture
		//Debug.Log("screenPoint: " + camera.ViewportToScreenPoint(this.transform.position));
    }
}
