using System;
using UnityEngine;

public class TurretPlacer : MonoBehaviour {

	public Transform turret;


	[Header( "Mouse look" )]
	public float mouseSensitivity = 1;
	public float turretYawSensitivity = 6;

	[Header( "Movement" )]
	public float playerAccMagnitude = 400; // meters per second^2
	public float drag = 1.6f;

	// internal state
	Vector3 vel, acc;
	float pitchDeg;
	float yawDeg;
	float turretYawOffsetDeg;

	void Awake() {
		Vector3 startEuler = transform.eulerAngles;
		pitchDeg = startEuler.x;
		yawDeg = startEuler.y;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update() {
		if( Input.GetKeyDown( KeyCode.Escape ) ) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		UpdateMovement();
		UpdateMouseLook();
		UpdateTurretYawInput();
		PlaceTurret();
	}

	void UpdateMovement() {
		// local function
		void TestInput( KeyCode key, Vector3 dir ) {
			if( Input.GetKey( key ) )
				acc += dir;
		}

		acc = Vector3.zero;
		TestInput( KeyCode.W, transform.forward );
		TestInput( KeyCode.S, -transform.forward );
		TestInput( KeyCode.A, -transform.right );
		TestInput( KeyCode.D, transform.right );
		TestInput( KeyCode.Space, Vector3.up );
		TestInput( KeyCode.LeftControl, Vector3.down );

		if( acc != Vector3.zero )
			acc = acc.normalized * playerAccMagnitude;
		vel += acc * Time.deltaTime;

		// 1 meter per second
		float dt = Time.deltaTime;
		transform.position += vel * dt;
	}

	void FixedUpdate() {
		vel /= drag; // movement dampening
	}


	void UpdateTurretYawInput() {
		float scrollDelta = Input.mouseScrollDelta.y;
		turretYawOffsetDeg += scrollDelta * turretYawSensitivity;
		turretYawOffsetDeg = Mathf.Clamp( turretYawOffsetDeg, -90, 90 );
	}

	void UpdateMouseLook() {
		if( Cursor.lockState == CursorLockMode.None )
			return;
		float xDelta = Input.GetAxis( "Mouse X" );
		float yDelta = Input.GetAxis( "Mouse Y" );
		pitchDeg += -yDelta * mouseSensitivity;
		pitchDeg = Mathf.Clamp( pitchDeg, -90, 90 );
		yawDeg += xDelta * mouseSensitivity;
		transform.rotation = Quaternion.Euler( pitchDeg, yawDeg, 0 );
	}

	void PlaceTurret() {
		Ray ray = new Ray( transform.position, transform.forward );
		if( Physics.Raycast( ray, out RaycastHit hit ) ) {
			turret.position = hit.point;
			Vector3 yAxis = hit.normal;
			Vector3 zAxis = Vector3.Cross( transform.right, yAxis ).normalized;
			Debug.DrawLine( ray.origin, hit.point, new Color( 1, 1, 1, 0.4f ) );
			Quaternion offsetRot = Quaternion.Euler( 0, turretYawOffsetDeg, 0 );
			turret.rotation = Quaternion.LookRotation( zAxis, yAxis ) * offsetRot;
		}
	}

}