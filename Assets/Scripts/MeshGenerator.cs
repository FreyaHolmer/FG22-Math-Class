using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

	Mesh mesh;

	// right click
	[ContextMenu( "Generate Mesh" )]
	void GenerateMesh() {
		if( mesh == null ) {
			mesh = new Mesh(); // unity object
			GetComponent<MeshFilter>().sharedMesh = mesh;
		}

		List<Vector3> vertices = new List<Vector3>() {
			new Vector3( -1f, 0f, -1f ),
			new Vector3( -1f, 0f, +1f ),
			new Vector3( +1f, 0f, +1f ),
			new Vector3( +1f, 0f, -1f )
		};
		Handles.DrawAAPolyLine();
		List<Vector3> normals = new List<Vector3>() {
			Vector3.up,
			Vector3.up,
			Vector3.up,
			Vector3.up
		};
		List<int> triangles = new List<int>() { 3, 0, 1, 3, 1, 2 };

		mesh.Clear();
		mesh.SetVertices( vertices );
		mesh.SetTriangles( triangles, 0 );
		mesh.SetNormals( normals );
	}

}