using System;
using com.davidhopetech.core.Run_Time.Scripts.Service;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using com.davidhopetech.core.Run_Time.Utils;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace com.davidhopetech.vr.Run_Time.Scripts.Interaction.States
{
	[Serializable]
	public class DHTInteractionGrabbingState : DHTInteractionState
	{
		public  DHTGrabable      grabedItem;
		private ParentConstraint _parentConstraint;
		private ParentConstraint _grabableParentConstraint;

		internal Transform        GrabItemInitialTransform;

		const         float GRAB_TIME      = 0.05f;
		private const float HALF_GRAB_TIME = GRAB_TIME / 2;

		private DHTEventService         eventService;
		private DHTDebugPanel_1_Service debugService;
		private UnityEvent<GameObject>  _grabableReleasedEvent;
		protected override void StartExt()
		{
			eventService = DHTServiceLocator.Get<DHTEventService>();
			_grabableReleasedEvent = (eventService.Get<DHTGrabableReleasedEvent>())._event;
			debugService = DHTServiceLocator.Get<DHTDebugPanel_1_Service>();
			// DebugMiscEvent.Invoke("Grabbing State");
			var rb = MirrorHandGO.GetComponent<Rigidbody>();
			rb.isKinematic = false;
			_grabableParentConstraint = grabedItem.GetComponent<ParentConstraint>();
			InitializeControllerConstraint();
		}

		void InitializeControllerConstraint()
		{
			_parentConstraint = MirrorHandGO.GetComponent<ParentConstraint>();
			var cs = new ConstraintSource();
			cs.sourceTransform = GrabItemInitialTransform;
			cs.weight          = 0f;
			_parentConstraint.SetSource(1, cs);
			_parentConstraint.constraintActive = true;
			_parentConstraint.rotationAxis     = Axis.None;
		}

		private float grabTimer = 0;

		enum GrabState
		{
			ReachOut,
			BringBack,
			Holding
		}

		GrabState _grabState;
		
		protected override void UpdateStateImpl()
		{
			// DebugValue1Event.Invoke(_input.GrabValue().ToString());
			if (MirrorHand.grabStopped)
			{
				ChangeToIdleState();
				var rigidbody =  grabedItem.GetComponent<Rigidbody>();
				var velocity  = MirrorHand.velocityBuffer[5];
				
				debugService.SetElement(6, $"Velocity = {velocity}", "");
				
				rigidbody.velocity = velocity * playerController.throwMultiplyer;
				_grabableReleasedEvent.Invoke(grabedItem.gameObject);
			}

			switch (_grabState)
			{
				case GrabState.ReachOut:
					grabTimer += Time.deltaTime;
					if (grabTimer > GRAB_TIME/2)
					{
						InitializeGrabableConstraint();
						_grabState = GrabState.BringBack;
					}
					else
					{
						AdjustParentConstraint(grabTimer/HALF_GRAB_TIME);
					}
					break;
				case GrabState.BringBack:
					grabTimer += Time.deltaTime;
					if (grabTimer > GRAB_TIME)
					{
						_grabState = GrabState.Holding;
						AdjustParentConstraint(0.0f);
						Destroy(GrabItemInitialTransform.gameObject);
					}
					else
					{
						AdjustParentConstraint((GRAB_TIME-grabTimer)/HALF_GRAB_TIME);
					}
					break;
				case GrabState.Holding:
					break;
				default:
					break;
			}

			ApplyHandForce();
		}

		void InitializeGrabableConstraint()
		{
			if (!_grabableParentConstraint)
			{
				DHTDebug.Log($"Game Object '{grabedItem.name}' needs a ParentConstraint");
				return;
			}

			var cs = new ConstraintSource();
			cs.sourceTransform = MirrorHand.transform;
			cs.weight          = 1f;
			_grabableParentConstraint.SetSource(0, cs);
			_grabableParentConstraint.constraintActive = true;
			_grabableParentConstraint.rotationAxis     = Axis.None;
		}


		void AdjustParentConstraint(float value)
		{
			var cs0 = _parentConstraint.GetSource(0);
			var cs1 = _parentConstraint.GetSource(1);

			cs0.weight = 1.0f - value;
			cs1.weight = value;

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
			_grabableParentConstraint.constraintActive = false;

			
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

