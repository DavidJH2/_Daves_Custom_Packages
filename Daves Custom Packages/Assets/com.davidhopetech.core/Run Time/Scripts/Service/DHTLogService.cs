
using UnityEngine;
using UnityEngine.Events;


public class DHTLogService : DHTService<DHTLogService>
{
    public UnityEvent<string> LogEvent = new();


    public void Log(string message)
    {
        LogEvent.Invoke(message + "\n");
    }
}
