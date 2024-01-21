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
	//[SerializeField] private GameObject ExceptionScreenGO;
	[SerializeField] private TMP_Text ExceptionScreenTMPText;
	
	private string                 log;
	private DHTExceptionLogService service;

	
	void Start()
	{
		service                = DHTServiceLocator.Get<DHTExceptionLogService>();

		if (ExceptionScreenTMPText == null)
		{
			ExceptionScreenTMPText = GetComponentInChildren<TMP_Text>();
		}

		service.LogEvent.AddListener(AddLogEntry);
		// InitLog();
		ExceptionScreenTMPText.text = log;
		
		//GenerateException();
	}

	private int count;

	private void Update()
	{
	}
	
	void GenerateExceptionsAtInterval()
	{
		count++;
		if (count % 90 == 0)
		{
			GenerateException();
		}
	}

	void GenerateException()
	{
		string str = null;
		var    a   = str.Length;
	}


	void InitLog()
	{
		foreach (var logEntry in service.log)
		{
			AddLogEntry(logEntry);
		}
	}

	void OnNewException()
	{
		
	}
	
	void AddLogEntry(DHTExceptionLogService.LogEntry logEntry)
	{
		//log                         += logEntry.stackTrace;
		ExceptionScreenTMPText.text += $"{logEntry.condition}\n{logEntry.stackTrace}\n\n";
	}


	private void OnDisable()
	{
		service.LogEvent.RemoveListener(AddLogEntry);
	}
}
