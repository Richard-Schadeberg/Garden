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
    public HexCell[] cells;
	public CameraPose cameraPose;
	public UIWaterSource sourceObj;
	void Awake () {
		cells = new HexCell[height * width];
		for (int z = 0, i = 0; z < height; z++) {
			for (int x = 0; x < width; x++) {
				CreateCell(x, z, i++);
			}
		}
	}
	public static HexGrid S;
    void Start () {
		S = this;
		cameraPose.placeCamera(PointsForCamera());
		Load();
		CellEvaluation.EvaluateShadiness(this);
		CellColour.ColourGrid(this);
		HexCell source = cellAt(new HexCoordinates(3,5));
		sourceObj.OnPlacement(source);
		CellColour.ColourCell(source);
	}
	void CreateCell (int x, int z, int i) {
		Vector3 position;	
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
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
	Vector3[] PointsForCamera() {
		List<Vector3> points = new List<Vector3>();
		Vector3[] corners = new Vector3[4];
		corners[0] = transform.position;
		corners[1] = corners[0] + new Vector3(0,0,hexgridHeight());
		corners[2] = corners[0] + new Vector3(hexgridWidth(),0,0);
		corners[3] = corners[0] + new Vector3(hexgridWidth(),0,hexgridHeight());
		Vector3[] elevations = new Vector3[GameConstants.maxElevation+3];
		for (int i = 0; i <= GameConstants.maxElevation+2; i++) {
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
	public void Save () {
		string path = Path.Combine(Application.persistentDataPath, "test.map");
		using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create))) {
			for (int i = 0; i < cells.Length; i++) {
				cells[i].Save(writer);
			}
		}
	}
	public void Load () {
		string path = Path.Combine(Application.persistentDataPath, "test.map");
		using (BinaryReader reader = new BinaryReader(File.OpenRead(path))) {
			for (int i = 0; i < cells.Length; i++) {
				cells[i].Load(reader);
			}
		}
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
			cell.SetElevation(elevate);
		}
	}
	public HexCell cellAt(HexCoordinates coordinates) {
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		return cells[index];
	}
}