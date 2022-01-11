using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public static MainUI S;
    public OccupierInformation occupierDisplay;
    public Occupier currentOccupier;
    public HexGrid hexGrid;
    void Awake() {
        S = this;
    }
    public void SelectOccupier(Occupier occupier) {
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
            CellColour.ColourCell(cell);
        }
        CellEvaluation.GridUpdated(hexGrid);
        if (!(currentOccupier is UIWater || currentOccupier is UIRemover || cell.cellState.Occupier is CellWaterSource)) SelectOccupier(cell.cellState.Occupier);
        if (currentOccupier is UIWater) OccupierInformation.S.DisplayOccupier(currentOccupier);
    }
}
