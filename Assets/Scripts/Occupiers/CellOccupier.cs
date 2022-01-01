using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellOccupier : Occupier
{
    public OccupierType occupierType;
    public enum OccupierType {
        waterSource,
        water,
        path,
        plant
    }
}
