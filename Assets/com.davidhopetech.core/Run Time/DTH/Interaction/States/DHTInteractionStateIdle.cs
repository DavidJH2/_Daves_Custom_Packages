using System;
using UnityEngine;
using System.Linq;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using com.davidhopetech.core.Run_Time.DTH.Interaction.States;
using UnityEngine.InputSystem;

[Serializable]
class DHTInteractionStateIdle : DHTInteractionState
{
	public override void UpdateStateImpl()
	{
		DistancesToInteractors(Controller._rightInteractor);
	}

	
	private new void Awake()
	{
		base.Awake();
	}


	private void Start()
	{
		_teleportEvent.Invoke("");
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
			_debugValue1Event.Invoke($"In Grab Range");
			
			if (_input._isGrabing)
			{
				ChangeToGrabbingState();
			}
		}
		else
		{
			_debugValue1Event.Invoke($"Not In Grab Range");
		}
	}

	private void ChangeToGrabbingState()
	{
		Debug.Log("######  Change to Grabbing State  ######");

		Controller._dhtInteractionState = Controller.gameObject.AddComponent<DHTInteractionStateGrabbing>();
		Destroy(this);
	}
}

