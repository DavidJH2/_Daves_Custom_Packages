using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;

public class DHTDebugPanel_Service : DHTService<DHTDebugPanel_Service>
{
    [SerializeField] private DebugPanelElement[] elements;
    [SerializeField] private bool                _resetOnStart = true;

    private void Start()
    {
        // var service                     = DHTServiceLocator.Get<DHTDebugPanelService_DONT_USE>();
        // if(service) service.dhtDebugPanelService =  this as DHTDebugPanel_Service<DHTDebugPanel_1_Service>;

        DHTServiceLocator.Get<DHTLogService>().Log($"Reset On Start: {_resetOnStart}");
        if (_resetOnStart) ResetOnStart();
    }

    private void ResetOnStart()
    {
        foreach (var element in elements)
        {
            element.Reset();
        }
    }

    public void SetElement(int elemNum, string newValue)
    {
        var element = elements[elemNum]; //
        element.Set(newValue);
    }

    public void SetElement(int elemNum, string newLabel, string newValue)
    {
        if(elements == null || elemNum>=elements.Length) return;
        
        var element = elements[elemNum];
        element.Set(newLabel, newValue);
    }
}
