using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleTesting : MonoBehaviour {

	public Transform aTf;
	public Transform bTf;
	public Transform cTf;
	public Transform pTf;

	void OnDrawGizmos() {
		Vector2 a = aTf.position;
		Vector2 b = bTf.position;
		Vector2 c = cTf.position;
		Vector2 pt = pTf.position;

		Gizmos.DrawSphere( pt, 0.02f );

		Gizmos.color = TriangleContains( a, b, c, pt ) ? Color.white : Color.red;
		Gizmos.DrawLine( a, b );
		Gizmos.DrawLine( b, c );
		Gizmos.DrawLine( c, a );
	}

	// wedge product/determinant/perp. dot product/"2D cross product"
	public float Wedge( Vector2 a, Vector2 b ) => a.x * b.y - a.y * b.x;

	bool TriangleContains( Vector2 a, Vector2 b, Vector2 c, Vector2 pt ) {
		bool ab = GetSide( a, b, pt );
		bool bc = GetSide( b, c, pt );
		bool ca = GetSide( c, a, pt );
		return ab == bc && bc == ca;
	}

	bool GetSide( Vector2 a, Vector2 b, Vector2 p ) {
		Vector2 sideVec = b - a;
		Vector2 ptRel = p - a;
		return Wedge( sideVec, ptRel ) > 0;
	}

}