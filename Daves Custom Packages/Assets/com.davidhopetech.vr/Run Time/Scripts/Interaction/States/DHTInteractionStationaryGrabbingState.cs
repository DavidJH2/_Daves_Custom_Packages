using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Serialization;

namespace com.davidhopetech.vr.Run_Time.Scripts.Interaction.States
{
	[Serializable]
	public class DHTInteractionStationaryGrabbingState : DHTInteractionState
	{
		[FormerlySerializedAs("GrabedItem")] public  DHTStationaryGrabable      grabedItem;
		private  ParentConstraint _parentConstraint;
		internal Transform        GrabItemInitialTransform;

		
		protected override void StartExt()
		{
			// DebugMiscEvent.Invoke("Grabbing State");
			var rb = MirrorHandGO.GetComponent<Rigidbody>();
			rb.isKinematic = false;
			
			_parentConstraint = MirrorHandGO.GetComponent<ParentConstraint>();
			var cs = new ConstraintSource();
			cs.sourceTransform = grabedItem.transform;
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
			var interactorPos = MirrorHand.xrControler.transform.position;

			var dist  = interactorPos - grabedItem.transform.position;
			var accel = dist * playerController.handSpringCoeeff;
			var rb    = grabedItem.GetComponentInParent<Rigidbody>();
			
			var loc = grabedItem.transform.position;
			if (rb == null)
			{
				Debug.Log($"Object '{grabedItem.name}' missing RigidBody");
				return;
			}
			
			rb.AddForceAtPosition(accel, loc, ForceMode.Force);
		}
		
		
		private void ChangeToIdleState()
		{
			Debug.Log("######  Change to Idle State  ######");
			DebugValue1Event.Invoke("###  Change to Idle State  ###");

			_parentConstraint.constraintActive = false;
			
			DHTInteractionIdleState component = playerController.gameObject.AddComponent<DHTInteractionIdleState>();
			component.selfHandle = selfHandle;
			component.MirrorHand = MirrorHand;
			
			selfHandle.InteractionState        = component;
			
			MirrorHandGO.GetComponent<ParentConstraint>().enabled = false;
			MirrorHandGO.EnableAllColliders();
			
			Destroy(this);
		}
	}
}

