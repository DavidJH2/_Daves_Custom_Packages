using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DHTLogService : MonoBehaviour
{
    public UnityEvent<string> LogEvent = new();


    public void Log(string message)
    {
        Debug.Log(message);
        LogEvent.Invoke(message + "\n");
    }
}
