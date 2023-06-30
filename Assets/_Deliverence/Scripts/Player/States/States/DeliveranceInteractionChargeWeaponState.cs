using System;
using com.davidhopetech.core.Run_Time.Scripts.Interaction.States;
using UnityEngine;

namespace _Deliverence.Scripts.Player.States.States
{
	[Serializable]
	public class DeliveranceInteractionChargeWeaponState : DeliveranceInteractionState
	{
		GrenadeLauncer granadeLauncer;

		
		protected override void UpdateStateImpl()
		{
			if (!MirrorHand.TriggerPulled)
			{
				LaunchGrenade();
				ChangeToIdleState();
			}

			granadeLauncer.ChargeGrenade();
		}

		
		private void LaunchGrenade()
		{
			granadeLauncer._particleSystem.Stop();
			granadeLauncer.Activate();
		}


		private new void Awake()
		{
			base.Awake();
		}


		protected override void StartExt()
		{
			granadeLauncer = Controller.grenadeLauncerGO.GetComponent<GrenadeLauncer>();
			granadeLauncer.ResetParticleSystem();
			granadeLauncer._particleSystem.Play();
		}


		private void ChangeToIdleState()
		{
			Debug.Log("######  Change to Grabbing State  ######");
			DebugValue1Event.Invoke("###  Change to Grabbing State  ###");

			DeliveranceInteractionIdleState component = Controller.gameObject.AddComponent<DeliveranceInteractionIdleState>();
			component.MirrorHand        = MirrorHand;
			component.selfHandle        = selfHandle;
			selfHandle.InteractionState = component;

			Destroy(this);
		}
	}
}

