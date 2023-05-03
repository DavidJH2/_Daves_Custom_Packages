using System;
using UnityEngine;
using System.Linq;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using UnityEngine.InputSystem;

[Serializable]
class DHTInteractionStateIdle : DHTInteractionState
{
	internal DHTPlayerInput              _input = null;
	
	private  DHTUpdateDebugValue1Event   _debugValue1;
	private  DHTUpdateDebugTeleportEvent _teleportEvent;
	private  bool                        _isGrabing;


	private new void Awake()
	{
		base.Awake();

		_input         = new DHTPlayerInput();
		_debugValue1   = EventService.dhtUpdateDebugValue1Event;
		_teleportEvent = EventService.dhtUpdateDebugTeleportEvent;
	}


	private void Start()
	{
		_teleportEvent.Invoke("");
	}

	private void OnEnable()
	{
		Debug.Log("Idle State: Input Enabled");
		_input.Enable();

		_teleportEvent.Invoke("Hello");
	}


	private void OnDisable()
	{
		Debug.Log("Idle State: Input Disabled");
		_input.Disable();
	}

	
	private void StartedGrabbing()
	{
		Debug.Log("Started Grabbing");
		_isGrabing = true;
	}

	private void StopedGrabbing()
	{
		Debug.Log("Stopped Grabbing");
		_isGrabing = false;
	}

	
	public override void UpdateState()
	{
		var grab = _input.InitialActionMap.Grab.ReadValue<float>();

		if (_isGrabing)
		{
			if (grab < GripThreshold)
			{
				StopedGrabbing();
			}
		}
		else
		{
			if (grab >= GripThreshold)
			{
				StartedGrabbing();
			}
		}
		DistancesToInteractors(Controller._rightInteractor);
	}

	
	void DistancesToInteractors(GameObject interactor)
	{
		var closeGrabables = Controller._grabables.Where((grabable, index) =>
		{
			var dist = Vector3.Distance(grabable.transform.position, interactor.transform.position);
			return (dist <= grabable.grabRadius);
		});

		if (closeGrabables.Count() > 0)
		{
			_debugValue1.Invoke($"In Grab Range");
			
			if (_isGrabing)
			{
				ChangeToGrabbingState();
			}
		}
		else
		{
			_debugValue1.Invoke($"Not In Grab Range");
		}
	}

	private void ChangeToGrabbingState()
	{
		Debug.Log("######  Change to Grabbing State  ######");

		Controller._dhtInteractionState = Controller.gameObject.AddComponent<DHTInteractionStateGrabbing>();
		Destroy(this);
	}
}

