	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using com.davidhopetech.core.Run_Time.DHTInteraction;
	using Unity.VisualScripting;
	using UnityEngine;
	using UnityEngine.InputSystem;
	using UnityEngine.Serialization;
	using UnityEngine.XR;
	using InputDevice = UnityEngine.XR.InputDevice;

	public class DHTPlayerController : MonoBehaviour
	{
		[SerializeField] internal GameObject                 _rightInteractor;
		[SerializeField] internal InputDeviceCharacteristics controllerCharacteristics;
		
		private                   InputDevice                targetDevice;

		internal List<DHTGrabable>   _grabables;
		internal DHTInteractionState _dhtInteractionState;

		void Start()
		{
			
			_dhtInteractionState = gameObject.AddComponent<DHTInteractionStateIdle>();
			_grabables           = GameObject.FindObjectsOfType<DHTGrabable>().ToList();
			
			Debug.Log($"Number of Grabables: {_grabables.Count}");
			TryInitialize();
		}

		void TryInitialize()
		{
			List<InputDevice> devices = new List<InputDevice>();

			InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
			if (devices.Count > 0)
			{
				targetDevice = devices[0];
			}
		}

		// UpdateStateImpl is called once per frame
		void Update()
		{
			_dhtInteractionState?.UpdateState();
		}
	}
