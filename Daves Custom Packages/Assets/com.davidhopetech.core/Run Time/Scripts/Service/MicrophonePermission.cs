using UnityEngine;
using UnityEngine.Android;

public class MicrophonePermission : MonoBehaviour
{
	public void CheckMicrophonePermission()
	{
		// Check if the user has already granted the RECORD_AUDIO permission
		if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
		{
			// Request the permission
			Permission.RequestUserPermission(Permission.Microphone);
		}
	}
}