using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPath : UIOccupier
{
    public override void OnPlacement(HexCell cell) {
        cell.cellState.Path = true;
        cell.hexGrid.Triangulate();
    }
}
