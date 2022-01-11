using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PathMesh : MonoBehaviour {
	const float pathHeight = 3.5f;
	Mesh stoneMesh;
	List<Vector3> vertices;
	List<int> triangles;
	List<Vector3> pathCorners;
	public HexCell hexCell;
	void Awake () {
		hexCell = transform.parent.gameObject.GetComponent<CellOccupier>().hexCell;
		GetComponent<MeshFilter>().mesh = stoneMesh = new Mesh();
		stoneMesh.name = "Hex Mesh";
		vertices = new List<Vector3>();
		triangles = new List<int>();
		pathCorners = new List<Vector3>();
		GenCorners();
		MeshCorners();
		stoneMesh.vertices = vertices.ToArray();
		stoneMesh.triangles = triangles.ToArray();
		stoneMesh.RecalculateNormals();
	}
	void MeshCorners() {
		Vector3 vert = new Vector3(0f,pathHeight,0f);
		for (int i = 0; i < pathCorners.Count - 1; i++) {
			AddTriangle(pathCorners[i],pathCorners[i+1],pathCorners[i+1]+vert);
			AddTriangle(pathCorners[i],pathCorners[i+1]+vert,pathCorners[i]+vert);
		}
		for (int i = 0; i < 24; i+=4) {
			AddTriangle(pathCorners[i]+vert,pathCorners[i+1]+vert,pathCorners[i+2]+vert);
			AddTriangle(pathCorners[i]+vert,pathCorners[i+2]+vert,pathCorners[i+3]+vert);
			AddTriangle(pathCorners[i]+vert,pathCorners[i+3]+vert,vert);
			AddTriangle(pathCorners[i+3]+vert,pathCorners[i+4]+vert,vert);
		}
	}
	void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) {
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
	}
	void GenCorners() {
		pathCorners.Clear();
		foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection))) {
			GenCorners(direction);
		}
		pathCorners.Add(pathCorners[0]);
	}
	void GenCorners(HexDirection direction) {
		Vector3[] pointsArray = new Vector3[4];
		Vector3 cornerCCW = HexMetrics.edgeCornerCCW(direction);
		Vector3 cornerCW = HexMetrics.edgeCornerCW(direction);
		pointsArray[1] = cornerCCW + (HexMetrics.edgeVectorCW(direction)/4f);
		pointsArray[0] = pointsArray[1] - (HexMetrics.edgeMidpoint(direction)/HexMetrics.innerRadius * GameConstants.elevationDistance/2);
		pointsArray[2] = cornerCCW + (HexMetrics.edgeVectorCW(direction)*3f/4f);
		pointsArray[3] = pointsArray[2] - (HexMetrics.edgeMidpoint(direction)/HexMetrics.innerRadius * GameConstants.elevationDistance/2);
		if (hexCell.GetNeighbor(direction) != null) {
			if (hexCell.GetNeighbor(direction).cellState.Elevation == hexCell.cellState.Elevation + 1) {
				pointsArray[1] += new Vector3(0f,GameConstants.elevationDistance/2,0f);
				pointsArray[2] += new Vector3(0f,GameConstants.elevationDistance/2,0f);
			}
			if (hexCell.GetNeighbor(direction).cellState.Elevation == hexCell.cellState.Elevation - 1) {
				pointsArray[1] += new Vector3(0f,-GameConstants.elevationDistance/2,0f);
				pointsArray[2] += new Vector3(0f,-GameConstants.elevationDistance/2,0f);
			}
		}
		foreach (Vector3 point in pointsArray) {
			pathCorners.Add(point);
		}
	}
	Vector3 Flat(Vector3 vector) {
		return new Vector3(vector.x,0f,vector.z);
	}
}
