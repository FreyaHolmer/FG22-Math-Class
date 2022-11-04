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
	
}