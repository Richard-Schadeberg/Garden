using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Occupier : MonoBehaviour {
    public string occupierName;
    public OccupierConditions conditions;
    public OccupierGoal[] goals;
    public virtual void OnPlacement(HexCell cell) {}
    public virtual void OnSelection() {}
}
