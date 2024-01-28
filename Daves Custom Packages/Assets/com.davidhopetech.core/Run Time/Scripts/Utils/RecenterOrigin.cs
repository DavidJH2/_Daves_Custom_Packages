using System.Collections;
using System.Collections.Generic;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.XR;

public class RecenterOrigin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Recenter();
    }

    
    
    // #########################################################################################################
    // ###########                                                                                   ###########
    // ###########                 Recentering Programatically is not Supported by Meta             ############
    // ###########                                                                                   ###########
    // #########################################################################################################
    
    public void Recenter()
    {
        DHTServiceLocator.Get<DHTLogService>().Log("------  Attempting to recenter  ------\n");
        List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances<XRInputSubsystem>(subsystems);

        if (subsystems.Count > 0)
        {
            XRInputSubsystem inputSubsystem = subsystems[0];
            if (inputSubsystem != null)
            {
                var succesful = inputSubsystem.TryRecenter();
                var succesStr = succesful ? "Succesfull" : "Not succesful";
                DHTServiceLocator.Get<DHTLogService>().Log($"{succesStr}\n");
            }
        }
    }
}
