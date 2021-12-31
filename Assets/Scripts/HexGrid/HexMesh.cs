using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour {

	Mesh hexMesh;
	List<Vector3> vertices;
	List<int> triangles;
	List<Color> colours;
	MeshCollider meshCollider;
	void Awake () {
		GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
		meshCollider = gameObject.AddComponent<MeshCollider>();
		hexMesh.name = "Hex Mesh";
		vertices = new List<Vector3>();
		triangles = new List<int>();
		colours = new List<Color>();
	}	
    public void Triangulate (HexCell[] cells) {
		hexMesh.Clear();
		vertices.Clear();
		triangles.Clear();
		colours.Clear();
		for (int i = 0; i < cells.Length; i++) {
			Triangulate(cells[i]);
		}
		hexMesh.vertices = vertices.ToArray();
		hexMesh.triangles = triangles.ToArray();
		hexMesh.colors = colours.ToArray();
		hexMesh.RecalculateNormals();
		meshCollider.sharedMesh = hexMesh;
	}
	
	void Triangulate (HexCell cell) {
		Color color = cell.GetColour();
		Vector3 center = cell.transform.localPosition + new Vector3(0,cell.cellState.elevation*GameConstants.elevationDistance,0);
		cell.hexImage.gameObject.transform.position = center;
		cell.hexImage.reflectState(cell.cellState);
		CapsuleCollider collider = cell.GetComponent<CapsuleCollider>();
		collider.center = new Vector3(0,cell.cellState.elevation*GameConstants.elevationDistance - collider.height/2 + collider.radius,0);
		for (int i = 0; i < 6; i++) {
			AddTriangle(
				center,
				center + HexMetrics.corners[i],
				center + HexMetrics.corners[i + 1],
				color
			);
			AddTriangle(
				center + HexMetrics.corners[i],
				center + HexMetrics.corners[i] + new Vector3(0,-10*GameConstants.elevationDistance,0),
				center + HexMetrics.corners[i + 1],
				color
			);
			AddTriangle(
				center + HexMetrics.corners[i+1],
				center + HexMetrics.corners[i] + new Vector3(0,-10*GameConstants.elevationDistance,0),
				center + HexMetrics.corners[i + 1] + new Vector3(0,-10*GameConstants.elevationDistance,0),
				color
			);
		}
	}
    void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3,Color color) {
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
		colours.Add(color);
		colours.Add(color);
		colours.Add(color);
	}
}