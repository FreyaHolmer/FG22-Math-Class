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
		// local function
		void TestInput( KeyCode key, Vector3 dir ) {
			if( Input.GetKey( key ) )
				acc += dir;
		}

		acc = Vector3.zero;
		TestInput( KeyCode.W, Vector3.up );
		TestInput( KeyCode.S, Vector3.down );
		TestInput( KeyCode.A, Vector3.left );
		TestInput( KeyCode.D, Vector3.right );

		if( acc != Vector3.zero )
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