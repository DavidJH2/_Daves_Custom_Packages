using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Management;

public class HMDInitialization : MonoBehaviour
{
	public System.Action onHMDInitialized; // Custom callback action

	void Start()
	{
		StartCoroutine(WaitForHMDTracking());
	}

	IEnumerator WaitForHMDTracking()
	{
		/*
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
		*/

		// Debug.Log("------  HMD Initialized  ------");
		yield return new WaitUntil(() => XRGeneralSettings.Instance.Manager.isInitializationComplete);

		// Once tracking is confirmed, invoke the callback
		// yield return new WaitForSeconds(0.1f);			// <--- Required only for Quest 3
		onHMDInitialized?.Invoke();
	}
}




//  well, the XROrigin already supports teleportation when It's ready. I just need to know when it is ready, not the 