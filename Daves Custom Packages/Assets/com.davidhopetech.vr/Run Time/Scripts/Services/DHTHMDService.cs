using System;
using System.Collections.Generic;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using com.davidhopetech.core.Run_Time.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;

public class DHTHMDService : DHTService<DHTHMDService>
{
	public UnityEvent<bool> UserPresenceEvent  = new UnityEvent<bool>();
	public UnityEvent       HMDFirstMountEvent = new UnityEvent();
	
	[SerializeField] internal InputActionProperty loggingToggle;

#if true
	private InputDevice inputDevice ;
	private Action      state;
	private bool        lastHmdMounted = true;

	private                  DHTLogService logService;
	[SerializeField] private bool          loggingState = true;

	private float lastButtonValuel  = -1f;
	private bool  HMDHasBeenUpdated = false;


	public bool hmdMounted
	{
		get => GetHMDMounted();
	}
	void Start()
	{
		// DHTDebug.LogTag($"Service '{typeof(DHTHMDService).Name}' Starting   <--------");
		logService = DHTServiceLocator.Get<DHTLogService>();
		SetState(FindHMD);
	}
	
	
	void SetState(Action newState)
	{
		state = newState;
	}

	
	void Update()
	{
		state.Invoke();
	}

	void FindHMD()
	{
		var inputDevices = new List<InputDevice>();
		InputDevices.GetDevices(inputDevices);

		foreach (var device in inputDevices)
		{
			if (device.characteristics.HasFlag(InputDeviceCharacteristics.HeadMounted))
			{
				inputDevice = device;
				
				SetState(UpdateHMDUserPresence);
				HMDFirstMountEvent.Invoke();
				//UpdateHMDUserPresence();
			}
		}
	}


	void UpdateHMDUserPresence()
	{
		UpdateLoggingState();
		if (!HMDHasBeenUpdated)
		{
			HMDHasBeenUpdated = true;
			UserPresenceEvent.Invoke(hmdMounted);
			lastHmdMounted = hmdMounted;
		}
		else if (hmdMounted != lastHmdMounted)
		{
			UserPresenceEvent.Invoke(hmdMounted);
			lastHmdMounted = hmdMounted;
		}
	}

	void UpdateLoggingState()
	{
		var buttonValue = loggingToggle.action.ReadValue<float>();
		if (buttonValue != lastButtonValuel)
			if(buttonValue >0.9f)
				loggingState = !loggingState;
	}

	bool GetHMDMounted()
	{
		Vector3 velocity;
		
		inputDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out velocity);
		// if(loggingState) logService.Log($"HMD Velocity: {velocity}\t\tlastHmdMounted = {lastHmdMounted}");
		var hmdMounted =  (velocity != Vector3.zero);
		
		return hmdMounted;
	}
#else
	private void Start()
	{
		// Subscribe to the session state change event
		OpenXRRuntime.wantsToQuit    += OnSessionEnding;
		OpenXRRuntime.wantsToRestart += OnSessionRestarting;
	}

	private void OnDestroy()
	{
		// Unsubscribe from the session state change event
		OpenXRRuntime.wantsToQuit    -= OnSessionEnding;
		OpenXRRuntime.wantsToRestart -= OnSessionRestarting;
	}

	private bool OnSessionEnding()
	{
		UserPresenceEvent.Invoke(false);     // HMD removed
		
		Debug.Log("OpenXR session is ending");
		// Return true to allow the session to end, or false to prevent it
		return true;
	}

	private bool OnSessionRestarting()
	{
		UserPresenceEvent.Invoke(true);      // HMD put on
		
		Debug.Log("OpenXR session is restarting");
		// Return true to allow the session to restart, or false to prevent it
		return true;
	}    
#endif
}
