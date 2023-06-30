using System.Collections.Generic;
using System.Linq;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using com.davidhopetech.core.Run_Time.Scripts.Interaction.States;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace _Deliverence.Scripts.Player
{
	public class DeliverancePlayerController : MonoBehaviour
	{
		//[SerializeField] internal InputDeviceCharacteristics controllerCharacteristics;
		public                    GameObject                  grenadeLauncerGO;
		[SerializeField] internal DeliveranceInteractionState leftHandInitialInteractionState;
		[SerializeField] internal DeliveranceInteractionState rightHandInitialInteractionState;
		[SerializeField] internal GameObject                  leftMirrorHand;
		[SerializeField] internal GameObject                  rightMirrorHand;
		[SerializeField] private  TextMeshProUGUI             debugTMP;


		internal List<DHTInteractable>          Interactables;
		internal DeliveranceInteractionStateRef LeftHandInteractionStateRef;
		internal DeliveranceInteractionStateRef RightHandInteractionStateRef;

		private XROrigin     _xrOrgin;
		public ZombieTarget _target;


		private void Awake()
		{
			_xrOrgin = GetComponent<XROrigin>();
			_target  = GetComponentInChildren<ZombieTarget>();
		}

		void Start()
		{
			RightHandInteractionStateRef                = new DeliveranceInteractionStateRef(rightHandInitialInteractionState);
			rightHandInitialInteractionState.MirrorHand = rightMirrorHand.GetComponent<MirrorHand>();
			rightHandInitialInteractionState.selfHandle = RightHandInteractionStateRef;

			LeftHandInteractionStateRef                = new DeliveranceInteractionStateRef(leftHandInitialInteractionState);
			leftHandInitialInteractionState.MirrorHand = leftMirrorHand.GetComponent<MirrorHand>();
			leftHandInitialInteractionState.selfHandle = LeftHandInteractionStateRef;

			Interactables = FindObjectsOfType<DHTInteractable>().ToList();

			Debug.Log($"Number of Grabables: {Interactables.Count}");
		}
		
		public void SetVRMode(TMP_Dropdown dropdown)
		{
			switch (dropdown.value)
			{
				case 0:
					_xrOrgin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
					break;
				case 1:
					_xrOrgin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
					break;
			}
		}

		// UpdateStateImpl is called once per frame
		void FixedUpdate()
		{
			RightHandInteractionStateRef.InteractionState.FixedUpdateState();
			LeftHandInteractionStateRef.InteractionState.FixedUpdateState();

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
		}
		
		public void TriggerPulled()
		{
			var gl = grenadeLauncerGO.GetComponent<DHTInteractable>();
			gl.Activate();
		}
	}
}
