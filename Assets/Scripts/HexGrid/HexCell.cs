using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class HexCell : MonoBehaviour {
	public HexCoordinates coordinates;
	public CellState cellState;
	HexImages hexImage;
	[SerializeField]
	HexCell[] neighbors;
	public void OnMouseDown() {
		Debug.Log(coordinates);
	}
	public void SetOccupier(Occupier occupier) {
		if (cellState.occupier!=null) {
			Destroy(cellState.occupier);
		}
		cellState.occupier = (CellOccupier)occupier;
	}
	public HexCell GetNeighbor (HexDirection direction) {
		return neighbors[(int)direction];
	}
	public HexCell[] GetNeighbours() {
		List<HexCell> neighbours = neighbors.ToList();
		while (neighbours.Remove(null));
		return neighbours.ToArray();
	}
	public HexCell[] GetNearbys() {
		List<HexCell> nearbys = new List<HexCell>();
		HexDirection[] directions = {HexDirection.NE,HexDirection.E,HexDirection.SE,HexDirection.SW,HexDirection.W,HexDirection.NW};
		foreach (HexDirection direction in directions) {
			HexCell neighbour = GetNeighbor(direction);
			if (neighbour!=null) {
				HexCell nearby = neighbour.GetNeighbor(direction);
				if (nearby!=null) nearbys.Add(nearby);
				HexDirection nextDir;
				if (direction==HexDirection.NW) {
					nextDir = HexDirection.NE;
				} else {
					nextDir = direction+1;
				}
				nearby = neighbour.GetNeighbor(nextDir);
				if (nearby!=null) nearbys.Add(nearby);
			}
		}
		return nearbys.ToArray();
	}
	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}
	public void Save (BinaryWriter writer) {
		writer.Write(cellState.elevation);
	}

	public void Load (BinaryReader reader) {
		SetElevation(reader.ReadInt32());
	}
	public void SetElevation(int elevation) {
		cellState.elevation = elevation;
		transform.position = new Vector3(transform.position.x,elevation * GameConstants.elevationDistance,transform.position.z);
	}
}