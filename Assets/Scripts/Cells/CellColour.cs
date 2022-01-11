using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellColour : MonoBehaviour {
    public static CellColour S;
    void Start() {
        CellColour.S = this;       
    }
    public Color defaultColour;
    public Color waterColour;
    [Range(0f,1f)]
    public float shadeVal;
    public static void ColourGrid(HexGrid hexGrid) {
        foreach (HexCell cell in hexGrid.cells) {
            ColourCell(cell);
        }
    }
    public static void ColourCell(HexCell cell) {
        Color colour = GetColourOf(cell);
        cell.gameObject.GetComponent<Renderer>().material.SetColor("_Color", colour);
    }
    public static Color GetColourOf(HexCell cell) {
        Color colour;
        if (cell.cellState.Water) {
            colour = S.waterColour;
        } else {
            colour = S.defaultColour;
        }
        if (cell.cellState.Shadiness != Shadiness.Sunny) {
            float H,S,V;
            Color.RGBToHSV(colour,out H,out S,out V);
            V -= CellColour.S.shadeVal;
            if (cell.cellState.Shadiness == Shadiness.Dark) {
                V -= CellColour.S.shadeVal;
            }
            colour = Color.HSVToRGB(H,S,V);
        }
        return colour;
    }
}
