using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHTEventService : MonoBehaviour
{
    public DHTUpdateDebugValue1Event   dhtUpdateDebugValue1Event   = new DHTUpdateDebugValue1Event();
    public DHTUpdateDebugTeleportEvent dhtUpdateDebugTeleportEvent = new DHTUpdateDebugTeleportEvent();
    public DHTUpdateDebugMiscEvent     dhtUpdateDebugMiscEvent     = new DHTUpdateDebugMiscEvent();
}
