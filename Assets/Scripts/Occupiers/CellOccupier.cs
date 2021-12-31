using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellOccupier
{
    public OccupierType occupierType;
    public enum OccupierType {
        waterSource,
        water,
        path,
        plant
    }
}
