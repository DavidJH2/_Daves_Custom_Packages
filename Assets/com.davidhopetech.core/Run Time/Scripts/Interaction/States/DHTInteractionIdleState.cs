using System;
using System.Linq;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using com.davidhopetech.core.Run_Time.DTH.Interaction.States;
using com.davidhopetech.core.Run_Time.DTH.Scripts;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using UnityEngine;
using UnityEngine.Animations;


[Serializable]
class DHTInteractionIdleState : DHTInteractionState
{
	public override void UpdateStateImpl()
	{
		FindClosestInteractor(Controller.rightMirrorHand.GetComponent<MirrorHand>());
		FindClosestInteractor(Controller.leftMirrorHand.GetComponent<MirrorHand>());
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
				return;
			}

			if (interactable is DHTSpatialLock spatialLock)
			{
				ChangeToSpatialLockState(mirrorHand, spatialLock);
				return;
			}
		}
		else
		{
			DebugMiscEvent.Invoke($"Not In Grab Range");
		}
	}

	
	private void ChangeToGrabbingState(MirrorHand mirrorHand, DHTGrabable grabable)
	{
		Debug.Log("######  Change to Grabbing State  ######");
		DebugValue1Event.Invoke("###  Change to Grabbing State  ###");
		var MirrorHandGO     = mirrorHand.gameObject;
		
		DHTInteractionGrabbingState component = Controller.gameObject.AddComponent<DHTInteractionGrabbingState>();
		component.GrabedItem        = grabable;
		component.Interactor        = mirrorHand.target.gameObject;
		component.MirrorHandGO      = MirrorHandGO;
		Controller.InteractionState = component;
		
		MirrorHandGO.GetComponent<ParentConstraint>().enabled = true;
		MirrorHandGO.DisableAllColliders();
		
		Destroy(this);
	}

	
	private void ChangeToSpatialLockState(MirrorHand mirrorHand, DHTSpatialLock spatialLock)
	{
		Debug.Log("######  Change to Spatial Lock State  ######");
		DebugValue1Event.Invoke("###  Change to Spatial Lock State  ###");


		var MirrorHandGO = mirrorHand.gameObject;
		
		DHTInteractionSpatialLockingState component = Controller.gameObject.AddComponent<DHTInteractionSpatialLockingState>();
		component.SpatialLock        = spatialLock;
		component.Interactor        = mirrorHand.target.gameObject;
		component.MirrorHandGO      = MirrorHandGO;
		
		Controller.InteractionState = component;
		
		MirrorHandGO.GetComponent<ParentConstraint>().enabled = true;
		
		Destroy(this);
	}
}

