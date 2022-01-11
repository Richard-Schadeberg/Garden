using UnityEngine;

[System.Serializable]
public struct OccupierConditions {
    public bool[] wetness;
    public bool[] shadiness;
}
[System.Serializable]
public struct OccupierGoal {
    public string description;
    public bool essential;
    public bool completed;
}
[System.Serializable]
public struct CellState {
    public bool OccupierValid;
    public bool Water;
    public bool WaterValid;
    public CellOccupier Occupier;
    public int Elevation;
    public Wetness Wetness;
    public Shadiness Shadiness;
}
[System.Serializable]
public enum Wetness {
    Water,
    ColdWet,
    Wet,
    Damp,
    Dry,
    HotDry
}
[System.Serializable]
public enum Shadiness {
    HotSunny,
    Sunny,
    Shaded,
    Dark,
    ColdDark
}
[System.Serializable]
public enum Season {
    Winter,
    Spring,
    Summer,
    Autumn
}
public static class SeasonExtensions {
    public static bool HotSeason(this Season season) {
        if (season == Season.Spring || season == Season.Summer) return true;
        return false;
    }
}