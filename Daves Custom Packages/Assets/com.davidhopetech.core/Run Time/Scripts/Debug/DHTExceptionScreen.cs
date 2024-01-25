using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using TMPro;
using UnityEngine;

public class DHTExceptionScreen : MonoBehaviour
{
	[SerializeField] private bool     ShowTrace = false;
	[SerializeField] private TMP_Text ExceptionScreenTMPText;
	
	private string                 log;
	private DHTExceptionService service;

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
		service = DHTServiceLocator.Get<DHTExceptionService>();
		service.LogEvent.AddListener(AddLogEntry);
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
	}


	private void OnDisable()
	{
		service.LogEvent.RemoveListener(AddLogEntry);
	}
}
