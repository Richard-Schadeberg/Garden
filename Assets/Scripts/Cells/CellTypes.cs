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
    public bool Valid;
    public bool Water;
    public bool Path;
    public CellOccupier occupier;
    public int elevation;
    public Wetness wetness;
    public Shadiness shadiness;
}
[System.Serializable]
public enum Wetness {
    Dry,
    Damp,
    Wet,
    HotDry,
    ColdWet
}
[System.Serializable]
public enum Shadiness {
    Sunny,
    Shaded,
    Dark,
    HotSunny,
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