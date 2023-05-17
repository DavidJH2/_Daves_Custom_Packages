using System;
using com.davidhopetech.core.Run_Time.DTH.Interaction;
using com.davidhopetech.core.Run_Time.Scripts.Interaction;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
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

		protected DHTEventService     EventService;
		protected DHTPlayerController Controller;

		internal MirrorHand MirrorHand;
		internal GameObject MirrorHandGO;

		internal DHTInteractionStateRef selfHandle;

		
		internal void Awake()
		{
			Controller   = GetComponent<DHTPlayerController>();
			EventService = DHTServiceLocator.dhtEventService;

			DebugMiscEvent   = EventService.dhtUpdateDebugMiscEvent;
			TeleportEvent    = EventService.dhtUpdateDebugTeleportEvent;
			DebugValue1Event = EventService.dhtUpdateDebugValue1Event;
		}


		private void Start()
		{
			MirrorHandGO = MirrorHand.gameObject;
		}

		public void UpdateState()
		{
			UpdateStateImpl();
		}

		public abstract void UpdateStateImpl();
	}
}

