using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;

public class HMDInitialization : MonoBehaviour
{
	public System.Action onHMDInitialized; // Custom callback action

	void Start()
	{
		StartCoroutine(WaitForHMDTracking());
	}

	IEnumerator WaitForHMDTracking()
	{
		XRNodeState hmdState   = new XRNodeState();
		bool        isTracking = false;

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

		// Once tracking is confirmed, invoke the callback
		onHMDInitialized?.Invoke();
	}
}