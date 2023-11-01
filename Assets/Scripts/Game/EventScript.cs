using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript : MonoBehaviour
{
    static string[] events = new string[] { "Invert" };
    static public string StartEvent()
    {
        return events[Random.Range(0, events.Length)];
    }
}
