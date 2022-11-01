using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class MathTrigger : MonoBehaviour {

	public enum Shape {
		Wedge,
		Spherical,
		Cone
	}

	public Transform target;

	public Shape shape;
	[FormerlySerializedAs( "radius" )]
	public float radiusOuter = 1;
	public float radiusInner = 1;
	public float height = 1;
	[Range( 0, 360 )]
	public float fovDeg = 45;

	float FovRad => fovDeg * Mathf.Deg2Rad;
	float AngThresh => Mathf.Cos( FovRad / 2 );

	void SetGizmoMatrix( Matrix4x4 m ) => Gizmos.matrix = Handles.matrix = m;

	void OnDrawGizmos() {
		// makes gizmos relative to this transform
		SetGizmoMatrix( transform.localToWorldMatrix );
		Gizmos.color = Handles.color = Contains( target.position ) ? Color.white : Color.red;

		switch( shape ) {
			case Shape.Wedge:
				DrawWedgeGizmo();
				break;
			case Shape.Spherical:
				DrawSphereGizmo();
				break;
			case Shape.Cone:
				DrawConeGizmo();
				break;
		}
	}

	Stack<Matrix4x4> matrices = new Stack<Matrix4x4>();
	void PushMatrix() => matrices.Push( Gizmos.matrix );
	void PopMatrix() => SetGizmoMatrix( matrices.Pop() );

	void DrawConeGizmo() {
		float p = AngThresh;
		float x = Mathf.Sqrt( 1 - p * p );
		Vector3 vLeftDir = new Vector3( -x, 0, p );
		Vector3 vRightDir = new Vector3( x, 0, p );
		Vector3 vLeftOuter = vLeftDir * radiusOuter;
		Vector3 vRightOuter = vRightDir * radiusOuter;
		Vector3 vLeftInner = vLeftDir * radiusInner;
		Vector3 vRightInner = vRightDir * radiusInner;

		// arcs
		void DrawFlatWedge() {
			Handles.DrawWireArc( default, Vector3.up, vLeftOuter, fovDeg, radiusOuter );
			Handles.DrawWireArc( default, Vector3.up, vLeftInner, fovDeg, radiusInner );
			Gizmos.DrawLine( vLeftInner, vLeftOuter );
			Gizmos.DrawLine( vRightInner, vRightOuter );
		}

		DrawFlatWedge();
		PushMatrix(); // saves the current matrix to the stack
		SetGizmoMatrix( Gizmos.matrix * Matrix4x4.TRS( default, Quaternion.Euler( 0, 0, 90 ), Vector3.one ) );
		DrawFlatWedge();
		PopMatrix();

		// rings
		void DrawRing( float turretRadius ) {
			float a = FovRad / 2;
			float dist = turretRadius * Mathf.Cos( a );
			float radius = turretRadius * Mathf.Sin( a );
			Vector3 center = new Vector3( 0, 0, dist );
			Handles.DrawWireDisc( center, Vector3.forward, radius );
		}

		DrawRing( radiusOuter );
		DrawRing( radiusInner );
	}

	void DrawSphereGizmo() {
		Gizmos.DrawWireSphere( default, radiusInner );
		Gizmos.DrawWireSphere( default, radiusOuter );
	}

	void DrawWedgeGizmo() {
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

	public bool Contains( Vector3 position ) =>
		shape switch {
			Shape.Wedge     => WedgeContains( position ),
			Shape.Spherical => SphereContains( position ),
			Shape.Cone      => ConeContains( position ),
			_               => throw new IndexOutOfRangeException()
		};


	static float AngleBetweenNormalizedVectors( Vector3 a, Vector3 b ) {
		return Mathf.Acos( Mathf.Clamp( Vector3.Dot( a, b ), -1, 1 ) );
	}

	bool ConeContains( Vector3 position ) {
		if( SphereContains( position ) == false )
			return false;
		Vector3 dirToTarget = ( position - transform.position ).normalized;
		float angleRad = AngleBetweenNormalizedVectors( transform.forward, dirToTarget );
		return angleRad < FovRad / 2;
	}

	bool SphereContains( Vector3 position ) {
		float distance = Vector3.Distance( transform.position, position );
		return distance >= radiusInner && distance <= radiusOuter;
	}

	bool WedgeContains( Vector3 position ) {
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