using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWater : UIOccupier
{
    public override void OnPlacement(HexCell cell) {
        cell.cellState.Water = true;
        cell.hexGrid.Triangulate();
    }
}
