using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public static MainUI S;
    public OccupierInformation occupierDisplay;
    void Awake() {
        S = this;
    }
    public void SelectOccupier(UIOccupier occupier) {
        if (occupierDisplay.occupier==null) {
            occupierDisplay.DisplayOccupier(occupier);
        } else {
            occupierDisplay.Reset();
        }
    }
}
