using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRemover : UIOccupier {
    public override void OnPlacement(HexCell cell) {
        if (cell.cellState.Occupier == null) cell.cellState.Water = false;
        cell.ClearOccupier();
        cell.hexImage.FlagComplete();
    }
}
