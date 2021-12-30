using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderNumber : MonoBehaviour
{
    public void SetString(float f) {
        GetComponent<Text>().text = f.ToString("0");
    }
}
