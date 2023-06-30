using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    private static TextMeshProUGUI debugTMP;
    
    void Start()
    {
        var debugTextGO = FindObjectOfType<DebugText>();
        debugTMP = debugTextGO.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static void AddDebugText(string text)
    {
        debugTMP.text += text;
    }


    public static void SetDebugText(string text)
    {
        debugTMP.text = text;
    }
}
