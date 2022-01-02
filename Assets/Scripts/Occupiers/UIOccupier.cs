using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIOccupier : Occupier, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        MainUI.S.SelectOccupier(this);
    }
}
