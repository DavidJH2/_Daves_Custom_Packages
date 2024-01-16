using UnityEngine;

namespace com.davidhopetech.core.Run_Time.Scripts.Service_Locator
{
    public class DHTServiceLocator : Singleton<DHTServiceLocator>
    {
        public ServiceType Get<ServiceType>() where ServiceType : Object
        {
            ServiceType service;
            service = FindObjectOfType<ServiceType>(true);
            return service;
        }
    }
}
