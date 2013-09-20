using UnityEngine;
using System.Collections;

/// <summary>
/// Follow camera.cs
/// author: ecmartz
/// Essentially we have no control over this camera. It is a rip off of Follow. We need the ability to aim.
/// 
/// Aim: 360 degrees without the camera going under the terrain
/// 	For this, we would need a crosshair empty object at which the camera can pan over to the player and
/// 	then perform a LookAt() to the crosshair object.
/// </summary>/

public class FollowCamera : MonoBehaviour {
	
	public Transform target;
	public float damping = 1.0f;
	
	private Vector3 _offset;
	private Transform _target;
	private float _currentAngle;
	private float _desiredAngle;
	private float _angle;
	private Quaternion _rotation;
	
	// Use this for initialization
	void Start () {
		_offset = target.transform.position - this.transform.position;
		
		_target = target;
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		_currentAngle = transform.eulerAngles.y;
		_desiredAngle = target.transform.eulerAngles.y;
		_angle = Mathf.LerpAngle(_currentAngle, _desiredAngle, Time.deltaTime * damping);
		_rotation = Quaternion.Euler(0, _angle, 0);
		transform.position = target.transform.position - (_rotation * _offset);
		transform.LookAt(target.transform);
		
		// An aiming feature would require the player to hold a mouse or controller button to aim, much like aiming
		// in Ocarina of Time
	}
	
	//There could be a "collider" function that prevents the camera from going under the terrain
	
}
