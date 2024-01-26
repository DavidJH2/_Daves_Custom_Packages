using System;
using com.davidhopetech.core.Run_Time.Scripts.Service;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.Events;

namespace com.davidhopetech.vr.Run_Time.Scripts.Interaction.States
{

	[Serializable]
	public abstract class DHTInteractionState : MonoBehaviour
	{
		protected UnityEvent<string> DebugMiscEvent;
		protected UnityEvent<string> TeleportEvent;
		protected UnityEvent<string> DebugValue1Event;

		protected DHTEventService     dhtEventService;
		protected DHTPlayerController Controller;

		public   MirrorHand MirrorHand;
		internal GameObject MirrorHandGO;

		public DHTInteractionStateRef selfHandle;
		internal Animator               handAnimator;

		
		public void Awake()
		{
			Controller      = GetComponent<DHTPlayerController>();
			// dhtEventService = DHTServiceLocator.dhtEventService;
			dhtEventService = DHTServiceLocator.Get<DHTEventService>();

			DebugMiscEvent   = dhtEventService.Get<DHTUpdateDebugMiscEvent>()?._event;
			TeleportEvent    = dhtEventService.Get<DHTUpdateDebugTeleportEvent>()?._event;
			DebugValue1Event = dhtEventService.Get<DHTUpdateDebugMiscEvent>()?._event;
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

