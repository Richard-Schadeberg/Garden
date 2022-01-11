using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWater : UIOccupier
{
    public bool makesWater;
    public override void OnPlacement(HexCell cell) {
        cell.cellState.Water = makesWater;
    }
}