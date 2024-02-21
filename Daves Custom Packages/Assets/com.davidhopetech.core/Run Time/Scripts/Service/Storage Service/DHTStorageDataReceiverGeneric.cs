
using System;
using System.Globalization;
using com.davidhopetech.core.Run_Time.Scripts.Service;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using TMPro;
using UnityEngine;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    public class DHTStorageDataReceiverGeneric<T> : MonoBehaviour where T: IConvertible, IComparable
    {
        [SerializeField] internal string                      dataItemName;
        [SerializeField] internal T                           defaultValue;
        [SerializeField] internal bool                        UsePlayerPrefs;
        internal                  bool                        _isUpdating = false;
        internal                  DHTStorageServiceGeneric<T> _storageService;
        internal                  string                      PlayerPrefsKey;
        internal                  DHTStorageDataGeneric<T>    dataItem;
        
        internal void Start()
        {
            PlayerPrefsKey = UsePlayerPrefs ? $"{dataItemName}PF" : "";
            
            _storageService = DHTServiceLocator.Get<DHTStorageServiceGeneric<T>>();
            if (_storageService is null) throw new Exception("No Storage DHTService available");

            dataItem = _storageService.GetData(dataItemName, defaultValue, PlayerPrefsKey);
            if (dataItem == null) throw new Exception($"DataItem '{dataItemName}' not in DataStorage");
        }
    }
}
