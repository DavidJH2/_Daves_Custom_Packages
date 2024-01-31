using System;
using System.Collections.Generic;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using com.davidhopetech.core.Run_Time.Utils;
using UnityEngine;
using UnityEngine.XR;

public class RecenterOrigin : MonoBehaviour
{
	// Start is called before the first frame update
	HMDInitialization hmdInitialization;
	void OnEnable()
	{
		hmdInitialization = GetComponent<HMDInitialization>();
		hmdInitialization.onHMDInitialized += Recenter;
		// Recenter();
	}

	private void OnDisable()
	{
		hmdInitialization.onHMDInitialized -= Recenter;
	}


	// #########################################################################################################
	// ###########                                                                                   ###########
	// ###########                 Recentering Programatically is not Supported by Meta             ############
	// ###########                                                                                   ###########
	// #########################################################################################################

	public void Recenter()
	{
		Debug.Log("------  Attempting to recenter  ------");
		var service =  DHTServiceLocator.Get<DHTLogService>();
		
		if (com.davidhopetech.core.Run_Time.Utils.DHT.ShowPostionResetDebug && service) service.Log("------  Attempting to recenter  ------\n");
		List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
		SubsystemManager.GetInstances<XRInputSubsystem>(subsystems);

		if (subsystems.Count > 0)
		{
			XRInputSubsystem inputSubsystem = subsystems[0];
			if (inputSubsystem != null)
			{
				var succesful = inputSubsystem.TryRecenter();
				var succesStr = succesful ? "Succesfull" : "Not succesful";
				if (service) service.Log($"{succesStr}\n");
			}
		}
	}
}

