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
		 var interactables = Controller.Interactables;

		 var orderedInteractables = interactables.OrderBy(o => o.Dist(interactorPos));
		 var interactable = orderedInteractables.First();

		 if (interactable.InRange(interactorPos))
		 {
			 DebugMiscEvent.Invoke($"Closest Interactable: {interactable.gameObject.name}");
			 AdjustParentConstraint();
		 }
		 else
		 {
			 ChangeToIdleState();
		 }
	 }


	 void AdjustParentConstraint()
	 {
		 var cs0 = _parentConstraint.GetSource(0);
		 var cs1 = _parentConstraint.GetSource(1);

		 var grab = MirrorHand.GrabValue;

		 grab = 1.0f;
		 
		 TeleportEvent.Invoke($"Grab: {grab}");
		 
		 cs0.weight = 1.0f - grab;
		 cs1.weight = grab;

		 _parentConstraint.SetSource(0, cs0);
		 _parentConstraint.SetSource(1, cs1);
	 }


	 private void ChangeToIdleState()
	 {
		 Debug.Log("######  Change to Idle State  ######");
		 DebugValue1Event.Invoke("###  Change to Idle State  ###");

		 _parentConstraint.constraintActive = false;
		 Controller.InteractionState        = Controller.gameObject.AddComponent<DHTInteractionIdleState>();

		 MirrorHandGO.GetComponent<ParentConstraint>().enabled = false;
		 MirrorHandGO.EnableAllColliders();

		 Destroy(this);
	 }
 }

