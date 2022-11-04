using UnityEngine;

public class GridCube : MonoBehaviour {

	public float moveTime = 1f;
	public AnimationCurve movementAnim;

	// Animation state
	bool isAnimating;
	float animTime;
	Vector3 posStart;
	Vector3 posTarget;

	public void Update() {
		if( isAnimating )
			Animate();
		else
			CheckInput();
	}

	void Animate() {
		animTime += Time.deltaTime;
		float tTime = Mathf.Clamp01( animTime / moveTime );

		// apply easing function to t
		float tValue = movementAnim.Evaluate( tTime ); // EaseOutBack( tTime );

		transform.position = Vector3.LerpUnclamped( posStart, posTarget, tValue );
		if( tTime >= 1f ) // animation done
			isAnimating = false;
	}

	void CheckInput() {
		void CheckMove( KeyCode key, Vector3 dir ) {
			if( Input.GetKeyDown( key ) ) {
				posStart = transform.position;
				posTarget = posStart + dir;
				isAnimating = true;
				animTime = 0;
			}
		}

		CheckMove( KeyCode.W, Vector2.up );
		CheckMove( KeyCode.S, Vector2.down );
		CheckMove( KeyCode.A, Vector2.left );
		CheckMove( KeyCode.D, Vector2.right );
	}


}