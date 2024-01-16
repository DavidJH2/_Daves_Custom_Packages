using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugPanelElement : MonoBehaviour
{
    public TMP_Text label;
    public TMP_Text value;
    
    
    public void Set(string newValue)
    {
        value.text = newValue;
    }

    public void Set(string newLabel, string newValue)
    {
        label.text = newLabel;
        value.text = newValue;
    }
}
