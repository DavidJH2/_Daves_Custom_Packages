
using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    [DefaultExecutionOrder(1000)]
    public class DHT_InputField_StorageDataReceiver : DHTStorageDataReceiverGeneric<string>
    {
        internal DHT_TMP_InputField _inputField;
        
        new void Start()
        {
            base.Start();
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
                        dataItem.value = (string) Convert.ChangeType(newName, typeof(string));
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
