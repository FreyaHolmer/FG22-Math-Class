using UnityEngine;

public class Spring : MonoBehaviour {

	public bool torusMode;
	public Gradient colorGradient;
	public Color colorStart;
	public Color colorEnd;
	[Range( 0, 4 )] public float height = 1;
	[Range( 0, 2 )] public float radiusMinor = 1;
	[Range( 0, 2 )] public float radiusMajor = 1;
	[Range( 0, 16 )] public float turnCount = 1;

	void OnDrawGizmos() {
		Gizmos.matrix = transform.localToWorldMatrix;
		int DETAIL = 256;
		Vector3 prev = GetSpringPoint( 0 );
		for( int i = 1; i < DETAIL; i++ ) {
			float t = i / ( DETAIL - 1f );
			// float tColor = Mathf.Repeat( t + Time.realtimeSinceStartup, 1 );
			Gizmos.color = colorGradient.Evaluate( t ); //Color.LerpUnclamped( colorStart, colorEnd, t );
			Vector3 pt = GetSpringPoint( t );
			Gizmos.DrawLine( prev, pt );
			prev = pt;
		}
	}

	Vector3 GetSpringPoint( float t ) {
		float coilAngle = t * MathFG.TAU * turnCount; // in radians!
		Vector2 xzVec = MathFG.AngToDir( coilAngle ) * radiusMinor;

		if( torusMode ) {
			Vector3 pDir = MathFG.AngToDir( t * MathFG.TAU );
			Vector3 p = pDir * radiusMajor;
			return p + xzVec.x * pDir + new Vector3( 0, 0, xzVec.y );
		}

		return new Vector3( xzVec.x, xzVec.y, t * height );
	}

}