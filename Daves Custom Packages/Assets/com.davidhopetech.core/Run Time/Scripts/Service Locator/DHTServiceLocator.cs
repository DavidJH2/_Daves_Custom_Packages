using UnityEngine;

namespace com.davidhopetech.core.Run_Time.Scripts.Service_Locator
{
    public class DHTServiceLocator : MonoBehaviour
    {
        public static readonly DHTEventService dhtEventService = new DHTEventService();

        public static DHTHMDService dhtHmdService
        {
            get
            {
                return __dhtHmdService;
            }
        }
        
        private static           DHTHMDService __dhtHmdService;
        [SerializeField] private DHTHMDService _dhtHmdService;

        public void Start()
        {
            __dhtHmdService = _dhtHmdService;
        }
    }
}
