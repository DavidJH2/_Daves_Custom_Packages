using System;
using UnityEngine;
using System.Linq;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using UnityEngine.InputSystem;

[Serializable]
class DHTInteractionStateIdle : DHTInteractionState
{
	private  DHTUpdateDebugValue1Event   _debugValue1;
	private  DHTUpdateDebugTeleportEvent _teleportEvent;
	internal DHTPlayerInput              _input = null;


	private void Awake()
	{
		_input      = new DHTPlayerInput();
		_controller = GetComponent<DHTPlayerController>();
		
		var eventContainer = FindObjectOfType<DHTEventContainer>().GetComponent<DHTEventContainer>();
		_debugValue1   = eventContainer.dhtUpdateDebugValue1Event;
		_teleportEvent = eventContainer.dhtUpdateDebugTeleportEvent;
	}

	private void OnEnable()
	{
		Debug.Log("Input Enabled");
		_input.Enable();
		_input.InitialActionMap.Teleport.performed += Teleport;
	}

	
		
	private void Teleport(InputAction.CallbackContext context)
	{
		_teleportEvent.Invoke(context.ReadValue<float>().ToString());
	}

	public override void UpdateState()
	{
		DistancesToInteractors(_controller._rightInteractor);
	}

	void DistancesToInteractors(GameObject interactor)
	{
		float radius = 0;
		var closeGrabables = _controller._grabables.Where((grabable, index) =>
		{
			var dist = Vector3.Distance(grabable.transform.position, interactor.transform.position);

			radius = grabable.grabRadius;
			return (dist <= grabable.grabRadius);
		});


		if (closeGrabables.Count() > 0)
		{
			if (_controller.grabAction.inProgress)
			{
				_debugValue1.Invoke($"Grabbing");
			}
			else
			{
				_debugValue1.Invoke($"In Grab Range");
			}
		}
		else
		{
			_debugValue1.Invoke($"Not In Grab Range");
		}
	}

		
	private void OnDisable()
	{
		_input.Disable();
	}
}

