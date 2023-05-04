using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using UnityEngine.InputSystem;
using UnityEngine.XR;

[Serializable]
class DHTInteractionStateGrabbing : DHTInteractionState
{
	private  IEnumerable<DHTGrabable>    closeGrabables;
	private  bool                        _isGrabing;

	private new void Awake()
	{
		base.Awake();
	}

	private void OnEnable()
	{
		// Debug.Log("Grabbing State: Input Enabled");
		_input.Enable();
	}


	private void OnDisable()
	{
		// Debug.Log("Grabbing State: Input Disabled");
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
		
		ChangeToIdleState();
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

		_teleportEvent.Invoke($"StartedGrabbing = {grab.ToString()}\nIsGrabbing: {_isGrabing.ToString()}");
		// DistancesToInteractors(Controller._rightInteractor);
	}

	
	void DistancesToInteractors(GameObject interactor)
	{
		float radius = 0;
		closeGrabables = Controller._grabables.Where((grabable, index) =>
		{
			var dist = Vector3.Distance(grabable.transform.position, interactor.transform.position);

			radius = grabable.grabRadius;
			return (dist <= grabable.grabRadius);
		});

		if (closeGrabables.Count() > 0)
		{
			if (_input.InitialActionMap.Grab.inProgress)
			{
				_debugValue1Event.Invoke($"StartedGrabbing");
			}
			else
			{
				_debugValue1Event.Invoke($"In Grab Range");
			}
		}
		else
		{
			_debugValue1Event.Invoke($"Not In Grab Range");
		}
	}

	
	private void ChangeToIdleState()
	{
		Debug.Log("######  Change to Idle State  ######");

		Controller._dhtInteractionState = Controller.gameObject.AddComponent<DHTInteractionStateIdle>();
		Destroy(this);
	}
}

