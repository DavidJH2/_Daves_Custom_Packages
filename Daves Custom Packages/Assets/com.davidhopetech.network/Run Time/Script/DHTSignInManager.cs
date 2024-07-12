using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.davidhopetech.core.Run_Time.Utils;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class DHTSignInManager : Singleton<DHTSignInManager>
{
    public async Task SignInAnonymously()
    {
        DHTDebug.LogTag("Initializing Unity Services...", this);
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}
