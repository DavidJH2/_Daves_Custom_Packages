using System;
using System.Linq;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using com.davidhopetech.core.Run_Time.DTH.Interaction.States;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;


[Serializable]
class DHTInteractionIdleState : DHTInteractionState
{
	public override void UpdateStateImpl()
	{
		FindClosestInteractor(Controller.leftMirrorHand.GetComponent<MirrorHand>());
		FindClosestInteractor(Controller.rightMirrorHand.GetComponent<MirrorHand>());
	}

	
	private new void Awake()
	{
		base.Awake();
	}


	private void Start()
	{
	}


	void FindClosestInteractor(MirrorHand mirrorHand)
	{
		var interactor    = mirrorHand.target;
		var interactorPos = interactor.transform.position;
		var interactables = Controller.Interactables;

		var orderedInteractables = interactables.OrderBy(o => o.Dist(interactorPos));

		var interactable = orderedInteractables.First();

		if (interactable.InRange(interactorPos))
		{
			DebugMiscEvent.Invoke($"Closest Interactable: {interactable.gameObject.name}");

			if (mirrorHand.IsGrabbing && interactable is DHTGrabable grabable)
			{
				ChangeToGrabbingState(mirrorHand, grabable);
			}
		}
		else
		{
			DebugMiscEvent.Invoke($"Not In Grab Range");
		}
	}

	private void ChangeToGrabbingState(MirrorHand mirrorHand, DHTGrabable grabable)
	{
		// Debug.Log("######  Change to Grabbing State  ######");

		DHTInteractionGrabbingState component = Controller.gameObject.AddComponent<DHTInteractionGrabbingState>();
		component.GrabedItem            = grabable;
		component.Interactor            = mirrorHand.target.gameObject;
		component.MirrorHandGo            = mirrorHand.gameObject;
		Controller.InteractionState = component;
		
		mirrorHand.gameObject.DisableAllColliders();
		
		Destroy(this);
	}
}

