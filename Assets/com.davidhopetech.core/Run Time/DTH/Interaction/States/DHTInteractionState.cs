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

		protected DHTUpdateDebugMiscEvent     DebugMiscEvent;
		protected DHTUpdateDebugTeleportEvent TeleportEvent;
		protected DHTUpdateDebugValue1Event   DebugValue1Event;
		
		protected DHTEventService             EventService ;
		protected DHTPlayerController         Controller;

		private bool _lastIsGrabbing;


		public void UpdateState()
		{
			setGrabFlags();
			UpdateStateImpl();
		}

		internal bool _isGrabbing
		{
			get { return _input.GrabValue() > .01; }
		}

		public abstract void UpdateStateImpl();


		internal void Awake()
		{
			EventService = DHTServiceLocator.DhtEventService;
			Controller   = GetComponent<DHTPlayerController>();

			_input = new DHTInput();

			DebugValue1Event = EventService.dhtUpdateDebugValue1Event;
			TeleportEvent    = EventService.dhtUpdateDebugTeleportEvent;
			DebugMiscEvent   = EventService.dhtUpdateDebugMiscEvent;
		}

		
		private void OnEnable()
		{
			// Debug.Log("State Enabled");
			_input.Enable();
		}

		
		private void OnDisable()
		{
			// Debug.Log("State Disabled");
			//_input.Disable();
		}

		
		public void setGrabFlags()
		{
			grabStarted     = (_isGrabbing && !_lastIsGrabbing);
			grabStopped     = (!_isGrabbing && _lastIsGrabbing);

			_lastIsGrabbing = _isGrabbing;
		}
	}
}

