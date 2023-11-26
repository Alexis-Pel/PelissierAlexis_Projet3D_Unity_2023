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
    static public int PlayerPrefScoreInfinite
    {
        get { return PlayerPrefs.GetInt("score_inf"); }
        set { PlayerPrefs.SetInt("score_inf", value); }
    }
}
