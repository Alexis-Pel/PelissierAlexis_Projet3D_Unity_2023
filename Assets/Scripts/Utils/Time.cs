using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Globalization;

public class TimeScript : MonoBehaviour
{
    private float time = 0f;
    public Stopwatch stopwatch = new();
    private bool isRunning = false;

    public float TimeGetSet
    {
        get { return time; }
        set { time = value; }
    }

    static public float PlayerPrefTime
    {
        get { return PlayerPrefs.GetFloat("time"); }
        set { PlayerPrefs.SetFloat("time", value); }
    }

    void Update()
    {
        if (isRunning)
        {
            TimeGetSet += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        isRunning = true;
        stopwatch.Start();
    }

    public void StopTimer()
    {
        isRunning = false;
        stopwatch.Stop();
    }

    /// <summary>
    /// Transform a string to float
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    static public float GetValueTime(string value)
    {
        return float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
    }

    /// <summary>
    /// Make a float a human readable time
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    static public string GetFormattedTime(float time)
    {
        string minutes = Mathf.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        string miliseconds = (time * 100 % 100).ToString("00");

        return string.Format("{0}:{1}:{2}", minutes, seconds, miliseconds);
    }

}
