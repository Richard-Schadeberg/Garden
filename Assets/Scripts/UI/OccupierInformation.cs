using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
public class OccupierInformation : MonoBehaviour
{
    public static OccupierInformation S;
    void Start() {
        S = this;
        Reset();
    }
    public ConditionIndicator wetness,shadiness;
    public Text occupierName;
    public TickDisplay tickObj;
    List<TickDisplay> tickers = new List<TickDisplay>();
    public Occupier occupier;
    public void DisplayOccupier(Occupier newOccupier) {
        Reset();
        if (newOccupier==null) return;
        occupier = newOccupier;
        wetness.gameObject.SetActive(true);
        shadiness.gameObject.SetActive(true);
        occupierName.gameObject.SetActive(true);
        if (occupier is UIWater || occupier is UIPath || occupier is UIRemover) {
            wetness.gameObject.SetActive(false);
            shadiness.gameObject.SetActive(false);
        }
        wetness.DisplayConditions(occupier.conditions.wetness.Skip(1).ToArray());
        shadiness.DisplayConditions(occupier.conditions.shadiness);
        occupierName.text = occupier.occupierName;
        Array.Sort(occupier.goals, (x, y) => y.essential.CompareTo(x.essential));
        DisplayGoals(occupier.goals,newOccupier.occupierName);
    }
    public void Reset() {
        occupier = null;
        wetness.gameObject.SetActive(false);
        shadiness.gameObject.SetActive(false);
        occupierName.gameObject.SetActive(false);
        foreach (TickDisplay tick in tickers) {
            Destroy(tick.gameObject);
        }
        tickers.Clear();
    }
    void DisplayGoals(OccupierGoal[] goals,string namer) {
        int i=0;
        foreach (OccupierGoal goal in goals) {
            TickDisplay ticker = Instantiate(tickObj);
            ticker.gameObject.SetActive(true);
            tickers.Add(ticker);
            ticker.gameObject.transform.SetParent(gameObject.transform);
            ticker.gameObject.transform.position = tickObj.transform.position;
            float offset = i * ticker.gameObject.GetComponent<RectTransform>().rect.height;
            if (!goal.essential) {
                offset += 0.5f * ticker.gameObject.GetComponent<RectTransform>().rect.height;
            }
            ticker.gameObject.transform.position += new Vector3(0,-offset,0);
            ticker.ShowGoal(goal,namer);
            i++;
        }
    }
}
