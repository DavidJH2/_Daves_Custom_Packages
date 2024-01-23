using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DHTExceptionScreen : MonoBehaviour
{
	[SerializeField] private bool     ShowTrace = false;
	[SerializeField] private TMP_Text ExceptionScreenTMPText;
	
	private string                 log;
	private DHTExceptionLogService service;

	private void Awake()
	{
		if (ExceptionScreenTMPText == null)
		{
			ExceptionScreenTMPText = GetComponentInChildren<TMP_Text>();
		}

		ExceptionScreenTMPText.text = "";
	}

	void Start()
	{
		service = DHTServiceLocator.Get<DHTExceptionLogService>();
		service.LogEvent.AddListener(AddLogEntry);
	}

	void GenerateException()
	{
		string str = null;
		var    a   = str.Length;
	}


	void AddLogEntry(DHTExceptionLogService.LogEntry logEntry)
	{
		var message = $"{logEntry.condition}\n";
		
		if (ShowTrace)
		{
			message += $"{logEntry.stackTrace}\n";
		}
		message                     += "\n";
		
		ExceptionScreenTMPText.text += message;
	}


	private void OnDisable()
	{
		service.LogEvent.RemoveListener(AddLogEntry);
	}
}
