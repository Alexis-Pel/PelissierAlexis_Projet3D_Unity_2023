using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score
{
    static public int PlayerPrefScore
    {
        get { return PlayerPrefs.GetInt("score"); }
        set { PlayerPrefs.SetInt("score", value); }
    }
}
