using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using TMPro;
using UnityEngine;

namespace com.davidhopetech.core.Run_Time.DTHDebug
{
	public class DHTXRDebug : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI value1;
		[SerializeField] private TextMeshProUGUI teleportValue;
		[SerializeField] private TextMeshProUGUI miscValue;

		private DHTUpdateDebugValue1Event   _debugValue1;
		private DHTUpdateDebugTeleportEvent _debugTeleportEvent;
		private DHTUpdateDebugMiscEvent     _debugMiscEvent;
	
		
		void Start()
		{
			var eventService = DHTServiceLocator.dhtEventService;
			
			eventService.dhtUpdateDebugValue1Event.AddListener(UpdateValue1);
			eventService.dhtUpdateDebugTeleportEvent.AddListener(UpdateTeleportValue);
			eventService.dhtUpdateDebugMiscEvent.AddListener(UpdateMiscValue);
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
