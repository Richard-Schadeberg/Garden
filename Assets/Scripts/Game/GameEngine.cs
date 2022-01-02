using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine
{
    public static GameEngine S;
    void Start() {
        GameEngine.S = this;
    }  
}
