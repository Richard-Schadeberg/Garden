using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HexImages : MonoBehaviour
{
    public Image[] flags;
    public Image occupierSprite;
    public GameObject hexagon;
    public Image[] borderlines;
    public HexCell hexCell;
    public void BlockNeighbour(HexDirection direction) {
        BorderLine(direction).gameObject.SetActive(true);
    }
    public void UnblockNeighbour(HexDirection direction) {
        BorderLine(direction).gameObject.SetActive(false);
    }
    public void UnblockNeighbours() {
        foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection))) {
            UnblockNeighbour(direction);
        }
    }
    Image BorderLine(HexDirection direction) {
        return borderlines[(int)direction];
    }
    public void DisplayOccupier(Occupier occupier) {
        if (occupier==null) {
            occupierSprite.gameObject.SetActive(false);
        } else {
            occupierSprite.gameObject.SetActive(true);
            occupierSprite.sprite = occupier.cellImage;
        }
    }
    public void FlagIncomplete() {
        flags[1].gameObject.SetActive(true);
    }
    public void FlagComplete() {
        flags[1].gameObject.SetActive(false);
    }
}
