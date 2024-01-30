using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using com.davidhopetech.core.Run_Time.Utils;
using Run_Time.Scripts.Misc;
using Unity.XR.CoreUtils;
using UnityEngine;
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
		
		if(startOrientation == null) startOrientation = ObjectExtentions.DHTFindObjectOfType<StartPosition>().gameObject;
		if(startOrientation == null) startOrientation = gameObject;
		
		if (xrOrigin == null)
		{
			xrOrigin = GetComponent<XROrigin>();
		}
	}


	public void HMDInitialized()
	{
		if (resetPositionOnStart)
		{
			ResetPosition();
		}
	}

	
	private int resetCount = 0;

	public void ResetPosition()
	{
		if (startOrientation == null)
		{
			if (_logService) _logService.Log($"--------  No Start Position to Reset To  ------");
			return;
		}
		
		resetCount++;
		if (DTH.ShowPostionResetDebug && _logService) _logService.Log($"--------  Resetting Position ({resetCount})  ------");
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
