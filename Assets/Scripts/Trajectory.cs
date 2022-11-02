using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour {

	[Range( 0, 8 )]
	public float launchSpeed = 1;
	public float drawDuration = 1;
	Vector3 Pos => transform.position;
	Vector3 Vel => transform.right * launchSpeed;
	Vector3 Acc => Physics.gravity;
	public Rigidbody rb; // test object

	void Awake() {
		if( rb != null ) {
			rb.position = Pos;
			rb.velocity = Vel;
		}
	}

	List<Vector3> drawPts = new List<Vector3>();

	void OnDrawGizmos() {
		const int DETAIL = 80;
		drawPts.Clear();
		for( int i = 0; i < DETAIL; i++ ) {
			float t = i / ( DETAIL - 1f );
			float time = t * drawDuration;
			drawPts.Add( GetPoint( time ) );
		}

		for( int i = 0; i < DETAIL - 1; i++ ) // draw lines (connect the samples)
			Gizmos.DrawLine( drawPts[i], drawPts[i + 1] );
	}

	public Vector3 GetPoint( float t ) => Pos + Vel * t + ( Acc / 2 ) * t * t;
	public Vector3 GetVelocity( float t ) => Vel + Acc * t;

}