using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickDisplay : MonoBehaviour
{
    public Text textObj;
    public Image checkBackground;
    public Image checkMark;
    public void SetText(string text) {
        textObj.text = text;
    }
    public void SetCheck(bool presence) {
        textObj.gameObject.SetActive(presence);
    }
}
