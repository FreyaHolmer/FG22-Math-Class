using System;
using UnityEditor;
using UnityEngine;

public class RadialTrigger : MonoBehaviour {

	public float radius = 1;
	public Transform player;

	void OnDrawGizmos() {
		Vector3 center = transform.position;
		if( player == null )
			return;
		Vector3 playerPos = player.position;
		Vector3 delta = center - playerPos;
		float sqrDist = Vector3.Dot( delta, delta ); // squared length of delta
		bool inside = sqrDist <= radius * radius;
		Gizmos.color = inside ? Color.white : Color.red;
		Gizmos.DrawWireSphere( center, radius );
	}


}