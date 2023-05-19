 using System;
using System.Linq;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using com.davidhopetech.core.Run_Time.DTH.Scripts;
 using com.davidhopetech.core.Run_Time.Scripts.Interaction.States;
 using UnityEngine;
using UnityEngine.Animations;


 [Serializable]
 class DHTInteractionSpatialLockingState : DHTInteractionState
 {
	 internal DHTSpatialLock   SpatialLock;
	 private  ParentConstraint _parentConstraint;


	 private new void Awake()
	 {
		 base.Awake();
	 }


	 protected override void StartExt()
	 {
		 _parentConstraint = MirrorHandGO.GetComponent<ParentConstraint>();

		 MirrorHand.active = false;
		 
		 var cs = new ConstraintSource();
		 cs.sourceTransform = SpatialLock.transform;
		 cs.weight          = 0f;
		 _parentConstraint.SetSource(1, cs);
		 _parentConstraint.constraintActive = true;
		 _parentConstraint.rotationAxis     = Axis.X | Axis.Y | Axis.Z;
	 }


	 protected override void UpdateStateImpl()
	 {
		 var interactorPos = MirrorHand.target.transform.position;

		 if (SpatialLock.InRange(interactorPos))
		 {
			 AdjustParentConstraint(interactorPos);
		 }
		 else
		 {
			 ChangeToIdleState();
		 }
	 }


	 void AdjustParentConstraint(Vector3 interactorPos)
	 {
		 var cs0 = _parentConstraint.GetSource(0);
		 var cs1 = _parentConstraint.GetSource(1);

		 var dis           = SpatialLock.Dist(interactorPos);
		 var scope         = SpatialLock.fullLockRadius - SpatialLock.range;
		 var normalizedDis = (dis - SpatialLock.range) / scope;
		 var locking       = Mathf.Clamp( normalizedDis, 0, 1);
		 
		 TeleportEvent.Invoke($"Grab: {locking}");
		 
		 cs0.weight = 1.0f - locking;
		 cs1.weight = locking;

		 _parentConstraint.SetSource(0, cs0);
		 _parentConstraint.SetSource(1, cs1);
	 }


	 private void ChangeToIdleState()
	 {
		 Debug.Log("######  Change to Idle State  ######");
		 DebugValue1Event.Invoke("###  Change to Idle State  ###");

		 handAnimator.SetBool("Near Two Buttons", false);

		 _parentConstraint.constraintActive = false;
		 MirrorHand.active                  = true;
		 
		 DHTInteractionIdleState component = Controller.gameObject.AddComponent<DHTInteractionIdleState>();
		 component.selfHandle   = selfHandle;
		 component.MirrorHand = MirrorHand;
			
		 selfHandle.InteractionState = component;

		 MirrorHandGO.GetComponent<ParentConstraint>().enabled = false;
		 MirrorHandGO.EnableAllColliders();

		 Destroy(this);
	 }
 }

