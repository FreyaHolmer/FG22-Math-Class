using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class MathTrigger : MonoBehaviour {

	public Transform target;

	[FormerlySerializedAs( "radius" )]
	public float radiusOuter = 1;
	public float radiusInner = 1;
	public float height = 1;
	[Range( 0, 180 )]
	public float fovDeg = 45;

	float FovRad => fovDeg * Mathf.Deg2Rad;
	float AngThresh => Mathf.Cos( FovRad / 2 );

	void OnDrawGizmos() {
		// makes gizmos relative to this transform
		Gizmos.matrix = Handles.matrix = transform.localToWorldMatrix;
		Gizmos.color = Handles.color = Contains( target.position ) ? Color.white : Color.red;

		Vector3 top = new Vector3( 0, height, 0 );

		float p = AngThresh;
		float x = Mathf.Sqrt( 1 - p * p );

		Vector3 vLeftDir = new Vector3( -x, 0, p );
		Vector3 vRightDir = new Vector3( x, 0, p );
		Vector3 vLeftOuter = vLeftDir * radiusOuter;
		Vector3 vRightOuter = vRightDir * radiusOuter;
		Vector3 vLeftInner = vLeftDir * radiusInner;
		Vector3 vRightInner = vRightDir * radiusInner;

		Handles.DrawWireArc( default, Vector3.up, vLeftOuter, fovDeg, radiusOuter );
		Handles.DrawWireArc( top, Vector3.up, vLeftOuter, fovDeg, radiusOuter );

		Handles.DrawWireArc( default, Vector3.up, vLeftInner, fovDeg, radiusInner );
		Handles.DrawWireArc( top, Vector3.up, vLeftInner, fovDeg, radiusInner );

		Gizmos.DrawLine( vLeftInner, vLeftOuter );
		Gizmos.DrawLine( vRightInner, vRightOuter );
		Gizmos.DrawLine( top + vLeftInner, top + vLeftOuter );
		Gizmos.DrawLine( top + vRightInner, top + vRightOuter );

		Gizmos.DrawLine( vLeftInner, top + vLeftInner );
		Gizmos.DrawLine( vRightInner, top + vRightInner );
		Gizmos.DrawLine( vLeftOuter, top + vLeftOuter );
		Gizmos.DrawLine( vRightOuter, top + vRightOuter );
	}

	public bool Contains( Vector3 position ) {
		Vector3 vecToTargetWorld = position - transform.position;

		// inverse transform is world to local
		Vector3 vecToTarget = transform.InverseTransformVector( vecToTargetWorld );

		// height position check
		if( vecToTarget.y < 0 || vecToTarget.y > height )
			return false; // outside the height range

		// angular check
		Vector3 flatDirToTarget = vecToTarget;
		flatDirToTarget.y = 0;
		float flatDistance = flatDirToTarget.magnitude;
		flatDirToTarget /= flatDistance; // normalizes flatDirToTarget
		if( flatDirToTarget.z < AngThresh )
			return false; // outside the angular wedge

		// cylindrical radial test
		if( flatDistance > radiusOuter || flatDistance < radiusInner )
			return false; // outside the infinite cylinder

		// we're inside!
		return true;
	}


}