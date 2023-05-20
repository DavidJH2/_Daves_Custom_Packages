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

		protected DHTEventService     dhtEventService;
		protected DHTPlayerController Controller;

		internal MirrorHand MirrorHand;
		internal GameObject MirrorHandGO;

		internal DHTInteractionStateRef selfHandle;
		internal Animator               handAnimator;

		
		internal void Awake()
		{
			Controller   = GetComponent<DHTPlayerController>();
			dhtEventService = DHTServiceLocator.dhtEventService;

			DebugMiscEvent   = dhtEventService.dhtUpdateDebugMiscEvent;
			TeleportEvent    = dhtEventService.dhtUpdateDebugTeleportEvent;
			DebugValue1Event = dhtEventService.dhtUpdateDebugValue1Event;
		}


		private void Start()
		{
			MirrorHandGO = MirrorHand.gameObject;
			handAnimator = MirrorHandGO.GetComponentInChildren<Animator>();
			StartExt();
		}

		protected abstract void StartExt();


		public void UpdateState()
		{
			UpdateStateImpl();
		}

		protected abstract void UpdateStateImpl();
	}
}

