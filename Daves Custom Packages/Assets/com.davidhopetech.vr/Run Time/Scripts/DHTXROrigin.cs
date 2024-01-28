using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof (XROrigin), typeof(TeleportationProvider))]
public class DHTXROrigin : MonoBehaviour
{
	[SerializeField] private XROrigin              xrOrigin;
	[SerializeField] private TeleportationProvider teleportationProvider;
	[SerializeField] private GameObject            startOrientation;

	void OnEnable()
	{
		GetComponent<HMDInitialization>().onHMDInitialized += InitializePosition;
	}
	
	void Start()
	{
		teleportationProvider         = GetComponent<TeleportationProvider>();
		
		if(startOrientation == null) startOrientation = gameObject;
		
		if (xrOrigin == null)
		{
			
			xrOrigin = GetComponent<XROrigin>();
		}
	}

	public void InitializePosition()
	{
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
		GetComponent<HMDInitialization>().onHMDInitialized -= InitializePosition;
	}
}
