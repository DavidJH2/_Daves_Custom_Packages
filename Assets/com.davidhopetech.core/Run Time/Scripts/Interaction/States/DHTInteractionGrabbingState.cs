using System;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using com.davidhopetech.core.Run_Time.Scripts.Interaction.States;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Serialization;

namespace com.davidhopetech.core.Run_Time.DTH.Interaction.States
{
	[Serializable]
	class DHTInteractionGrabbingState : DHTInteractionState
	{
		internal DHTGrabable      GrabedItem;
		private  ParentConstraint _parentConstraint;

		
		protected override void StartExt()
		{
			// DebugMiscEvent.Invoke("Grabbing State");
			var rb = MirrorHandGO.GetComponent<Rigidbody>();
			rb.isKinematic = false;
			
			_parentConstraint = MirrorHandGO.GetComponent<ParentConstraint>();
			var cs = new ConstraintSource();
			cs.sourceTransform = GrabedItem.transform;
			cs.weight          = 0f;
			_parentConstraint.SetSource(1, cs);
			_parentConstraint.constraintActive = true;
			_parentConstraint.rotationAxis     = Axis.None;
		}


		protected override void UpdateStateImpl()
		{
			// DebugValue1Event.Invoke(_input.GrabValue().ToString());
			if (MirrorHand.grabStopped)
			{
				ChangeToIdleState();
			}

			AdjustParentConstraint();
			ApplyHandForce();
		}


		void AdjustParentConstraint()
		{
			var cs0 = _parentConstraint.GetSource(0);
			var cs1 = _parentConstraint.GetSource(1);

			var grab = MirrorHand.GrabValue;
			cs0.weight = 1.0f - grab;
			cs1.weight = grab;

			_parentConstraint.SetSource(0, cs0);
			_parentConstraint.SetSource(1, cs1);
		}

		
		void ApplyHandForce()
		{
			var interactorPos = MirrorHand.target.transform.position;

			var dist  = interactorPos - GrabedItem.transform.position;
			var accel = dist * Controller.handSpringCoeeff;
			var rb    = GrabedItem.GetComponentInParent<Rigidbody>();
			
			var loc = GrabedItem.transform.position;
			rb.AddForceAtPosition(accel, loc, ForceMode.Force);
		}
		
		
		private void ChangeToIdleState()
		{
			Debug.Log("######  Change to Idle State  ######");
			DebugValue1Event.Invoke("###  Change to Idle State  ###");

			_parentConstraint.constraintActive = false;
			
			DHTInteractionIdleState component = Controller.gameObject.AddComponent<DHTInteractionIdleState>();
			component.selfHandle = selfHandle;
			component.MirrorHand = MirrorHand;
			
			selfHandle.InteractionState        = component;
			
			MirrorHandGO.GetComponent<ParentConstraint>().enabled = false;
			MirrorHandGO.EnableAllColliders();
			
			Destroy(this);
		}
	}
}

