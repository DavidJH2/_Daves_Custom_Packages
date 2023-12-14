using System;
using System.Linq;
using com.davidhopetech.core.Run_Time.Scripts.Interaction.States;
using UnityEngine;

namespace _Deliverence.Scripts.Player.States.States
{
	[Serializable]
	public class DeliveranceInteractionMenuState : DeliveranceInteractionState
	{
		protected override void UpdateStateImpl()
		{
		}


		private new void Awake()
		{
			base.Awake();
		}


		protected override void StartExt()
		{
		}


		void FindClosestInteractor()
		{
			var interactorPos = MirrorHand.target.transform.position;
			var interactables = Controller.Interactables;

			if (interactables.Count == 0)
			{
				return;
			}

			var orderedInteractables = interactables.OrderBy(o => o.Dist(interactorPos));

			var interactable = orderedInteractables.First();


			if (interactable.InRange(interactorPos))
			{
			}
			else
			{
			}
		}


		private void ChangeToIdleState()
		{
			Debug.Log("######  Change to Idle State  ######");
			DebugValue1Event.Invoke("###  Change to Idle State  ###");

			DeliveranceInteractionIdleState component = Controller.gameObject.AddComponent<DeliveranceInteractionIdleState>();
			component.MirrorHand        = MirrorHand;
			component.selfHandle        = selfHandle;
			selfHandle.InteractionState = component;

			Destroy(this);
		}
	}
}

