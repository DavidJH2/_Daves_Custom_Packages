

using System;
using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using com.davidhopetech.core.Run_Time.Utils;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;

public class HMDInitialization : MonoBehaviour
{
	public  System.Action onHMDInitialized; // Custom callback action
	private DHTLogService _logService;

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}


	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void Start()
	{
		_logService = DHTServiceLocator.Get<DHTLogService>();
		StartCoroutine(WaitForHMDTracking());
	}

	IEnumerator WaitForHMDTracking()
	{
		bool   isTracking = false;
		XRNodeState hmdState;
		
		_logService?.Log("------  Wait for Node  ------");
		
		while (!isTracking)
		{
			
			var nodes = new List<XRNodeState>();
			InputTracking.GetNodeStates(nodes);

			foreach (var node in nodes)
			{
				if (node.nodeType == XRNode.Head)
				{
					hmdState   = node;
					isTracking = node.tracked;
					break;
				}
			}

			if (!isTracking)
			{
				yield return null; // Wait until the next frame to check again
			}
		}

		onHMDInitialized?.Invoke();
		
		if(DHTDebug.ShowPostionResetDebug) _logService?.Log("------  Wait for XR General Settings  ------");
		yield return new WaitUntil(() => XRGeneralSettings.Instance.Manager.isInitializationComplete);

		// Once tracking is confirmed, invoke the callback
		onHMDInitialized?.Invoke();
		if(DHTDebug.ShowPostionResetDebug) _logService?.Log("------  Wait 0.2 sec  ------");
		
		yield return new WaitForSeconds(.2f);			// <--- Required only for Quest 3
		onHMDInitialized?.Invoke();
		
		if(DHTDebug.ShowPostionResetDebug) _logService?.Log("------  Done  ------");
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
	}
}
