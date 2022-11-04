using UnityEngine;

public static class MathFG {

	// math utilities
	public const float TAU = 6.28318530718f;

	public static Vector2 AngToDir( float angleRad ) =>
		new Vector2(
			Mathf.Cos( angleRad ),
			Mathf.Sin( angleRad )
		);

	public static float DirToAng( Vector2 v ) => Mathf.Atan2( v.y, v.x );

	public static float QuadEaseIn( float t ) => t * t;
	public static float QuadEaseOut( float t ) => 1f - QuadEaseIn( 1f - t );
	public static float CubicInOut( float t ) => t * t * ( 3 - 2 * t );
	public static float EaseOutBack( float t ) => CustomEase( 5, 0, t );

	// custom start/end derivatives
	public static float CustomEase( float a, float b, float t ) {
		float c3 = ( a + b - 2 );
		float c2 = ( 3 - 2 * a - b );
		float t2 = t * t;
		float t3 = t2 * t;
		return c3 * t3 + c2 * t2 + a * t;
	}

}