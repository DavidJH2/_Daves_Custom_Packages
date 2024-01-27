using com.davidhopetech.core.Run_Time.Extensions;
using UnityEngine;

namespace com.davidhopetech.core.Run_Time.Scripts.Service_Locator
{
    // public class DHTServiceLocator : Singleton<DHTServiceLocator>
    public class DHTServiceLocator : MonoBehaviour
    {
        public static TServiceType Get<TServiceType>() where TServiceType : Object
        {
            var services = ObjectExtentions.DHTFindObjectsByType<TServiceType>(FindObjectsSortMode.None);

            if (services.Length == 0)
            {
                Debug.Log($"DHT Service '{typeof(TServiceType).Name}' Not In Scene");
                return null;
            }
            else if(services.Length>1)
                Debug.Log($"There should only be one DHT Service '{typeof(TServiceType).Name}' In Scene");
            
            return services[0];
        }
    }
}
