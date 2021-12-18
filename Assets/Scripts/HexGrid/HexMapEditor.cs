using UnityEngine;

public class HexMapEditor : MonoBehaviour {
	public HexGrid hexGrid;
	void Awake () {
	}
	void Update () {
		if (Input.GetMouseButton(0)) {
			HandleInput();
		}
	}
	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			// hexGrid.ColorCell(hit.point, activeColor);
		}
	}
}