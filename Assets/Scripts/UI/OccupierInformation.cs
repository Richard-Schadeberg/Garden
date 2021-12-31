using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OccupierInformation : MonoBehaviour
{
    public ConditionIndicator wetness,shadiness;
    public Text occupierName;
    public UIOccupier occupier;
    public void SetName(string n) {
        occupierName.text = n;
    }
    public void DisplayOccupier(UIOccupier occupier) {
        wetness.DisplayConditions(occupier.conditions.wetness);
        shadiness.DisplayConditions(occupier.conditions.shadiness);
        }
    public void Reset() {}
}
