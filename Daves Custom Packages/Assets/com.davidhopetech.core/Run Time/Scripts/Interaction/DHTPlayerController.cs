using System;
using System.Collections.Generic;
using System.Linq;
using com.davidhopetech.core.Run_Time.DTH.Interaction;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using com.davidhopetech.core.Run_Time.Scripts.Interaction.States;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR;

namespace com.davidhopetech.core.Run_Time.Scripts.Interaction
{
	public class DHTPlayerController : MonoBehaviour
	{
		//[SerializeField] internal InputDeviceCharacteristics controllerCharacteristics;
		[SerializeField]     internal DHTInteractionState leftHandInitialInteractionState;
		[SerializeField]     internal DHTInteractionState rightHandInitialInteractionState;
		[SerializeField]     internal GameObject          leftMirrorHand;
		[SerializeField]     internal GameObject          rightMirrorHand;
		[SerializeField]     internal float               handSpringCoeeff; // TODO: Move these to Settings Scriptable Object
		[SerializeField]     internal float               handDampCoeeff;
		[FormerlySerializedAs("dthJoystick")] [SerializeField] private  DHTJoystick         dhtJoystick;

		// private                   InputDevice                targetDevice;


		public List<DHTInteractable>  Interactables;
		internal DHTInteractionStateRef LeftHandInteractionStateRef;
		internal DHTInteractionStateRef RightHandInteractionStateRef;

		private XROrigin _xrOrgin;

		private void Awake()
		{
			_xrOrgin = GetComponent<XROrigin>();
		}

		void Start()
		{
			var rightHandInteractionState = gameObject.AddComponent<DHTInteractionIdleState>();
			RightHandInteractionStateRef         = new DHTInteractionStateRef(rightHandInteractionState);
			rightHandInteractionState.MirrorHand = rightMirrorHand.GetComponent<MirrorHand>();
			rightHandInteractionState.selfHandle = RightHandInteractionStateRef;

			var leftHandInteractionState = gameObject.AddComponent<DHTInteractionIdleState>();
			LeftHandInteractionStateRef         = new DHTInteractionStateRef(leftHandInteractionState);
			leftHandInteractionState.MirrorHand = leftMirrorHand.GetComponent<MirrorHand>();
			leftHandInteractionState.selfHandle = LeftHandInteractionStateRef;

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
		void Update()
		{
			RightHandInteractionStateRef.InteractionState.UpdateState();
			LeftHandInteractionStateRef.InteractionState.UpdateState();

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
		}
	}
}
