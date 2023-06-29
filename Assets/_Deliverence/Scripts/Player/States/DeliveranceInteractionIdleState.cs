using System;
using System.Linq;
using com.davidhopetech.core.Run_Time.DTH.Interaction.States;
using com.davidhopetech.core.Run_Time.DTH.Scripts;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

namespace com.davidhopetech.core.Run_Time.Scripts.Interaction.States
{
	[Serializable]
	public class DeliveranceInteractionIdleState : DeliveranceInteractionState
	{
		protected override void UpdateStateImpl()
		{
			if (MirrorHand.triggerPulledThisFrame)
			{
				Controller.TriggerPulled();
			}
			// FindClosestInteractor();
		}


		private new void Awake()
		{
			base.Awake();
		}


		protected override void StartExt()
		{
		}


		void FindClosestInteractor()
		{
			var interactorPos = MirrorHand.target.transform.position;
			var interactables = Controller.Interactables;

			if (interactables.Count == 0)
			{
				return;
			}

			var orderedInteractables = interactables.OrderBy(o => o.Dist(interactorPos));

			var interactable = orderedInteractables.First();


			if (interactable.InRange(interactorPos))
			{
				DebugMiscEvent.Invoke($"Closest Interactable: {interactable.gameObject.name}");

				if (MirrorHand.IsGrabbing && interactable is DHTGrabable grabable)
				{
					ChangeToGrabbingState(grabable);
					return;
				}
			}
			else
			{
				DebugMiscEvent.Invoke($"Not In Grab Range");
			}
		}


		private void ChangeToGrabbingState(DHTGrabable grabable)
		{
			Debug.Log("######  Change to Grabbing State  ######");
			DebugValue1Event.Invoke("###  Change to Grabbing State  ###");
			var MirrorHandGO = MirrorHand.gameObject;

			DeliveranceInteractionIdleState component = Controller.gameObject.AddComponent<DeliveranceInteractionIdleState>();
			component.MirrorHand        = MirrorHand;
			component.selfHandle        = selfHandle;
			selfHandle.InteractionState = component;

			Destroy(this);
		}
	}
}

