using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DTHLogService : MonoBehaviour
{
    //[SerializeField] private GameObject LogScreenGO;
    [SerializeField] private TMP_Text   LogScreenTMPText;

    
    void Awake()
    {
        if (LogScreenTMPText == null)
        {
            LogScreenTMPText = GetComponentInChildren<TMP_Text>();
        }

        LogScreenTMPText.text = "";
    }


    public void Log(string message)
    {
        LogScreenTMPText.text += message;
    }
}
