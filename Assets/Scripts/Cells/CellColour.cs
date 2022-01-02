using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellColour : MonoBehaviour {
    public static CellColour S;
    void Start() {
        CellColour.S = this;       
    }
    public Color cellColour;
    public Color waterColour;
}
