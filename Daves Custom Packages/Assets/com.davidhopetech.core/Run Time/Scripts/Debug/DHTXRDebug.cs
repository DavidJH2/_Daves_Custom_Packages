using com.davidhopetech.core.Run_Time.Scripts.Service;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace com.davidhopetech.core.Run_Time.DHTDebug
{
	public class DHTXRDebug : MonoBehaviour
	{
		public UnityEventBase unityEvent;
		[SerializeField] private TextMeshProUGUI value1;
		[SerializeField] private TextMeshProUGUI teleportValue;
		[SerializeField] private TextMeshProUGUI miscValue;

		private DHTUpdateDebugValue1Event   _debugValue1;
		private DHTUpdateDebugTeleportEvent _debugTeleportEvent;
		private DHTUpdateDebugMiscEvent     _debugMiscEvent;
	
		
		void Start()
		{
			Debug.unityLogger.Log("This is a Test");	
			// var eventService = DHTServiceLocator.dhtEventService;
			var eventService = DHTServiceLocator.Get<DHTEventService>();
			
			eventService.Get<DHTUpdateDebugValue1Event>()._event.AddListener(UpdateValue1);
			eventService.Get<DHTUpdateDebugTeleportEvent>()._event.AddListener(UpdateTeleportValue);
			
			// ***  New Methode  ***
			eventService.Get<DHTUpdateDebugMiscEvent>()._event.AddListener(UpdateMiscValue);
		}

		
		private void UpdateTeleportValue(string text)
		{
			teleportValue.text = text;
		}

		
		public void UpdateValue1(string text)
		{
			value1.text = text;
		}

		
		private void UpdateMiscValue(string text)
		{
			miscValue.text = text;
		}
	}
}
