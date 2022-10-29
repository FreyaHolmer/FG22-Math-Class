using UnityEngine;

public class Laser : MonoBehaviour {

	public int maxBounces = 40;

	public void OnDrawGizmos() {
		Vector3 origin = transform.position;
		Vector3 dir = transform.right; // x axis
		Ray ray = new Ray( origin, dir ); // initial laser origin and direction

		for( int i = 0; i < maxBounces; i++ ) {
			if( Physics.Raycast( ray, out RaycastHit hit ) ) {
				// Ray hit!
				Gizmos.color = Color.red;
				Gizmos.DrawLine( ray.origin, hit.point );
				Vector3 reflected = Reflect( ray.direction, hit.normal );
				ray.direction = reflected;
				ray.origin = hit.point;
			} else {
				// Ray missed - there are no more bounces.
				// Just draw a lil ray to indicate the last direction
				Gizmos.color = new Color( 1, 0, 0, 0.5f );
				Gizmos.DrawRay( ray.origin, ray.direction );
				break;
			}
		}
	}

	static Vector3 Reflect( Vector3 inDir, Vector3 n ) {
		float proj = Vector3.Dot( inDir, n );
		return inDir - 2 * proj * n;
	}

}