using System;
using UnityEngine;

public class Laser : MonoBehaviour {

	public int maxBounces = 40;
	
	public void OnDrawGizmos() {
		Vector2 origin = transform.position;
		Vector2 dir = transform.right; // x axis
		Ray ray = new Ray( origin, dir );

		for( int i = 0; i < maxBounces; i++ ) {
			if( Physics.Raycast( ray, out RaycastHit hit ) ) {
				Gizmos.color = Color.red;
				Gizmos.DrawLine( ray.origin, hit.point );
				Vector2 reflected = Reflect( ray.direction, hit.normal );
				Gizmos.color = Color.white;
				Gizmos.DrawLine( hit.point, (Vector2)hit.point+reflected );
				ray.direction = reflected;
				ray.origin = hit.point;
			} else {
				break;
			}
		}
	}

	Vector3 Reflect( Vector3 inDir, Vector3 n ) {
		float proj = Vector3.Dot( inDir, n );
		return inDir - 2 * proj * n;
	}

}