using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] private DebugPanelElement[] elements;
    [SerializeField] private bool                _resetOnStart = true;

    private void Start()
    {
        var service                     = DHTServiceLocator.Get<DHTDebugPanelService>();
        if(service) service.debugPanel1 = this;

        if (_resetOnStart) ResetOnStart();
    }

    private void ResetOnStart()
    {
        foreach (var element in elements)
        {
            element.Reset();
        }
    }

    public void SetElement(int elemNum, string newValue)
    {
        var element = elements[elemNum]; //
        element.Set(newValue);
    }

    public void SetElement(int elemNum, string newLabel, string newValue)
    {
        var element = elements[elemNum];
        element.Set(newLabel, newValue);
    }
}
