using com.davidhopetech.core.Run_Time.Misc;

namespace com.davidhopetech.core.Run_Time.DTH.ServiceLocator
{
    public class DHTServiceLocator : Singleton
    {
        static public DHTEventService DhtEventService = new DHTEventService();
    }
}
