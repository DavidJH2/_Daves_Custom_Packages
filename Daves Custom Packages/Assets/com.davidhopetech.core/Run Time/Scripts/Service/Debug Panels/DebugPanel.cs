using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] private DebugPanelElement[] elements;


    private void Start()
    {
        var service                     = DHTServiceLocator.Get<DHTDebugPanelService>();
        if(service) service.debugPanel1 = this;
    }

    public void SetElement(int elemNum, string newValue)
    {
        var elem = elements[elemNum];
        elem.Set(newValue);
    }

    public void SetElement(int elemNum, string newLabel, string newValue)
    {
        var elem = elements[elemNum];
        elem.Set(newLabel, newValue);
    }
}
