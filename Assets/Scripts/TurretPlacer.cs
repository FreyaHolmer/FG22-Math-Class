using UnityEngine;

public class TurretPlacer : MonoBehaviour {

	public Transform turret;

	float pitchDeg;
	float yawDeg;

	public float mouseSensitivity = 1;

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
			
		UpdateMouseLook();
		PlaceTurret();
	}

	void UpdateMouseLook() {
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
			turret.rotation = Quaternion.LookRotation( zAxis, yAxis );
		}
	}

}