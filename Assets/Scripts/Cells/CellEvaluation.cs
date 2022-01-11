using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class CellEvaluation {
    public static void GridUpdated(HexGrid hexGrid) {
        EvaluateWetness(hexGrid);
        EvaluateShadiness(hexGrid);
        foreach (HexCell cell in hexGrid.cells) {
            if (cell.cellState.Occupier != null) {
                cell.cellState.Occupier.TestGoals();
                if (cell.cellState.Occupier.PendingGoals()) {
                    cell.FlagIncomplete();
                } else {
                    cell.FlagComplete();
                }
            }
        }
        int points=0;
        HashSet<(string,string)> counted = new HashSet<(string, string)>();
        foreach (HexCell cell in hexGrid.cells) {
            if (cell.cellState.Occupier != null) {
                foreach (OccupierGoal goal in cell.cellState.Occupier.goals) {
                    if (goal.completed&&!goal.essential) {
                        if (!counted.Contains((cell.cellState.Occupier.occupierName,goal.description))) {
                            counted.Add((cell.cellState.Occupier.occupierName,goal.description));
                            points++;
                        }
                    }
                }
            }
        }
        QualityCount.S.SetQuality(points);
    }
    public static bool CompletedByAny(string name,string goalname) {
        foreach (HexCell cell in HexGrid.S.cells) {
            Occupier occupier = cell.cellState.Occupier;
            if (occupier!=null) {
                if (occupier.occupierName == name) {
                    foreach (OccupierGoal goal in occupier.goals) {
                        if (goal.completed && goal.description == goalname) return true;
                    }
                }
            }
        }
        return false;
    }
    public static void EvaluateWetness(HexGrid hexGrid) {
        foreach (HexCell cell in hexGrid.cells) {
            cell.cellState.Wetness = Wetness.Dry;
        }
        foreach (HexCell cell in hexGrid.cells) {
            if (cell.cellState.Water) {
                cell.cellState.Wetness = Wetness.Water;
                foreach (HexCell neighbour in cell.GetNeighbours()) {
                    if (neighbour.cellState.Wetness > Wetness.Wet) neighbour.cellState.Wetness = Wetness.Wet;
                }
                foreach (HexCell nearby in cell.GetNearbys()) {
                    if (nearby.cellState.Wetness > Wetness.Damp) nearby.cellState.Wetness = Wetness.Damp;
                }
            }
        }
    }
    public static void EvaluateShadiness(HexGrid hexGrid) {
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
        cell.cellState.Shadiness = shadiness;
    }
    static int ShadeFrom(HexCell cell,HexDirection direction) {
        HexCell neighbour = cell.GetNeighbor(direction);
        if (neighbour == null) return 0;
        int rise = neighbour.cellState.Elevation - cell.cellState.Elevation;
        if (rise > 0) return rise;
        else return 0;
    }
}
