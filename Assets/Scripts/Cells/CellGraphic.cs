using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class CellGraphic {
    public static void BorderElevations(HexGrid hexGrid) {
        foreach (HexCell cell in hexGrid.cells) {
            cell.hexImage.UnblockNeighbours();
        }
        foreach (HexCell cell in hexGrid.cells) {
            foreach(HexDirection direction in Enum.GetValues(typeof(HexDirection))) {
                HexCell neighbour = cell.GetNeighbor(direction);
                if (neighbour==null) {
                    cell.hexImage.BlockNeighbour(direction);
                } else if (neighbour.cellState.Elevation != cell.cellState.Elevation) {
                    cell.hexImage.BlockNeighbour(direction);
                }
            }
        }
    }
}