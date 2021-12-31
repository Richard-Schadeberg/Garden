using UnityEngine;
using System.IO;

public class HexMapEditor : MonoBehaviour {
	public HexGrid hexGrid;
	void Start () {
		Load();
	}
	// void Update () {
	// 	if (Input.GetMouseButton(0)) {
	// 		HandleInput();
	// 	}
	// }
	public void Save() {
		string path = Path.Combine(Application.persistentDataPath, "test.map");
		using (
			BinaryWriter writer =
				new BinaryWriter(File.Open(path, FileMode.Create))
		) {
			hexGrid.Save(writer);
		}
	}
	public void Load() {
		string path = Path.Combine(Application.persistentDataPath, "test.map");
		using (BinaryReader reader = new BinaryReader(File.OpenRead(path))) {
			hexGrid.Load(reader);
		}
	}
	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			HexCell touched = hexGrid.GetCellAt(hit.point);
			applyTool(currentTool,touched);
			hexGrid.Triangulate();
		}
	}
	void applyTool(Tool tool,HexCell touched)  {
		switch (tool) {
			case Tool.None:
				break;
			case Tool.Elevation:
				// touched.transform.position = new Vector3(touched.transform.position.x,elevation * HexMetrics.elevationDistance,touched.transform.position.z);
				touched.cellState.elevation = elevation;
				break;
			case Tool.Water:
				touched.cellState.Water = true;
				break;
			case Tool.RemoveWater:
				touched.cellState.Water = false;
				break;
			case Tool.WaterSource:
				touched.cellState.Water = true;
				break;
			case Tool.Path:
				touched.cellState.Path = true;
				break;
			case Tool.RemovePath:
				touched.cellState.Path = false;
				break;
		}
	}
	public void EditElevation(float i) {
		currentTool = Tool.Elevation;
		elevation = (int)i;
	}
	public void RandomizeElevation() {
		hexGrid.RandomizeElevation();
	}
	public void SetTool(int tool) {currentTool = (Tool)tool;}
	public void SetSeason(int season) {currentSeason = (Season)season;}
	Tool currentTool = Tool.None;
	Season currentSeason = Season.Winter;
	int elevation;
	enum Tool {
		None,
		Elevation,
		Water,
		RemoveWater,
		WaterSource,
		Path,
		RemovePath
	}
}