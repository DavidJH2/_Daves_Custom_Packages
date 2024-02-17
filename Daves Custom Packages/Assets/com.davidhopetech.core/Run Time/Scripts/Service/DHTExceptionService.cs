using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DHTExceptionService : DHTService<DHTExceptionService>
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
    
    internal override void Awake()
    {
        base.Awake();
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
