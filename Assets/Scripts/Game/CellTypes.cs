using UnityEngine;

[System.Serializable]
public struct CellState {
    public bool Water;
    public CellContents contents;
    public int elevation;
    public Wetness wetness;
    public Shadiness shadiness;
}
[System.Serializable]
public enum Wetness {
    Dry,
    Damp,
    Wet
}
[System.Serializable]
public enum Shadiness {
    Sunny,
    Shaded,
    Dark
}