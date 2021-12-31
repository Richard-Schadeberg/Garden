using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIOccupier : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData) {
        MainUI.S.SelectOccupier(this);
    }
    public OccupierInformation displayObj;
    public string occupierName;
    public OccupierConditions conditions;
}
