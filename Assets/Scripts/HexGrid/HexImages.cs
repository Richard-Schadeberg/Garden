using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexImages : MonoBehaviour
{
    public Image[] flags;
    public Image occupier;
    public GameObject hexagon;
    public void reflectState(CellState state) {
        occupier.gameObject.SetActive(!state.Water && state.wetness != Wetness.Dry && state.shadiness == Shadiness.Sunny);
    }
}
