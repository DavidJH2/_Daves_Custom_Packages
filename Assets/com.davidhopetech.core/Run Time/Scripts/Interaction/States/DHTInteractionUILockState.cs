using System;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Serialization;

namespace com.davidhopetech.core.Run_Time.DTH.Interaction.States
{
	[Serializable]
	class DHTInteractionUILockState : DHTInteractionState
	{
		internal DHTGrabable      GrabedItem;
		internal GameObject       Interactor;
		internal GameObject       MirrorHandGO;
		internal MirrorHand       MirrorHand;
		private  ParentConstraint _parentConstraint;

		
		private void Start()
		{
			// DebugMiscEvent.Invoke("Grabbing State");
			// MirrorHandGO = null;
			MirrorHand = MirrorHandGO.GetComponent<MirrorHand>();
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
			AdjustParentConstraint();
		}


		void AdjustParentConstraint()
		{
			var cs0 = _parentConstraint.GetSource(0);
			var cs1 = _parentConstraint.GetSource(1);
			
			cs0.weight = 1.0f - MirrorHand.GrabValue;
			cs1.weight = MirrorHand.GrabValue;

			_parentConstraint.SetSource(0, cs0);
			_parentConstraint.SetSource(1, cs1);
		}

		
		private void ChangeToIdleState()
		{
			// Debug.Log("######  Change to Idle State  ######");

			_parentConstraint.constraintActive = false;
			Controller.InteractionState = Controller.gameObject.AddComponent<DHTInteractionIdleState>();
			Destroy(this);
		}
	}
}

