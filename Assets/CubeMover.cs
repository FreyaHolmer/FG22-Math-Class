using UnityEngine;

public class CubeMover : MonoBehaviour {

	Vector3 velocity;
	Vector3 acc;
	public float playerAccMagnitude = 1; // meters per second^2
	public float drag = 1;

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawRay( transform.position, velocity );
		Gizmos.color = Color.cyan;
		Gizmos.DrawRay( transform.position, acc );
	}

	// every rendered frame
	public void Update() {
		acc = Vector3.zero;
		if( Input.GetKey( KeyCode.W ) )
			acc += Vector3.up;
		if( Input.GetKey( KeyCode.S ) )
			acc += Vector3.down;
		if( Input.GetKey( KeyCode.A ) )
			acc += Vector3.left;
		if( Input.GetKey( KeyCode.D ) )
			acc += Vector3.right;
		acc = acc.normalized * playerAccMagnitude;
		velocity += acc * Time.deltaTime;

		// 1 meter per second
		float dt = Time.deltaTime;
		transform.position += velocity * dt;
	}

	void FixedUpdate() {
		velocity /= drag; // movement dampening
	}

}