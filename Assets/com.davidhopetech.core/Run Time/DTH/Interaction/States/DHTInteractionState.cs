using System;
using com.davidhopetech.core.Run_Time.DTH.ServiceLocator;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.davidhopetech.core.Run_Time.DHTInteraction
{

	[Serializable]
	abstract class DHTInteractionState : MonoBehaviour
	{
		internal DHTInput _input = null;
		public   bool     grabStarted;
		public   bool     grabStopped;

		protected DHTUpdateDebugValue1Event   _debugValue1Event;
		protected DHTUpdateDebugTeleportEvent _teleportEvent;
		protected DHTUpdateDebugMiscEvent     _debugMiscEvent;
		protected DHTEventService     EventService ;
		protected DHTPlayerController Controller;

		private bool _lastIsGrabbing;


		public void UpdateState()
		{
			
			setGrabFlags();
			UpdateStateImpl();
		}

		public abstract void UpdateStateImpl();


		internal void Awake()
		{
			EventService = DHTServiceLocator.DhtEventService;
			Controller   = GetComponent<DHTPlayerController>();

			_input = new DHTInput();

			_debugValue1Event = EventService.dhtUpdateDebugValue1Event;
			_teleportEvent    = EventService.dhtUpdateDebugTeleportEvent;
			_debugMiscEvent   = EventService.dhtUpdateDebugMiscEvent;
		}

		
		private void OnEnable()
		{
			UnityEngine.Debug.Log("State Enabled");
			_input.Enable();
			_input.InitialActionMap.Grab.started  += OnGrabStarted;
			_input.InitialActionMap.Grab.canceled += OnGrabCanceled;
		}

		
		private void OnDisable()
		{
			UnityEngine.Debug.Log("State Disabled");
			//_input.Disable();
			_input.InitialActionMap.Grab.started  -= OnGrabStarted;
			_input.InitialActionMap.Grab.canceled -= OnGrabCanceled;
		}


		private void OnGrabStarted(InputAction.CallbackContext context)
		{
			UnityEngine.Debug.Log("Grab Started");
			_input._isGrabing = true;
		}


		private void OnGrabCanceled(InputAction.CallbackContext context)
		{
			UnityEngine.Debug.Log("Grab Canceled");
			_input._isGrabing = false;
		}

		
		public void setGrabFlags()
		{
			grabStarted     = (_input._isGrabing && !_lastIsGrabbing);
			grabStopped     = (!_input._isGrabing && _lastIsGrabbing);
			_lastIsGrabbing = _input._isGrabing;
		}
	}
}

