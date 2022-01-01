using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public static MainUI S;
    public OccupierInformation occupierDisplay;
    Occupier currentOccupier;
    void Awake() {
        S = this;
    }
    public void SelectOccupier(UIOccupier occupier) {
        if (occupierDisplay.occupier==occupier) {
            occupierDisplay.Reset();
            currentOccupier = null;
        } else {
            occupierDisplay.DisplayOccupier(occupier);
            currentOccupier = occupier;
        }
    }
    public void CellClicked(HexCell cell) {
        if (currentOccupier!=null) {
            currentOccupier.OnPlacement(cell);
        }
    }
}
