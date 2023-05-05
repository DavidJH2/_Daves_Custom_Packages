using System;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using UnityEngine;

namespace com.davidhopetech.core.Run_Time.DTH.Interaction.States
{
	[Serializable]
	class DHTInteractionStateGrabbing : DHTInteractionState
	{
		private void Start()
		{
			_debugMiscEvent.Invoke("Grabbing State");
		}

		public override void UpdateStateImpl()
		{
			_debugValue1Event.Invoke(_input.GrabValue().ToString());
			if (grabStopped)
			{
				ChangeToIdleState();
			}
				
		}

	
		private void ChangeToIdleState()
		{
			Debug.Log("######  Change to Idle State  ######");

			Controller._dhtInteractionState = Controller.gameObject.AddComponent<DHTInteractionStateIdle>();
			Destroy(this);
		}
	}
}

