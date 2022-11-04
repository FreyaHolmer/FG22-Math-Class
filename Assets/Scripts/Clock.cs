using System;
using UnityEditor;
using UnityEngine;

public class Clock : MonoBehaviour {

	[Range( 0, 0.2f )]
	public float tickSizeSecMin = 0.05f;
	[Range( 0, 0.2f )]
	public float tickSizeHours = 0.05f;

	public bool smoothSeconds;
	public bool use24hClock;

	int HoursOnClock => use24hClock ? 24 : 12;

	void OnDrawGizmos() {
		Handles.matrix = Gizmos.matrix = transform.localToWorldMatrix;
		Handles.DrawWireDisc( default, Vector3.forward, 1 );

		// ticks (minutes/seconds)
		for( int i = 0; i < 60; i++ ) {
			Vector2 dir = SecOrMinToDir( i );
			DrawTick( dir, tickSizeSecMin, 1 );
		}

		// ticks (hours)
		for( int i = 0; i < HoursOnClock; i++ ) {
			Vector2 dir = HoursToDir( i );
			DrawTick( dir, tickSizeHours, 3 );
		}

		// hands
		DateTime time = DateTime.Now;
		float seconds = time.Second;
		if( smoothSeconds )
			seconds += time.Millisecond / 1000f;
		DrawClockHand( SecOrMinToDir( seconds ), 0.9f, 1, Color.red );
		DrawClockHand( SecOrMinToDir( time.Minute ), 0.7f, 4, Color.white );
		DrawClockHand( HoursToDir( time.Hour ), 0.5f, 8, Color.white );
	}

	void DrawTick( Vector2 dir, float length, float thickness ) {
		Handles.DrawLine( dir, dir * ( 1f - length ), thickness );
	}

	void DrawClockHand( Vector2 dir, float length, float thickness, Color color ) {
		using( new Handles.DrawingScope( color ) )
			Handles.DrawLine( default, dir * length, thickness );
	}

	Vector2 HoursToDir( float hours ) => ValueToDirection( hours, HoursOnClock );
	Vector2 SecOrMinToDir( float secOrMin ) => ValueToDirection( secOrMin, 60 );

	Vector2 ValueToDirection( float value, float valueMax ) {
		// 0-1 value representing "percent"/fraction along the 0-60 range
		float t = value / valueMax;
		return FractionToClockDir( t );
	}

	Vector2 FractionToClockDir( float t ) {
		float angleRad = ( 0.25f - t ) * MathFG.TAU;
		return MathFG.AngToDir( angleRad );
	}

}