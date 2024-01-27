using System;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DHTExceptionScreen : MonoBehaviour
{
	[SerializeField] private bool     ShowTrace  = false;
	[SerializeField] private bool     ResetOnRun = true;
	[SerializeField] private TMP_Text ExceptionScreenTMPText;

	[SerializeField] private ExceptionScrollViewDragHandler _scrollRect;

	private                  string              log;
	private                  DHTExceptionService service;


	private void Awake()
	{
		if (ExceptionScreenTMPText == null)
		{
			ExceptionScreenTMPText = GetComponentInChildren<TMP_Text>();
		}

		if (ResetOnRun) ExceptionScreenTMPText.text = "";
	}

	void Start()
	{
		if (_scrollRect == null) _scrollRect = GetComponentInChildren<ExceptionScrollViewDragHandler>();
		service = DHTServiceLocator.Get<DHTExceptionService>();
		if (service) service.LogEvent.AddListener(AddLogEntry);
	}


	private int count = 0;
	private void Update()
	{
		/*
		if(count%2==0) ExceptionScreenTMPText.text += $"Count = {count}\n";
		_scrollRect.UpdatePos();
		count++;
		*/
	}
	

	void GenerateException()
	{
		string str = null;
		var    a   = str.Length;
	}


	void AddLogEntry(DHTExceptionService.LogEntry logEntry)
	{
		var message = $"{logEntry.condition}\n";
		
		if (ShowTrace)
		{
			message += $"{logEntry.stackTrace}\n";
		}
		message                     += "\n";
		
		ExceptionScreenTMPText.text += message;
		_scrollRect.UpdatePos();
	}


	private void OnDisable()
	{
		service.LogEvent.RemoveListener(AddLogEntry);
	}
}
