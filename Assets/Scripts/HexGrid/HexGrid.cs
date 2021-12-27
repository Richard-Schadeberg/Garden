using UnityEngine;
using UnityEngine.UI;
using System;
public class HexGrid : MonoBehaviour {
	public int width = 6;
	public int height = 6;
	public HexCell cellPrefab;
    HexMesh hexMesh;
    HexCell[] cells;
	Canvas gridCanvas;
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
		moveCamera();
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
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.hexGrid = this;
		SetCellNeighbours(cell,x,z,i);
		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
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
	public Text cellLabelPrefab;
	public Camera controlledCamera;
	public float controlledCameraTilt = 15;
	void moveCamera() {
		float fov = controlledCamera.fieldOfView;
		float bottomAngle = 90 - (fov/2) + controlledCameraTilt;
		float topAngle    = 90 + (fov/2) + controlledCameraTilt;
		float bottomSlope = (float)Math.Tan(bottomAngle * Math.PI/180);
		float topSlope    = (float)Math.Tan(topAngle    * Math.PI/180);
		float camZ = (hexgridHeight()+16) * topSlope / (topSlope - bottomSlope);
		float camY = camZ * bottomSlope;
		camZ += hexGridBottom()-4;
		float camX = hexGridLeft() + (hexGridWidth()/2);
		Vector3 cameraVector = new Vector3(camX,camY,camZ);
		controlledCamera.transform.position = transform.position + cameraVector;
		controlledCamera.transform.rotation = Quaternion.Euler(90-controlledCameraTilt,0,0);
	}
	float hexgridHeight() {
		float cellHeight = 2f * HexMetrics.outerRadius;
		float overlap = 0.5f * HexMetrics.outerRadius;
		return height * (cellHeight - overlap) + overlap;
	}
	float hexGridWidth() {
		float cellWidth = 2f * HexMetrics.innerRadius;
		float overlap = HexMetrics.innerRadius;
		return width * cellWidth + overlap;
	}
	float hexGridBottom() {
		return -HexMetrics.outerRadius;
	}
	float hexGridLeft() {
		return -HexMetrics.innerRadius;
	}
	public Color waterColour;
	public Color waterSourceColour;
	public Color wetColour;
	public Color dampColour;
	public Color shadeColour;
}