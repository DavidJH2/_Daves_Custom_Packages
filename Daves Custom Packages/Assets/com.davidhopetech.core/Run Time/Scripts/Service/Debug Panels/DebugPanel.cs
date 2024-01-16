using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] private DebugPanelElement[] elements;

    private void Start()
    {
        var element = GetComponentsInChildren<DebugPanelElement>();
        elements =  elements.Concat(element).ToArray();
    }

    public void SetElement(int elemNum,  string newValue)
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
