using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DHTEventBase<T> : MonoBehaviour
{
    public UnityEvent<T> Event = new();
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
