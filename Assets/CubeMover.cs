using UnityEngine;

public class CubeMover : MonoBehaviour {

	Vector3 velocity;
	public float playerSpeed = 1; // meters per second

	void OnDrawGizmos() {
		Gizmos.DrawRay( transform.position, velocity );
	}

	// every rendered frame
	public void Update() {
		velocity = Vector3.zero;
		if( Input.GetKey( KeyCode.W ) )
			velocity += Vector3.up;
		if( Input.GetKey( KeyCode.S ) )
			velocity += Vector3.down;
		if( Input.GetKey( KeyCode.A ) )
			velocity += Vector3.left;
		if( Input.GetKey( KeyCode.D ) )
			velocity += Vector3.right;
		velocity = velocity.normalized * playerSpeed;

		// 1 meter per second
		float dt = Time.deltaTime;
		transform.position += velocity * dt;
	}

}