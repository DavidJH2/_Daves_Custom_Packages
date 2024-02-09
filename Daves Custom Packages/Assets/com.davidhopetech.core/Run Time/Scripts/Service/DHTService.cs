using System;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using com.davidhopetech.core.Run_Time.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

public class DHTService<T> : MonoBehaviour where T : DHTService<T>
{
    void Awake()
    {
        if(DHTServiceLocator.IsServiceRegistered<T>())
        {
            DHTDebug.LogTag($"Service '{typeof(T).Name}' already exist, Destroying...");
            Destroy(gameObject);
        }
    }
}
