 using System;
using System.Linq;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using com.davidhopetech.core.Run_Time.DTH.Scripts;
using UnityEngine;
using UnityEngine.Animations;


 [Serializable]
 class DHTInteractionSpatialLockingState : DHTInteractionState
 {
	 internal DHTSpatialLock   SpatialLock;
	 internal GameObject       Interactor;
	 internal GameObject       MirrorHandGO;
	 internal MirrorHand       MirrorHand;
	 private  ParentConstraint _parentConstraint;


	 private new void Awake()
	 {
		 base.Awake();
	 }


	 private void Start()
	 {
		 MirrorHand                         = MirrorHandGO.GetComponent<MirrorHand>();
		 _parentConstraint                  = MirrorHandGO.GetComponent<ParentConstraint>();

		 MirrorHand.active = false;
		 
		 var cs = new ConstraintSource();
		 cs.sourceTransform = SpatialLock.transform;
		 cs.weight          = 0f;
		 _parentConstraint.SetSource(1, cs);
		 _parentConstraint.constraintActive = true;
		 _parentConstraint.rotationAxis     = Axis.X | Axis.Y | Axis.Z;
	 }


	 public override void UpdateStateImpl()
	 {
		 var interactor    = MirrorHand.target;
		 var interactorPos = interactor.transform.position;
		 
		 /*
		 var interactables = Controller.Interactables;

		 var orderedInteractables = interactables.OrderBy(o => o.Dist(interactorPos));
		 var interactable = orderedInteractables.First();
		 */

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

		 _parentConstraint.constraintActive = false;
		 MirrorHand.active           = true;
		 Controller.InteractionState = Controller.gameObject.AddComponent<DHTInteractionIdleState>();

		 MirrorHandGO.GetComponent<ParentConstraint>().enabled = false;
		 MirrorHandGO.EnableAllColliders();

		 Destroy(this);
	 }
 }

