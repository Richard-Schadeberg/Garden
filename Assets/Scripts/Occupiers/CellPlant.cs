using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellPlant : CellContents
{
    public Wetness[] illegalWetness;
    public Shadiness[] illegalShadiness;
    public bool canSurvive(Season season,Wetness wetness,Shadiness shadiness) {
        foreach (Wetness iter in illegalWetness) {
            if (iter == wetness) return false;
            if (iter == Wetness.HotDry  &&  season.HotSeason() && wetness == Wetness.Dry) return false;
            if (iter == Wetness.ColdWet && !season.HotSeason() && wetness == Wetness.Wet) return false;
        }
        foreach (Shadiness iter in illegalShadiness) {
            if (iter == shadiness) return false;
            if (iter == Shadiness.HotSunny &&  season.HotSeason() && shadiness == Shadiness.Sunny) return false;
            if (iter == Shadiness.ColdDark && !season.HotSeason() && shadiness == Shadiness.Dark)  return false;
        }
        return true;
    }
}