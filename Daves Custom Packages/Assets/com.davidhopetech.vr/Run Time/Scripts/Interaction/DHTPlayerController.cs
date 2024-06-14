using System;
using System.Collections.Generic;
using System.Linq;
using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.vr.Run_Time.Scripts.Interaction.States;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

namespace com.davidhopetech.vr.Run_Time.Scripts.Interaction
{
	public class DHTPlayerController : MonoBehaviour
	{
		//[SerializeField] internal InputDeviceCharacteristics controllerCharacteristics;
		[SerializeField] internal DHTInteractionState  leftHandInitialInteractionState;
		[SerializeField] internal DHTInteractionState  rightHandInitialInteractionState;
		[SerializeField] internal GameObject           leftMirrorHand;
		[SerializeField] internal GameObject           rightMirrorHand;
		[SerializeField] internal float                handSpringCoeeff; // TODO: Move these to Settings Scriptable Object
		[SerializeField] internal float                handDampCoeeff;
		[SerializeField] private  DHTJoystick          dhtJoystick;
		[SerializeField] internal float                throwMultiplyer = 2.0f;
		[SerializeField] private  InputActionReference _quitActon;

		// private                   InputDevice                targetDevice;


		public   List<DHTInteractable>  Interactables;
		internal DHTInteractionStateRef LeftHandInteractionStateRef;
		internal DHTInteractionStateRef RightHandInteractionStateRef;

		private XROrigin _xrOrgin;


		private void OnEnable()
		{
			if (_quitActon)
			{
				_quitActon.action.Enable();
				_quitActon.action.performed += Quit;
			}
			else
			{
				Debug.LogWarning("quit action reference is not set.");
			}
		}


		private void Awake()
		{
			_xrOrgin = ObjectExtentions.DHTFindObjectOfType<XROrigin>();
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

			Interactables = FindObjectsByType<DHTInteractable>(FindObjectsSortMode.None).ToList();

			// Debug.Log($"Number of Grabables: {Interactables.Count}");
		}


		public void OnHoverEntered(HoverEnterEventArgs args)
		{
			var a = args.interactableObject;
		}

		public void OnActivate(ActivateEventArgs args)
		{

		}


		public void OnSelectEnter(SelectEnterEventArgs args)
		{
			var a = args.interactableObject;
		}


		public void OnfocusExited(FocusExitEventArgs args)
		{

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
		}


		void Quit(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				Application.Quit();
			}
		}


		private void OnDisable()
		{
			_quitActon.action.Disable();
		}
	}
}
