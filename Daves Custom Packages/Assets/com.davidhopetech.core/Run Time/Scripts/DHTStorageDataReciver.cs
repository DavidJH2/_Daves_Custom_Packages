using System;
using com.davidhopetech.core.Run_Time.Scripts.Service;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using TMPro;
using UnityEngine;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    public class DHTStorageDataReciver : MonoBehaviour
    {
        [SerializeField] private string dataItemName;
        [SerializeField] private string defaultValue;
        
        private                  DHT_TMP_InputField    _inputField;
        private                  bool              _isUpdating = false;
        private                  DHTStorageService _storageService;

        void Start()
        {
            _storageService = DHTServiceLocator.Get<DHTStorageService>();
            if (_storageService is null) throw new Exception("No Storage DHTService available");

            var dataItem = _storageService.GetData(dataItemName, defaultValue);
            if (dataItem == null) throw new Exception($"DataItem '{dataItemName}' not in DataStorage");

            if (_inputField is null) _inputField = GetComponentInChildren<DHT_TMP_InputField>();
            _inputField.text = dataItem.value;
            

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
                        dataItem.value = newName;
                    }
                });
            
            dataItem.ChangeEvent += newName =>
            {
                if (!_isUpdating)
                {
                    _isUpdating      = true;
                    _inputField.text = newName;
                }
                else
                {
                    _isUpdating = false;
                }
            };
        }
    }
}
