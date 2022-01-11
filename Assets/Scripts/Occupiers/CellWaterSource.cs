using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CellWaterSource : CellOccupier {
    protected override bool CustomTestGoal(OccupierGoal goal) {
        switch (goal.description) {
            case "Connect to all Water tiles":
                return WaterConnectTest();
            case "(Water cannot flow uphill)":
                return WaterConnectTestDownhill();
            case "Connect to South edge":
                return Array.Exists(HexGrid.S.cells,x => x.cellState.Water && x.coordinates.Z==0);
            default:
                return false;
        }
    }
    bool WaterConnectTest() {
        HashSet<HexCell> connected = new HashSet<HexCell>();
        connected.Add(hexCell);
        ConnectNeighbours(connected,hexCell);
        HashSet<HexCell> waterCells = new HashSet<HexCell>();
        foreach (HexCell cell in HexGrid.S.cells) {
            if (cell.cellState.Water) waterCells.Add(cell);
        }
        return (connected.Count == waterCells.Count);
    }
    void ConnectNeighbours(HashSet<HexCell> connected, HexCell cell) {
        foreach (HexCell neighbour in cell.GetNeighbours()) {
            if (neighbour.cellState.Water && !connected.Contains(neighbour)) {
                connected.Add(neighbour);
                ConnectNeighbours(connected,neighbour);
            }
        }
    }
    bool WaterConnectTestDownhill() {
        HashSet<HexCell> connected = new HashSet<HexCell>();
        connected.Add(hexCell);
        ConnectNeighboursDownhill(connected,hexCell);
        HashSet<HexCell> waterCells = new HashSet<HexCell>();
        foreach (HexCell cell in HexGrid.S.cells) {
            if (cell.cellState.Water) waterCells.Add(cell);
        }
        return (connected.Count == waterCells.Count);
    }
    void ConnectNeighboursDownhill(HashSet<HexCell> connected, HexCell cell) {
        foreach (HexCell neighbour in cell.GetNeighbours()) {
            if (neighbour.cellState.Water && neighbour.cellState.Elevation <= cell.cellState.Elevation && !connected.Contains(neighbour)) {
                connected.Add(neighbour);
                ConnectNeighboursDownhill(connected,neighbour);
            }
        }
    }
}