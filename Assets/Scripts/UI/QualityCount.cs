using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QualityCount : MonoBehaviour {
    public Text counter;
    public static QualityCount S;
    void Start() {
        S = this;
    }
    public void SetQuality(int n) {
        counter.text = n.ToString();
    }
}
