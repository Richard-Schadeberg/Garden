using UnityEngine;

public class HexCell : MonoBehaviour {
	public HexCoordinates coordinates;
	public CellState cellState;
	[SerializeField]
	HexCell[] neighbors;
	public HexGrid hexGrid;
	public HexCell GetNeighbor (HexDirection direction) {
		return neighbors[(int)direction];
	}
	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}
	public void GetColour() {
		EvaluateState();
	}
	public void EvaluateState() {
		cellState.shadiness = Shadiness.Sunny;
		int shade = 0;
		if (GetNeighbor(HexDirection.NW).cellState.elevation > cellState.elevation) shade++;
		if (GetNeighbor(HexDirection.NE).cellState.elevation > cellState.elevation) shade++;
		if (GetNeighbor(HexDirection.W).cellState.elevation > cellState.elevation) shade++;
		if (GetNeighbor(HexDirection.E).cellState.elevation > cellState.elevation) shade++;
		if (shade==1) {
			cellState.shadiness = Shadiness.Shaded;
		}
	}
}