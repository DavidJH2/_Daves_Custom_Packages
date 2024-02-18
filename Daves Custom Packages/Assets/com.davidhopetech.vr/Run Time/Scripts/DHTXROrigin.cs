using System.Collections;
using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using com.davidhopetech.core.Run_Time.Utils;
using Run_Time.Scripts.Misc;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Management;


[RequireComponent(typeof (XROrigin), typeof(TeleportationProvider))]
public class DHTXROrigin : MonoBehaviour
{
	public static DHTXROrigin dhtXROrigin;
	
	[SerializeField] private bool                  resetPositionOnStart = true;
	[SerializeField] private XROrigin              xrOrigin;
	[SerializeField] private TeleportationProvider teleportationProvider;
	[SerializeField] private GameObject            InitialCameraPos;

	[SerializeField] private GameObject startOrientationGO = null;

	private DHTLogService _logService;
	private DHTHMDService _service;

	HMDInitialization hmdInitialization; 

	void OnEnable()
	{
	}
	
	void Start()
	{
		DHTXROrigin.dhtXROrigin = this;
		StartCoroutine(nameof(RecenterNextFrame));
		//StartCoroutine(nameof(InitializeXR));
		
		_logService = DHTServiceLocator.Get<DHTLogService>();
		_service = DHTServiceLocator.Get<DHTHMDService>();
		
		hmdInitialization = GetComponent<HMDInitialization>(); 
		hmdInitialization.onHMDInitialized += HMDInitialized;

		if (_service) _service.UserPresenceEvent.AddListener(OnUserPresence);

		teleportationProvider         = GetComponent<TeleportationProvider>();
		InitStartOrientation();
	
		if (xrOrigin == null)
		{
			xrOrigin = GetComponent<XROrigin>();
		}
		
		// MoveToStart();
	}

	private void InitStartOrientation()
	{
		if (startOrientationGO == null)
		{
			var startOrientation = ObjectExtentions.DHTFindObjectOfType<StartPosition>();
			if (startOrientation)
			{
				startOrientationGO = startOrientation.gameObject;
			}
			else
			{
				startOrientationGO = gameObject;
			}
		}
	}

	public IEnumerator  InitializeXR()
	{
		if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
		{
#if UNITY_EDITOR || !PLATFORM_ANDROID 
			// Initialize the XR Loader synchronously
			XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
#endif

			// Check if the Loader was successfully initialized
			if (XRGeneralSettings.Instance.Manager.activeLoader != null)
			{
				Debug.Log("XR Loader initialized successfully.");

				// Wait until the next frame to start subsystems, ensuring the loader is fully operational
				yield return null;

				XRGeneralSettings.Instance.Manager.StartSubsystems();
				Debug.Log("XR Subsystems started.");
			}
			else
			{
				// yield return null;
				//Debug.LogError("Failed to initialize XR Loader.");
			}
		}
		else
		{
			Debug.LogError("XRGeneralSettings or its Manager is null.");
		}
	}
	
	

	private void HMDInitialized()
	{
		// _logService.Log($"{DHTDebug.MethodeInfo(this)}:   HMDInitialized() event received");
	}


	public void OnUserPresence(bool state)
	{
		if (resetPositionOnStart && state)
		{
			// DHTDebug.LogTag($"---------------  Call Recenter()  State: {state}  ---------------");
			RecenterNextFrame();
			_service.UserPresenceEvent.RemoveListener(OnUserPresence);
		}
	}


	void RecenterNextFrame()
	{
		StartCoroutine(nameof(RecenterNextFrameCR));
	}
	IEnumerator RecenterNextFrameCR()
	{
		yield return null;
		Recenter();
	}
	
	public void MoveToStart()
	{
		if (startOrientationGO == null)
		{
			if (_logService) _logService.Log($"--------  No Start Position to Reset To  ------");
			return;
		}
		
		resetCount++;
		if (_logService) _logService.Log($"--------  Resetting Position ({resetCount})  ------");
		TeleportRequest request = new TeleportRequest()
		{
			destinationPosition = startOrientationGO.transform.position,
			destinationRotation = startOrientationGO.transform.rotation,
			matchOrientation    = MatchOrientation.TargetUpAndForward
		};
		teleportationProvider.QueueTeleportRequest(request);
	}
	
	
	private int resetCount = 0;

	public void Recenter()
	{
		// DHTDebug.LogTag($"---------------  Recenter()  ---------------");
		GetComponentInChildren<RecenterVRCam>().Recenter();
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
		// DHTDebug.Tag(this, "      <------------------------------");
		GetComponent<HMDInitialization>().onHMDInitialized -= HMDInitialized;
		if (_service) _service.UserPresenceEvent.RemoveListener(OnUserPresence);
	}
}
