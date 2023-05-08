using System;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Serialization;

namespace com.davidhopetech.core.Run_Time.DTH.Interaction.States
{
	[Serializable]
	class DHTInteractionStateGrabbing : DHTInteractionState
	{
		internal DHTGrabable      GrabedItem;
		internal GameObject       Interactor;
		internal GameObject       MirrorHand;
		private  ParentConstraint _parentConstraint;

		
		private void Start()
		{
			// DebugMiscEvent.Invoke("Grabbing State");
			MirrorHand = Controller._rightMirrorHand;
			var rb = MirrorHand.GetComponent<Rigidbody>();
			rb.isKinematic = false;
			
			_parentConstraint = MirrorHand.GetComponent<ParentConstraint>();
			var cs = new ConstraintSource();
			cs.sourceTransform = GrabedItem.transform;
			cs.weight          = 0f;
			_parentConstraint.SetSource(1, cs);
			_parentConstraint.constraintActive = true;
		}

		
		public override void UpdateStateImpl()
		{
			// DebugValue1Event.Invoke(_input.GrabValue().ToString());
			if (grabStopped)
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
			
			cs0.weight = 1.0f - _input.GrabValue();
			cs1.weight = _input.GrabValue();

			_parentConstraint.SetSource(0, cs0);
			_parentConstraint.SetSource(1, cs1);
		}

		void ApplyHandForce()
		{
			var dist  = MirrorHand.transform.position - GrabedItem.transform.position;
			var accel = dist * Controller.handSpringCoeeff;
			var ab    = GrabedItem.GetComponentInParent<ArticulationBody>();
			
			var loc = GrabedItem.transform.position;
			ab.AddForceAtPosition(accel, loc, ForceMode.Force);
		}
		
		
		private void ChangeToIdleState()
		{
			// Debug.Log("######  Change to Idle State  ######");

			_parentConstraint.constraintActive = false;
			Controller._dhtInteractionState = Controller.gameObject.AddComponent<DHTInteractionStateIdle>();
			Destroy(this);
		}
	}
}

