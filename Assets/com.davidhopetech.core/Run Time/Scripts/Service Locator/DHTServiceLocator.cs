using com.davidhopetech.core.Run_Time.Misc;

namespace com.davidhopetech.core.Run_Time.Scripts.Service_Locator
{
    public class DHTServiceLocator : Singleton
    {
        public static readonly DHTEventService dhtEventService = new DHTEventService();
    }
}
