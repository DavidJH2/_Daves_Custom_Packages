using System;
using UnityEngine;
using System.Linq;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using com.davidhopetech.core.Run_Time.DTH.Interaction.States;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using UnityEngine.InputSystem;

[Serializable]
class DHTInteractionIdleState : DHTInteractionState
{
	public override void UpdateStateImpl()
	{
		FindClosestInteractor(Controller._rightInteractor);
	}

	
	private new void Awake()
	{
		base.Awake();
	}


	private void Start()
	{
	}


	void FindClosestInteractor(GameObject interactor)
	{
		var intoractorPos = interactor.transform.position;
		var interactables = Controller._Interactables;

		var orderedInteractables = interactables.OrderBy(o => o.Dist(intoractorPos));

		var interactable = orderedInteractables.First();

		if (interactable.InRange(intoractorPos))
		{
			DebugMiscEvent.Invoke($"Closest Interactable: {interactable.gameObject.name}");
		}
		else
		{
			DebugMiscEvent.Invoke($"Not In Grab Range");
		}

		if (_isGrabbing && interactable is DHTGrabable grabable)
		{
			ChangeToGrabbingState(grabable);
		}
	}

	private void ChangeToGrabbingState(DHTGrabable grabable)
	{
		// Debug.Log("######  Change to Grabbing State  ######");

		DHTInteractionGrabbingState component = Controller.gameObject.AddComponent<DHTInteractionGrabbingState>();
		component.GrabedItem            = grabable;
		component.Interactor            = Controller._rightInteractor;
		component.MirrorHand            = Controller._rightMirrorHand;
		Controller._dhtInteractionState = component;
		
		Destroy(this);
	}
}

