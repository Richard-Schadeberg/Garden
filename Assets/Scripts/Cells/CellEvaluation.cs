using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CellEvaluation {
    public static void EvaluateGrid(HexGrid hexGrid) {
        EvaluateShadiness(hexGrid);
    }
    static void EvaluateShadiness(HexGrid hexGrid) {
        foreach (HexCell cell in hexGrid.cells) {
            EvaluateShadiness(cell);
        }
    }
    static void EvaluateShadiness(HexCell cell) {
        Shadiness shadiness;
		int shade = 0;
		shade += ShadeFrom(cell,HexDirection.NW);
		shade += ShadeFrom(cell,HexDirection.NE);
        if (shade==0) {
            shadiness = Shadiness.Sunny;
        } else if (shade<3) {
            shadiness = Shadiness.Shaded;
        } else {
            shadiness = Shadiness.Dark;
        }
        cell.cellState.shadiness = shadiness;
    }
    static int ShadeFrom(HexCell cell,HexDirection direction) {
        HexCell neighbour = cell.GetNeighbor(direction);
        if (neighbour == null) return 0;
        int rise = neighbour.cellState.elevation - cell.cellState.elevation;
        if (rise > 0) return rise;
        else return 0;
    }
}
