using com.davidhopetech.core.Run_Time.DTH.ServiceLocator;
using UnityEngine;
using TMPro;


namespace com.davidhopetech.core.Run_Time.Debug
{
	public class DHTXRDebug : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI value1;
		[SerializeField] private TextMeshProUGUI teleportValue;

		private DHTUpdateDebugValue1Event   _debugValue1;
		private DHTUpdateDebugTeleportEvent _debugTeleportEvent;
	
		void Start()
		{
			var eventContainer = DHTServiceLocator.DhtEventService;
			
			_debugValue1 = eventContainer.dhtUpdateDebugValue1Event;
			_debugValue1.AddListener(UpdateValue1);

			_debugTeleportEvent = eventContainer.dhtUpdateDebugTeleportEvent;
			_debugTeleportEvent.AddListener(UpdateTeleportValue);
		}

		private void UpdateTeleportValue(string text)
		{
			teleportValue.text = text;
		}

		public void UpdateValue1(string text)
		{
			value1.text = text;
		}


		void Update()
		{
		
		}
	}
}
