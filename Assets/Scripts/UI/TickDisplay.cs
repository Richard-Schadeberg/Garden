using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickDisplay : MonoBehaviour
{
    public void ShowGoal(OccupierGoal goal) {
        textObj.text = goal.description;
        checkMark.gameObject.SetActive(goal.completed);
    }
    public Text textObj;
    public Image checkBackground;
    public Image checkMark;
}