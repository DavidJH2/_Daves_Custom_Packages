using System;
using com.davidhopetech.core.Run_Time.DTH.ServiceLocator;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.davidhopetech.core.Run_Time.DHTInteraction
{

	[Serializable]
	abstract class DHTInteractionState : MonoBehaviour
	{
		protected DHTUpdateDebugValue1Event   _debugValue1Event;
		protected DHTUpdateDebugTeleportEvent _teleportEvent;
		protected DHTUpdateDebugMiscEvent     _debugMiscEvent;
		
		internal  float                       GripThreshold = .1f;
		internal  DHTPlayerInput              _input = null;
	
		protected DHTEventService     EventService ;
		protected DHTPlayerController Controller;
		

		internal void Awake()
		{
			EventService = DHTServiceLocator.DhtEventService;
			Controller     = GetComponent<DHTPlayerController>();

			_input            = new DHTPlayerInput();
			
			_debugValue1Event                    =  EventService.dhtUpdateDebugValue1Event;
			_teleportEvent                       =  EventService.dhtUpdateDebugTeleportEvent;
			_debugMiscEvent                      =  EventService.dhtUpdateDebugMiscEvent;
			_input.InitialActionMap.Grab.started += TestStarted;
			_input.InitialActionMap.Grab.canceled += TestCanceled;
		}

		
		private void TestStarted(InputAction.CallbackContext context)
		{
			var val = context.ReadValue<float>();
			_debugMiscEvent.Invoke($"Started: {val}");
		}

		
		private void TestCanceled(InputAction.CallbackContext context)
		{
			var val = context.ReadValue<float>();
			_debugMiscEvent.Invoke($"Canceled: {val}");
		}

		public abstract void UpdateState();
	}
}

