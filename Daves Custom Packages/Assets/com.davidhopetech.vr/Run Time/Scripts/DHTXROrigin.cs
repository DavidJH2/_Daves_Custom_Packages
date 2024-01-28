using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof (XROrigin), typeof(TeleportationProvider))]
public class DHTXROrigin : MonoBehaviour
{
	[SerializeField] private bool                  resetPositionOnStart = true;
	[SerializeField] private XROrigin              xrOrigin;
	[SerializeField] private TeleportationProvider teleportationProvider;
	[SerializeField] private GameObject            startOrientation;

	private DHTLogService _logService;

	void OnEnable()
	{
	}
	
	void Start()
	{
		_logService = DHTServiceLocator.Get<DHTLogService>();
		
		HMDInitialization hmdInitialization = GetComponent<HMDInitialization>(); 
		hmdInitialization.onHMDInitialized += HMDInitialized;
		teleportationProvider         = GetComponent<TeleportationProvider>();
		
		if(startOrientation == null) startOrientation = gameObject;
		
		if (xrOrigin == null)
		{
			
			xrOrigin = GetComponent<XROrigin>();
		}
	}


	public void HMDInitialized()
	{
		if (_logService) _logService.Log($"------  {this.GetType().Name}.{MethodBase.GetCurrentMethod().Name} Called  ------");
		if (_logService) _logService.Log($"------  {this.GetType().Name}.{MethodBase.GetCurrentMethod().Name}: resetPositionOnStart = {resetPositionOnStart}  ------");
		if (resetPositionOnStart)
		{
			ResetPosition();
		}
	}

	
	private int resetCount = 0;

	public void ResetPosition()
	{
		resetCount++;
		if (_logService) _logService.Log($"--------  Resetting Position ({resetCount})  ------");
		TeleportRequest request = new TeleportRequest()
		{
			destinationPosition = startOrientation.transform.position,
			destinationRotation = startOrientation.transform.rotation,
			matchOrientation    = MatchOrientation.TargetUpAndForward
		};
		teleportationProvider.QueueTeleportRequest(request);
	}


	public void SetVRMode(bool vrSittingMode)
	{
		if (vrSittingMode)
		{
			xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
		}
		else
		{
			xrOrigin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
		}
	}

	void OnDisable()
	{
		GetComponent<HMDInitialization>().onHMDInitialized -= HMDInitialized;
	}
}
