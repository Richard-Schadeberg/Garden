using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPath : UIOccupier
{
    public GameObject stonePrefab;
    public override void OnPlacement(HexCell cell) {
        GameObject newObject = new GameObject();
        CellOccupier occupier = MakeCellOccupier(newObject);
        occupier.CopyFrom(this);
        cell.SetOccupier(occupier);
        GameObject stone = Instantiate(stonePrefab,occupier.gameObject.transform);
        stone.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        cell.hexImage.DisplayOccupier(null);
        occupier.GetComponent<CellPath>().stonePrefab = stonePrefab;
    }
    public override CellOccupier MakeCellOccupier(GameObject newObject) {
        return newObject.AddComponent(typeof(CellPath)) as CellPath;
    }
}
