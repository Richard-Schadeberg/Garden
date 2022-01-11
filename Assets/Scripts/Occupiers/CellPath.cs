using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CellPath : CellOccupier {
    public GameObject stonePrefab;
    public override void OnPlacement(HexCell cell) {
        GameObject newObject = new GameObject();
        CellOccupier occupier = MakeCellOccupier(newObject);
        occupier.CopyFrom(this);
        cell.SetOccupier(occupier);
        GameObject stone = Instantiate(stonePrefab,occupier.gameObject.transform);
        stone.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        cell.hexImage.DisplayOccupier(null);
        occupier.GetComponent<CellPath>().stonePrefab = stonePrefab;
    }
    public override CellOccupier MakeCellOccupier(GameObject newObject) {
        return newObject.AddComponent(typeof(CellPath)) as CellPath;
    }
    protected override bool CustomTestGoal(OccupierGoal goal) {
        switch (goal.description) {
            case "Connect to West edge":
                return CanReachEdge(true,true);
            case "Connect to East edge":
                return CanReachEdge(false,true);
            case "Connect without large steps":
                return (CanReachEdge(true,false)&&CanReachEdge(false,false));
            default:
                return false;
        }
    }
    bool CanReachEdge(bool isWest,bool allowSteps) {
        HashSet<HexCell> tested = new HashSet<HexCell>();
        return CanReachEdge(tested,isWest,hexCell,allowSteps);
    }
    bool CanReachEdge(HashSet<HexCell> tested,bool isWest,HexCell cell,bool allowSteps) {
        if (cell.GetNeighbor(isWest?HexDirection.W:HexDirection.E)==null) return true;
        foreach (HexCell neighbour in cell.GetNeighbours()) {
            if (!tested.Contains(neighbour) && neighbour.cellState.Occupier != null) {
                if (neighbour.cellState.Occupier.occupierName == occupierName) {
                    if (Math.Abs(neighbour.cellState.Elevation - cell.cellState.Elevation) < 2 || allowSteps) {
                        tested.Add(neighbour);
                        if (CanReachEdge(tested,isWest,neighbour,allowSteps)) return true;
                    }
                }
            }
        }
        return false;
    }
}
