using UnityEngine;
using UnityEngine.XR.OpenXR;

public class OpenXRSessionWatcher : MonoBehaviour
{
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
		// Logic when the session is ending
		Debug.Log("OpenXR session is ending");
		// Return true to allow the session to end, or false to prevent it
		return true;
	}

	private bool OnSessionRestarting()
	{
		// Logic when the session is restarting
		Debug.Log("OpenXR session is restarting");
		// Return true to allow the session to restart, or false to prevent it
		return true;
	}
}