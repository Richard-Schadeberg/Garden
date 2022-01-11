using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CellOccupier : Occupier {
    public HexCell hexCell;
    public void ResetGoals() {
        for (int i = 0; i < goals.Length; i++) {
            goals[i].completed = false;
        }
    }
    public void TestGoals() {
        ResetGoals();
        for (int i = 0; i < goals.Length; i++) {
            goals[i].completed = TestGoal(goals[i]);
        }
    }
    bool TestGoal(OccupierGoal goal) {
        switch (goal.description) {
            case "Soil Moisture":
                return CanSurvive(hexCell.cellState.Wetness);
            case "In Water":
                return CanSurvive(hexCell.cellState.Wetness);
            case "Sunlight":
                return CanSurvive(hexCell.cellState.Shadiness);
            case "6 or more in total":
                return CountOfN(6,"Cactus");
            case "2 or more in total":
                return CountOfN(2,"Mushroom");
            case "4 or more in total":
                return CountOfN(4,"Lily");
            case "Group of 4 or more":
                return GroupOfN(4,"Grass");
            case "3 or more next to path":
                return AdjacentTo("Rose","Path",3);
            default:
                return CustomTestGoal(goal);
        }
    }
    protected virtual bool CustomTestGoal(OccupierGoal goal) {return false;}
    bool AdjacentTo(string plantName,string occupierName,int quantity) {
        int i = 0;
        foreach (HexCell cell in HexGrid.S.cells) {
            if (cell.cellState.Occupier != null) {
                if (cell.cellState.Occupier.occupierName == plantName && AdjacentTo(occupierName,cell)) {
                    i++;
                    if (i >= quantity) {
                        return true;
                    }
                    continue;
                }
            }
        }
        return false;
    }
    bool AdjacentTo(string occupierName,HexCell cell) {
        foreach (HexCell neighbour in cell.GetNeighbours()) {
            if (neighbour.cellState.Occupier != null) {
                if (neighbour.cellState.Occupier.occupierName == occupierName) return true;
            }
        }
        return false;

    }
    bool AdjacentTo(string occupierName) {return AdjacentTo(occupierName,hexCell);}
    bool CountOfN(int n,string name) {
        return (HexGrid.S.cells.Count(x => x.cellState.Occupier!=null && x.cellState.Occupier.occupierName==name)>=n);
    }
    bool GroupOfN(int n,string name) {
        foreach (HexCell cell in HexGrid.S.cells) {
            HashSet<HexCell> done = new HashSet<HexCell>();
            if (GroupOfN(n,name,cell,done)) return true;
        }
        return false;
    }
    bool GroupOfN(int n,string name,HexCell cell,HashSet<HexCell> done) {
        if (cell.cellState.Occupier == null) return false;
        if (cell.cellState.Occupier.occupierName != name) return false;
        if (done.Contains(cell)) return false;
        done.Add(cell);
        if (done.Count == n) return true;
        foreach (HexCell neighbour in cell.GetNeighbours()) {
            if (GroupOfN(n,name,neighbour,done)) return true;
        }
        return false;
    }
}
