using UnityEngine;

public class Turret : MonoBehaviour {

	public Transform target;
	public MathTrigger trigger;
	public Transform gunTf;
	public float smoothingFactor = 1;

	Quaternion targetRotation;
	
	// per-frame
	void Update() {
		if( trigger.Contains( target.position ) ) {
			// note: world space
			Vector3 vecToTarget = target.position - gunTf.position;
			targetRotation = Quaternion.LookRotation( vecToTarget, transform.up );
		} else {}
		
		// smoothing rotate toward target
		gunTf.rotation = Quaternion.Slerp( gunTf.rotation, targetRotation, smoothingFactor * Time.deltaTime );
	}

}