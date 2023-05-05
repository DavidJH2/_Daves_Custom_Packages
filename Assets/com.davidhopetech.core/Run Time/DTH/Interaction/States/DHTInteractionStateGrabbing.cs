using System;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using UnityEngine.InputSystem;

namespace com.davidhopetech.core.Run_Time.DTH.Interaction.States
{
	[Serializable]
	class DHTInteractionStateGrabbing : DHTInteractionState
	{
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
			UnityEngine.Debug.Log("######  Change to Idle State  ######");

			Controller._dhtInteractionState = Controller.gameObject.AddComponent<DHTInteractionStateIdle>();
			Destroy(this);
		}
	}
}

