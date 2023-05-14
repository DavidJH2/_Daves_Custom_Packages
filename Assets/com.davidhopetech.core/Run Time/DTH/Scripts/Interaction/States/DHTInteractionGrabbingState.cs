using System;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Serialization;

namespace com.davidhopetech.core.Run_Time.DTH.Interaction.States
{
	[Serializable]
	class DHTInteractionGrabbingState : DHTInteractionState
	{
		internal DHTGrabable      GrabedItem;
		internal GameObject       Interactor;
		internal GameObject       MirrorHandGO;
		internal MirrorHand       MirrorHand;
		private  ParentConstraint _parentConstraint;

		
		private void Start()
		{
			MirrorHand = MirrorHandGO.GetComponent<MirrorHand>();
			
			// DebugMiscEvent.Invoke("Grabbing State");
			var rb = MirrorHandGO.GetComponent<Rigidbody>();
			rb.isKinematic = false;
			
			_parentConstraint = MirrorHandGO.GetComponent<ParentConstraint>();
			var cs = new ConstraintSource();
			cs.sourceTransform = GrabedItem.transform;
			cs.weight          = 0f;
			_parentConstraint.SetSource(1, cs);
			_parentConstraint.constraintActive = true;
		}

		
		public override void UpdateStateImpl()
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
			var dist  = MirrorHandGO.transform.position - GrabedItem.transform.position;
			var accel = dist * Controller.handSpringCoeeff;
			var rb    = GrabedItem.GetComponentInParent<Rigidbody>();
			
			var loc = GrabedItem.transform.position;
			rb.AddForceAtPosition(accel, loc, ForceMode.Force);
		}
		
		
		private void ChangeToIdleState()
		{
			// Debug.Log("######  Change to Idle State  ######");

			_parentConstraint.constraintActive = false;
			Controller.InteractionState = Controller.gameObject.AddComponent<DHTInteractionIdleState>();
			
			MirrorHandGO.GetComponent<ParentConstraint>().enabled = false;
			MirrorHandGO.EnableAllColliders();
			
			Destroy(this);
		}
	}
}

