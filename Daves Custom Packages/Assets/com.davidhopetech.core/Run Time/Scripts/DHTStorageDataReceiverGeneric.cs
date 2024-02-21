using System;
using System.Globalization;
using Codice.CM.WorkspaceServer.DataStore.IncomingChanges;
using com.davidhopetech.core.Run_Time.Scripts.Service;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using TMPro;
using UnityEngine;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    public class DHTStorageDataReceiverGeneric<T> : MonoBehaviour where T: IConvertible, IComparable
    {
        [SerializeField] private string dataItemName;
        [SerializeField] private T      defaultValue;
        [SerializeField] private bool   UsePlayerPrefs;
        
        private DHT_TMP_InputField          _inputField;
        private bool                        _isUpdating = false;
        private DHTStorageServiceGeneric<T> _storageService;

        private string PlayerPrefsKey;
        
        void Start()
        {
            PlayerPrefsKey = UsePlayerPrefs ? $"{dataItemName}PF" : "";
            
            _storageService = DHTServiceLocator.Get<DHTStorageServiceGeneric<T>>();
            if (_storageService is null) throw new Exception("No Storage DHTService available");

            var dataItem = _storageService.GetData(dataItemName, defaultValue, PlayerPrefsKey);
            if (dataItem == null) throw new Exception($"DataItem '{dataItemName}' not in DataStorage");

            if (_inputField is null) _inputField = GetComponentInChildren<DHT_TMP_InputField>();
            _inputField.text = Convert.ToString(dataItem.value, CultureInfo.InvariantCulture);
            

            if (_inputField is not null)
                _inputField.onValueChanged.AddListener(newName =>
                {
                    if (_isUpdating)
                    {
                        _isUpdating = false;
                    }
                    else
                    {
                        _isUpdating     = true;
                        dataItem.value = (T) Convert.ChangeType(newName, typeof(T));
                    }
                });
            
            dataItem.ChangeEvent += newName =>
            {
                if (!_isUpdating)
                {
                    _isUpdating      = true;
                    _inputField.text = Convert.ToString(newName, CultureInfo.InvariantCulture);
                    // dataItem.value   = (T) Convert.ChangeType(newName, typeof(T));
                }
                else
                {
                    _isUpdating = false;
                }
            };
        }
    }
}
