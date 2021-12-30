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
		float hue,sat,val;
		sat=0;
		val=0;
		hue = 240f/360f;

		if (cellState.wetness==Wetness.Wet) {
			sat=0.04f;
			hue = 120f/360f;
		}
		if (cellState.wetness==Wetness.Damp) {
			sat=0.02f;
			hue = 120f/360f;
		}
		if (cellState.wetness==Wetness.Dry) {
			sat=0.06f;
			hue = 33f/360f;
		}
		if (cellState.Water) {
			sat=1f;
			hue = 240f/360f;
		}

		if (cellState.shadiness==Shadiness.Sunny) val=1f;
		if (cellState.shadiness==Shadiness.Shaded) val=0.7f;
		if (cellState.shadiness==Shadiness.Dark) val=0.4f;

		hue += coordinates.Z*1f/360f;
		val -= coordinates.Z*1f/50f;

		return Color.HSVToRGB(hue,sat,val);
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
	public void Save (BinaryWriter writer) {
		writer.Write(cellState.elevation);
	}

	public void Load (BinaryReader reader) {
		cellState.elevation = reader.ReadInt32();
	}
}