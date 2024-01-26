using System.Collections;
using System.Collections.Generic;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DHTLogScreen : MonoBehaviour
{
	[SerializeField] private TMP_Text LogScreenTMPText;

	private DHTLogService service;

	void Awake()
	{
		if (LogScreenTMPText == null)
		{
			LogScreenTMPText = GetComponentInChildren<TMP_Text>();
		}

		LogScreenTMPText.text = "";
	}

	
	void Start()
	{
		service = DHTServiceLocator.Get<DHTLogService>(); 
		if(service) service.LogEvent.AddListener(Log);
	}


	public void Log(string message)
	{
		if(LogScreenTMPText) LogScreenTMPText.text += message;
	}


	private void OnDisable()
	{
		service.LogEvent.RemoveListener(Log);
	}
}
