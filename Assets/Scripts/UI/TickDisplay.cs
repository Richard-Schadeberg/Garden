using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickDisplay : MonoBehaviour
{
    public void ShowGoal(OccupierGoal goal,string occupierName) {
        textObj.text = goal.description;
        checkMark.gameObject.SetActive(goal.completed);
        if (!goal.essential) {
            checkMark.gameObject.SetActive(CellEvaluation.CompletedByAny(occupierName,goal.description));
        }
    }
    public Text textObj;
    public Image checkBackground;
    public Image checkMark;
}