using System;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using UnityEngine;
using UnityEngine.Animations;

namespace com.davidhopetech.core.Run_Time.DTH.Interaction.States
{
	[Serializable]
	class DHTInteractionStateGrabbing : DHTInteractionState
	{
		public  DHTGrabable      grabedItem;
		private GameObject       rightHand;
		private ParentConstraint parentConstraint;

		
		private void Start()
		{
			_debugMiscEvent.Invoke("Grabbing State");
			rightHand = Controller._rightMirrorHand;
			var rb = rightHand.GetComponent<Rigidbody>();
			rb.isKinematic = false;
			
			parentConstraint = rightHand.GetComponent<ParentConstraint>();
			var cs = new ConstraintSource();
			cs.sourceTransform = grabedItem.transform;
			cs.weight          = 0f;
			parentConstraint.SetSource(1, cs);
			parentConstraint.constraintActive = true;
		}

		
		public override void UpdateStateImpl()
		{
			_debugValue1Event.Invoke(_input.GrabValue().ToString());
			if (grabStopped)
			{
				ChangeToIdleState();
			}

			var cs0 = parentConstraint.GetSource(0);
			var cs1 = parentConstraint.GetSource(1);
			
			cs0.weight = 1.0f - _input.GrabValue();
			cs1.weight = _input.GrabValue();

			parentConstraint.SetSource(0, cs0);
			parentConstraint.SetSource(1, cs1);
		}

	
		private void ChangeToIdleState()
		{
			Debug.Log("######  Change to Idle State  ######");

			parentConstraint.constraintActive = false;
			Controller._dhtInteractionState = Controller.gameObject.AddComponent<DHTInteractionStateIdle>();
			Destroy(this);
		}
	}
}

