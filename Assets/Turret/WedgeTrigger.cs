using UnityEditor;
using UnityEngine;

public class WedgeTrigger : MonoBehaviour {

	public Transform target;

	public float radius = 1;
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

		Vector3 vLeft = new Vector3( -x, 0, p ) * radius;
		Vector3 vRight = new Vector3( x, 0, p ) * radius;
		
		Quaternion rotA = Quaternion.Euler( 90, 90, 45 );
		Quaternion rotB = Quaternion.AngleAxis( 60, Vector3.right );
		Quaternion rotCombined = rotA * rotB; // Combined rotation
		Quaternion rotInverse = Quaternion.Inverse( rotCombined );
		Vector3 rotateVector = rotInverse * vLeft;

		Handles.DrawWireArc( default, Vector3.up, vLeft, fovDeg, radius );
		Handles.DrawWireArc( top, Vector3.up, vLeft, fovDeg, radius );

		Gizmos.DrawRay( default, vLeft );
		Gizmos.DrawRay( default, vRight );
		Gizmos.DrawRay( top, vLeft );
		Gizmos.DrawRay( top, vRight );

		Gizmos.DrawLine( default, top );
		Gizmos.DrawLine( vLeft, top + vLeft );
		Gizmos.DrawLine( vRight, top + vRight );
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
		if( flatDistance > radius )
			return false; // outside the infinite cylinder

		// we're inside!
		return true;
	}


}