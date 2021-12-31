using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionIndicator : MonoBehaviour
{
    public Image[] backings;
    public void DisplayConditions(bool[] conditions) {
        for (int i=0;i<5;i++) {
            if (conditions[i]) {
                backings[i].color = Color.green;
            } else {
                backings[i].color = Color.red;
            }
        }
    }
}
