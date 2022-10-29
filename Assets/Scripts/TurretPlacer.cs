using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlacer : MonoBehaviour {

	public Transform turret;

	void OnDrawGizmos() {
		if( turret == null )
			return;
		Ray ray = new Ray( transform.position, transform.forward );
		if( Physics.Raycast( ray, out RaycastHit hit ) ) {
			turret.position = hit.point;
			Vector3 yAxis = hit.normal;
			Vector3 zAxis = Vector3.Cross( transform.right, yAxis ).normalized;
			Gizmos.color = new Color( 1, 1, 1, 0.4f );
			Gizmos.DrawLine( ray.origin, hit.point );
			turret.rotation = Quaternion.LookRotation( zAxis, yAxis );
		}
	}

}