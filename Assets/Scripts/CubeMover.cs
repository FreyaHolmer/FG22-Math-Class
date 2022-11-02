using System;
using UnityEngine;

public class CubeMover : MonoBehaviour {

	Vector3 startPos;

	void Awake() => startPos = transform.position;

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawRay( transform.position, new Vector3( VelocityX( Time.time ), 0, 0 ) );
	}

	float PositionX( float t ) => -2 * t + t * t;

	float VelocityX( float t ) => -2 + 2 * t;

	// every rendered frame
	public void Update() {
		transform.position = startPos + new Vector3( PositionX( Time.time ), 0, 0 );
	}

}