using System.Collections.Generic;
using UnityEngine;

public class BezierMesh : MonoBehaviour {

	Mesh mesh;

	public Vector3 profilePtA = new Vector3( 0, 0, -1 );
	public Vector3 profilePtB = new Vector3( 0, 0, +1 );

	public int segmentCount;
	public int VertexCount => 2 * ( segmentCount + 1 );
	public int TriangleCount => segmentCount * 2;

	void OnValidate() {
		segmentCount = Mathf.Max( 1, segmentCount ); // make sure it's always greater than 1
	}

	List<Vector3> verts = new List<Vector3>();
	List<Vector3> normals = new List<Vector3>();
	List<int> triangles = new List<int>();

	void OnDrawGizmos() {
		if( mesh == null ) {
			mesh = new Mesh();
			mesh.MarkDynamic();
		}
		GetComponent<MeshFilter>().sharedMesh = mesh;

		Gizmos.matrix = transform.localToWorldMatrix;

		// clear old data
		verts.Clear();
		normals.Clear();
		triangles.Clear();

		// defining vertices
		for( int i = 0; i < segmentCount + 1; i++ ) {
			float t = i / (float)segmentCount;
			Matrix4x4 mtx = GetPoint( t );
			verts.Add( mtx.MultiplyPoint3x4( profilePtA ) );
			verts.Add( mtx.MultiplyPoint3x4( profilePtB ) );
			Vector3 normal = mtx.GetColumn( 1 );
			normals.Add( normal );
			normals.Add( normal );
		}

		// defining triangles
		for( int s = 0; s < segmentCount; s++ ) {
			int root = s * 2;
			int neighbor = root + 1;
			int next = root + 2;
			int nextNeighbor = root + 3;

			// first triangle
			triangles.Add( root );
			triangles.Add( neighbor );
			triangles.Add( next );

			// second triangle
			triangles.Add( neighbor );
			triangles.Add( nextNeighbor );
			triangles.Add( next );
		}

		mesh.Clear();
		mesh.SetVertices( verts );
		mesh.SetNormals( normals );
		mesh.SetTriangles( triangles, 0 );


		// debugging
		// Gizmos.matrix *= bezM;
		// Gizmos.DrawSphere( Vector3.zero, 0.04f );
		// Gizmos.color = Color.red;
		// Gizmos.DrawRay( default, Vector3.right );
		// Gizmos.color = Color.green;
		// Gizmos.DrawRay( default, Vector3.up );
		// Gizmos.color = Color.blue;
		// Gizmos.DrawRay( default, Vector3.forward );
	}

	Vector3 P0 => transform.GetChild( 0 ).localPosition;
	Vector3 P1 => transform.GetChild( 1 ).localPosition;
	Vector3 P2 => transform.GetChild( 2 ).localPosition;
	Vector3 P3 => transform.GetChild( 3 ).localPosition;

	Matrix4x4 GetPoint( float t ) {
		Vector3 a = Vector3.Lerp( P0, P1, t );
		Vector3 b = Vector3.Lerp( P1, P2, t );
		Vector3 c = Vector3.Lerp( P2, P3, t );
		Vector3 d = Vector3.Lerp( a, b, t );
		Vector3 e = Vector3.Lerp( b, c, t );
		Vector3 origin = Vector3.Lerp( d, e, t );
		Vector3 tangent = ( e - d ).normalized;
		Vector3 normal = Vector3.zero;
		normal.x = -tangent.y; // swizzling
		normal.y = tangent.x;

		return new Matrix4x4(
			tangent, // X direction
			normal, // Y direction
			Vector3.forward, // Z direction
			Vec4( origin, 1 ) // position
		);
	}

	Vector4 Vec4( Vector3 v3, float w = 0 ) => new Vector4( v3.x, v3.y, v3.z, w );

}