using System;
using UnityEngine;

public class Transformation : MonoBehaviour {

	public Vector2 worldCoord;
	public Vector2 localCoord;

	public void OnDrawGizmos() {
		Matrix4x4 localToWorldMtx = transform.localToWorldMatrix;
		Vector4 vector = default;

		Vector4 vec = localToWorldMtx * vector;


		// localToWorldMtx.MultiplyPoint3x4(  ) // faster

		// Transforming between local and world
		// transform.TransformPoint( )//             M*( v.x, v.y, v.z, 1)
		// transform.InverseTransformPoint(  ) // M^-1*( v.x, v.y, v.z, 1)
		// transform.TransformVector(  )       //    M*( v.x, v.y, v.z, 0)
		// transform.InverseTransformVector(  )// M^-1*( v.x, v.y, v.z, 0)


		// update in the inspector:
		localCoord = WorldToLocal( worldCoord );

		Gizmos.DrawSphere( worldCoord, 0.1f );
	}

	// 3b
	Vector3 WorldToLocal( Vector3 world ) {
		// Matrix4x4 mtx = Matrix4x4.TRS( new Vector3( 2, 5, 6 ), Quaternion.identity, Vector3.one );
		return transform.InverseTransformPoint( world );
		// return transform.worldToLocalMatrix.MultiplyPoint3x4( world );
		// Vector3 rel = world - transform.position;
		// float x = Vector3.Dot( rel, transform.right ); // x axis
		// float y = Vector3.Dot( rel, transform.up ); // y axis
		// float z = Vector3.Dot( rel, transform.forward ); // z axis
		// return new(x, y, z);
	}

	// 3a
	Vector3 LocalToWorld( Vector3 local ) {
		return transform.TransformPoint( local );
		// return transform.localToWorldMatrix.MultiplyPoint3x4( local );
		// return transform.localToWorldMatrix * new Vector4( local.x, local.y, local.z, 1 );
		// Vector3 position = transform.position;
		// position += local.x * transform.right; // x axis
		// position += local.y * transform.up; // y axis
		// position += local.z * transform.forward; // z axis
		// return position;
	}


}