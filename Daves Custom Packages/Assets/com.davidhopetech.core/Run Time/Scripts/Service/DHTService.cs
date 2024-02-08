using System;
using com.davidhopetech.core.Run_Time.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

public class DHTService<T> : MonoBehaviour where T : Object
{
    void Awake()
    {
        if(FindObjectsOfType<T>().Length > 1)
        {
            DHTDebug.LogTag($"Service '{typeof(T).Name}' already exist, Destroying...");
            Destroy(gameObject);
        }
    }
}
