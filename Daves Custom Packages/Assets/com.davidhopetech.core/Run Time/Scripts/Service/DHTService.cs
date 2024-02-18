using System;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using com.davidhopetech.core.Run_Time.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

public class DHTService<T> : MonoBehaviour where T : DHTService<T>
{
    internal virtual void Awake()
    {
        // DHTDebug.LogTag($"Service '{typeof(T).Name}' Awake   <--------");

        var service = DHTServiceLocator.Get<T>();
        if(service != this)
        {
            var msg = $"Service '{typeof(T).Name}' already exist, Destroying...";
            DHTDebug.LogTag(msg);
            Destroy(gameObject);
        }
    }
}
