using com.davidhopetech.core.Run_Time.Extensions;
using UnityEngine;

namespace com.davidhopetech.core.Run_Time.Scripts.Service_Locator
{
    public class DHTServiceLocator : Singleton<DHTServiceLocator>
    {
        public static TServiceType Get<TServiceType>() where TServiceType : Object
        {
            return ObjectExtentions.DHTFindObjectOfType<TServiceType>(true);
        }
    }
}
