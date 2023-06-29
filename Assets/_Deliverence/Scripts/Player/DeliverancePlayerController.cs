using System.Collections.Generic;
using System.Linq;
using com.davidhopetech.core.Run_Time.DTH.Interaction;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using com.davidhopetech.core.Run_Time.Scripts.Interaction.States;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Serialization;

namespace com.davidhopetech.core.Run_Time.Scripts.Interaction
{
	public class DeliverancePlayerController : MonoBehaviour
	{
		//[SerializeField] internal InputDeviceCharacteristics controllerCharacteristics;
		[FormerlySerializedAs("grenadeLauncer")]                 public   GameObject                  grenadeLauncerGO;
		[SerializeField] internal DeliveranceInteractionState leftHandInitialInteractionState;
		[SerializeField] internal DeliveranceInteractionState rightHandInitialInteractionState;
		[SerializeField] internal GameObject                  leftMirrorHand;
		[SerializeField] internal GameObject                  rightMirrorHand;
		[SerializeField] internal float                       handSpringCoeeff; // TODO: Move these to Settings Scriptable Object
		[SerializeField] internal float                       handDampCoeeff;

		[FormerlySerializedAs("dthJoystick")] [SerializeField]
		private DHTJoystick dhtJoystick;

		// private                   InputDevice                targetDevice;


		internal List<DHTInteractable>       Interactables;
		internal DeliveranceInteractionStateRef LeftHandInteractionStateRef;
		internal DeliveranceInteractionStateRef RightHandInteractionStateRef;

		private XROrigin _xrOrgin;

		private void Awake()
		{
			_xrOrgin = GetComponent<XROrigin>();
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
