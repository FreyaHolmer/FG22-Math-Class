using UnityEngine;

public class MathThing : MonoBehaviour {

	// create two objects and assign them in the inspector
	public Transform A;
	public Transform B;

	public float scProj;

	// draws in the scene view
	void OnDrawGizmos() {
		if( A == null || B == null )
			return;

		Vector2 a = A.position;
		Vector2 b = B.position;

		Gizmos.color = Color.red;
		Gizmos.DrawLine( default, a );
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine( default, b );

		// Normalize A and draw it
		// float aLen = Mathf.Sqrt( a.x * a.x + a.y * a.y );
		// float aLen = a.magnitude;
		// Vector2 aNorm = a/aLen;
		Vector2 aNorm = a.normalized;
		Gizmos.color = Color.red;
		Gizmos.DrawSphere( aNorm, 0.05f );

		// Scalar projection
		scProj = Vector2.Dot( aNorm, b );

		// Vector projection
		Vector2 vecProj = aNorm * scProj;

		Gizmos.color = Color.white;
		Gizmos.DrawSphere( vecProj, 0.05f );
	}
}