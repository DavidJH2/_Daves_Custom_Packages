using System.Threading.Tasks;
using com.davidhopetech.core.Run_Time.Utils;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine.Events;


public class DHTSignInManager : Singleton<DHTSignInManager>
{
    public UnityEvent<bool> SignedInAnonymously = new UnityEvent<bool>();
    
    public async Task SignInAnonymously()
    {
        DHTDebug.LogTag("Initializing Unity Services...", this);
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        
        SignedInAnonymously.Invoke(true);
    }
}
