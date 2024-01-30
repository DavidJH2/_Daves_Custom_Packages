using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

public class DHTLogService : MonoBehaviour
{
    public UnityEvent<string> LogEvent = new();


    public void Log(string message)
    {
        StackTrace stackTrace = new StackTrace(true);
        StackFrame frame      = stackTrace.GetFrame(1);
        string     fileName   = frame.GetFileName();
        int        lineNumber = frame.GetFileLineNumber();

        string formattedMessage = $"{message} (at {fileName}:{lineNumber})";

        Debug.Log(message);
        LogEvent.Invoke(message + "\n");
    }
}
