using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;
using Random=UnityEngine.Random;
public class HexGrid : MonoBehaviour {
	public int width = 6;
	public int height = 6;
	public HexCell cellPrefab;
	public HexImages spritePrefab;
    HexMesh hexMesh;
    HexCell[] cells;
	Canvas gridCanvas;
	public CameraPose cameraPose;
	void Awake () {
		hexMesh = GetComponentInChildren<HexMesh>();
		gridCanvas = GetComponentInChildren<Canvas>();
		cells = new HexCell[height * width];
		for (int z = 0, i = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				CreateCell(x, z, i++);
			}
		}
	}
    void Start () {
		hexMesh.Triangulate(cells);
		cameraPose.placeCamera(PointsForCamera());
	}
	public void Triangulate() {
		hexMesh.Triangulate(cells);
	}
	void CreateCell (int x, int z, int i) {
		Vector3 position;	
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.hexImage = Instantiate<HexImages>(spritePrefab);
		cell.hexImage.transform.SetParent(cell.transform);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.hexGrid = this;
		SetCellNeighbours(cell,x,z,i);
	}
	void SetCellNeighbours(HexCell cell,int x,int z,int i) {
		if (x > 0) {
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}
		if (z > 0) {
			if ((z & 1) == 0) {
				cell.SetNeighbor(HexDirection.SE, cells[i - width]);
				if (x > 0) {
					cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
				}
			}
			else {
				cell.SetNeighbor(HexDirection.SW, cells[i - width]);
				if (x < width - 1) {
					cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
				}
			}
		}
	}
	public HexCell GetCellAt (Vector3 position) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		HexCell cell = cells[index];
		return cell;
	}
	Vector3[] PointsForCamera() {
		List<Vector3> points = new List<Vector3>();
		Vector3[] corners = new Vector3[4];
		corners[0] = transform.position;
		corners[1] = corners[0] + new Vector3(0,0,hexgridHeight());
		corners[2] = corners[0] + new Vector3(hexgridWidth(),0,0);
		corners[3] = corners[0] + new Vector3(hexgridWidth(),0,hexgridHeight());
		Vector3[] elevations = new Vector3[GameConstants.maxElevation+2];
		for (int i = 0; i <= GameConstants.maxElevation+1; i++) {
			elevations[i] = new Vector3(0,i*GameConstants.elevationDistance,0);
		}
		Vector3[] hexCorners = HexMetrics.corners;
		foreach (Vector3 corner in corners) {
			foreach (Vector3 elevation in elevations) {
				foreach (Vector3 hexCorner in hexCorners) {
					points.Add(corner + elevation + hexCorner);
				}
			}
		}
		return points.ToArray();
	}
	float hexgridHeight() {
		float cellHeight = 1.5f * HexMetrics.outerRadius;
		return (height-1) * cellHeight;
	}
	float hexgridWidth() {
		float cellWidth = 2f * HexMetrics.innerRadius;
		return (width-0.5f) * cellWidth;
	}
	public void Save (BinaryWriter writer) {
		for (int i = 0; i < cells.Length; i++) {
			cells[i].Save(writer);
		}
	}

	public void Load (BinaryReader reader) {
		for (int i = 0; i < cells.Length; i++) {
			cells[i].Load(reader);
		}
		Triangulate();
	}
	public void RandomizeElevation() {
		foreach (HexCell cell in cells) {
			float w = 2f;
			float max = 1 + (float)cell.coordinates.Z*(((float)GameConstants.maxElevation+w-2f)/((float)height-1f));
			float min = max - w;
			float rand = Random.Range(min,max);
			int elevate = (int)Math.Round(rand);
			if (elevate < 0) elevate = 0;
			if (elevate > GameConstants.maxElevation) elevate = GameConstants.maxElevation;
			cell.cellState.elevation = elevate;

			// int min = (cell.coordinates.Z+1)/2-1;
			// int max = (cell.coordinates.Z+1)/2+1;
			// int elevate = Random.Range(min,max);
			// if (elevate < 0) elevate = 0;
			// if (elevate > 3) elevate = 3;
			// cell.cellState.elevation = elevate;
		}
		Triangulate();
	}
}