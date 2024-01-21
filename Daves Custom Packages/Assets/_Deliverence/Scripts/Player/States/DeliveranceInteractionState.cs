using System;
using _Deliverence.Scripts.Player;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using com.davidhopetech.vr.Run_Time.Scripts;
using UnityEngine;

namespace com.davidhopetech.core.Run_Time.Scripts.Interaction.States
{

	[Serializable]
	public abstract class DeliveranceInteractionState : MonoBehaviour
	{
		protected DHTUpdateDebugMiscEvent     DebugMiscEvent;
		protected DHTUpdateDebugTeleportEvent TeleportEvent;
		protected DHTUpdateDebugValue1Event   DebugValue1Event;

		protected DHTEventService             dhtEventService;
		protected DeliverancePlayerController Controller;

		public   MirrorHand MirrorHand;
		internal GameObject MirrorHandGO;

		public   DeliveranceInteractionStateRef selfHandle;
		internal Animator                       handAnimator;

		
		public void Awake()
		{
			Controller      = GetComponent<DeliverancePlayerController>();
			// dhtEventService = DHTServiceLocator.dhtEventService;
			dhtEventService = DHTServiceLocator.Get<DHTEventService>();

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


		public void FixedUpdateState()
		{
			UpdateStateImpl();
		}

		protected abstract void UpdateStateImpl();
	}
}

