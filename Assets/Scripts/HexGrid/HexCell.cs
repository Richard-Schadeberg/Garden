using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HexCell : MonoBehaviour {
	public HexCoordinates coordinates;
	public CellState cellState;
	[SerializeField]
	HexCell[] neighbors;
	public HexGrid hexGrid;
	public HexCell GetNeighbor (HexDirection direction) {
		return neighbors[(int)direction];
	}
	HexCell[] GetNeighbours() {
		List<HexCell> neighbours = neighbors.ToList();
		while (neighbours.Remove(null));
		return neighbours.ToArray();
	}
	HexCell[] GetNearby() {
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
	bool IsNeighbour(HexCell cell) {
		if (cell == this) return true;
		return this.IsNeighbour(cell);
	}
	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}
	public Color GetColour() {
		EvaluateState();
		List<Color> colours = new List<Color>();
		colours.Add(shadeColour(cellState.shadiness));
		if (cellState.Water) colours.Add(hexGrid.waterColour);
		if (cellState.Path) colours.Add(hexGrid.pathColour);
		if (cellState.wetness==Wetness.Wet) colours.Add(hexGrid.wetColour);
		if (cellState.wetness==Wetness.Damp) colours.Add(hexGrid.dampColour);
		if (cellState.wetness==Wetness.Dry) colours.Add(hexGrid.dryColour);
		return BlendColours(colours.ToArray());
	}
	Color BlendColours (Color[] colours) {
		float r=0;
		float g=0;
		float b=0;
		foreach (Color colour in colours) {
			r += colour.r;
			g += colour.g;
			b += colour.b;
		}
		r /= colours.Length;
		g /= colours.Length;
		b /= colours.Length;
		return new Color(r,g,b);
	}
	public void EvaluateState() {
		int shade = 0;
		shade += ShadeFrom(HexDirection.NW);
		shade += ShadeFrom(HexDirection.NE);
		if (shade<1) {
			cellState.shadiness = Shadiness.Sunny;
		} else if (shade<3) {
			cellState.shadiness = Shadiness.Shaded;
		} else {
			cellState.shadiness = Shadiness.Dark;
		}
		cellState.wetness = Wetness.Dry;
		if (cellState.Water) cellState.wetness = Wetness.Wet;
		foreach (HexCell cell in GetNeighbours()) {
			if (cell.cellState.Water) {
				cellState.wetness = Wetness.Wet;
				break;
			}
		}
		if (cellState.wetness==Wetness.Dry) {
			foreach (HexCell cell in GetNearby()) {
				if (cell.cellState.Water) {
					cellState.wetness = Wetness.Damp;
					break;
				}
			}
		}
	}
	int ShadeFrom(HexDirection direction) {
		if (GetNeighbor(direction) != null) {
			int diff = GetNeighbor(direction).cellState.elevation - cellState.elevation;
			if (diff>0) return diff;
		}
		return 0;
	}
	Color shadeColour(Shadiness shadiness) {
		switch (shadiness) {
			case (Shadiness.Sunny):
				return hexGrid.sunnyColour;
			case (Shadiness.Shaded):
				return hexGrid.shadeColour;
			case (Shadiness.Dark):
				return hexGrid.darkColour;
			default:
				return hexGrid.sunnyColour;
		}
	}
}