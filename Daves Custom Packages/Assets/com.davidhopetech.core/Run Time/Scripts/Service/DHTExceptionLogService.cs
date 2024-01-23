using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DHTExceptionLogService : MonoBehaviour
{
    public UnityEvent<LogEntry> LogEvent = new();
    public struct LogEntry
    {
        public string condition;
        public string stackTrace;
        public LogType type;

        public LogEntry(string newCondition, string newStackTrace, LogType newType)
        {
            condition  = newCondition;
            stackTrace = newStackTrace;
            type       = newType;
        }
    }

    public List<LogEntry> log = new();
    
    void Awake()
    {
        Application.logMessageReceived += LogCallback;
    }

    void LogCallback(string condition, string stackTrace, LogType type)
    {
        var newLogEntry = new LogEntry(condition, stackTrace, type); 
        log.Add(newLogEntry);
        LogEvent.Invoke(newLogEntry);
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= LogCallback;
    }
}
