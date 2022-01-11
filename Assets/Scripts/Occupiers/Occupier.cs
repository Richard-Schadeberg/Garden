using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Occupier : MonoBehaviour {
    public string occupierName;
    [HideInInspector]
    public OccupierConditions conditions;
    public bool Water;
    public bool WetCold;
    public bool Wet;
    public bool Damp;
    public bool Dry;
    public bool DryHot;
    [Space(10)]
    public bool SunnyHot;
    public bool Sunny;
    public bool Shaded;
    public bool Dark;
    public bool DarkCold;
    public OccupierGoal[] goals;
    public Sprite cellImage;
    public void Start() {
        bool[] wetconds = {Water,WetCold,Wet,Damp,Dry,DryHot};
        bool[] sunconds = {SunnyHot,Sunny,Shaded,Dark,DarkCold};
        conditions.wetness = wetconds;
        conditions.shadiness = sunconds;
    }
    public virtual void OnPlacement(HexCell cell) {
        GameObject newObject = new GameObject();
        CellOccupier occupier = MakeCellOccupier(newObject);
        occupier.CopyFrom(this);
        cell.SetOccupier(occupier);
    }
    public virtual CellOccupier MakeCellOccupier(GameObject newObject) {
        return newObject.AddComponent(typeof(CellOccupier)) as CellOccupier;
    }
    public virtual void OnSelection() {}
    public void CopyFrom(Occupier occupier) {
        occupierName = occupier.occupierName;
        conditions = occupier.conditions;
        goals = new OccupierGoal[occupier.goals.Length];
        Array.Copy(occupier.goals,goals,occupier.goals.Length);
        gameObject.name = occupier.gameObject.name;
        Water = occupier.Water;
        WetCold = occupier.WetCold;
        Wet = occupier.Wet;
        Damp = occupier.Damp;
        Dry = occupier.Dry;
        DryHot = occupier.DryHot;
        SunnyHot = occupier.SunnyHot;
        Sunny = occupier.Sunny;
        Shaded = occupier.Shaded;
        Dark = occupier.Dark;
        DarkCold = occupier.DarkCold;
        cellImage = occupier.cellImage;
    }
    public bool CanSurvive(Wetness wetness) {
        return conditions.wetness[(int)wetness];
    }
    public bool CanSurvive(Shadiness shadiness) {
        return conditions.shadiness[(int)shadiness];
    }
    public bool CanSurvive(Wetness wetness,Shadiness shadiness) {
        return (CanSurvive(wetness)&&CanSurvive(shadiness));
    }
    public bool PendingGoals() {
        foreach (OccupierGoal goal in goals) {
            if (goal.essential && !goal.completed) return true;
        }
        return false;
    }
}
