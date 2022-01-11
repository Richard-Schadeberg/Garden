using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWaterSource : UIOccupier {
    public override CellOccupier MakeCellOccupier(GameObject newObject) {
        return newObject.AddComponent(typeof(CellWaterSource)) as CellWaterSource;
    }
    public override void OnPlacement(HexCell cell) {
        base.OnPlacement(cell);
        cell.cellState.Water = true;
    }
}
