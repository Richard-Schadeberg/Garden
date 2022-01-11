using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class HexCell : MonoBehaviour {
	public HexCoordinates coordinates;
	public CellState cellState;
	public HexImages hexImage;
	[SerializeField]
	HexCell[] neighbors;
	public void OnMouseDown() {
		if (cellState.Occupier is CellWaterSource) return;
		MainUI.S.CellClicked(this);
	}
	public void SetOccupier(Occupier occupier) {
		ClearOccupier();
		cellState.Occupier = occupier as CellOccupier;
		cellState.Occupier.gameObject.transform.SetParent(gameObject.transform,false);
		cellState.Occupier.hexCell = this;
		hexImage.DisplayOccupier(occupier);
	}
	public void ClearOccupier() {
		if (cellState.Occupier!=null) {
			Destroy(cellState.Occupier.gameObject);
			cellState.Occupier = null;
			hexImage.DisplayOccupier(null);
		}
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
		writer.Write(cellState.Elevation);
	}

	public void Load (BinaryReader reader) {
		SetElevation(reader.ReadInt32());
	}
	public void SetElevation(int elevation) {
		cellState.Elevation = elevation;
		transform.position = new Vector3(transform.position.x,elevation * GameConstants.elevationDistance,transform.position.z);
	}
	public void FlagIncomplete() {
		hexImage.FlagIncomplete();
	}
	public void FlagComplete() {
		hexImage.FlagComplete();
	}
}