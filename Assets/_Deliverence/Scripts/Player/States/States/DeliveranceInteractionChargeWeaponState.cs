using System;
using System.Linq;
using com.davidhopetech.core.Run_Time.DTH.Interaction.States;
using com.davidhopetech.core.Run_Time.DTH.Scripts;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

namespace com.davidhopetech.core.Run_Time.Scripts.Interaction.States
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

