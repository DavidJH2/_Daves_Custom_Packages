using System;
using com.davidhopetech.core.Run_Time.DTH.Interaction;
using com.davidhopetech.core.Run_Time.DTH.ServiceLocator;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.davidhopetech.core.Run_Time.DHTInteraction
{

	[Serializable]
	abstract class DHTInteractionState : MonoBehaviour
	{
		protected DHTUpdateDebugMiscEvent     DebugMiscEvent;
		protected DHTUpdateDebugTeleportEvent TeleportEvent;
		protected DHTUpdateDebugValue1Event   DebugValue1Event;
		
		protected DHTEventService             EventService ;
		protected DHTPlayerController         Controller;


		internal void Awake()
		{
			Controller   = GetComponent<DHTPlayerController>();
			// Input = new DHTInput();

			EventService = DHTServiceLocator.DhtEventService;
			
			DebugMiscEvent   = EventService.dhtUpdateDebugMiscEvent;
			TeleportEvent    = EventService.dhtUpdateDebugTeleportEvent;
			DebugValue1Event = EventService.dhtUpdateDebugValue1Event;
		}


		public void UpdateState()
		{
			UpdateStateImpl();
		}

		public abstract void UpdateStateImpl();
	}
}

