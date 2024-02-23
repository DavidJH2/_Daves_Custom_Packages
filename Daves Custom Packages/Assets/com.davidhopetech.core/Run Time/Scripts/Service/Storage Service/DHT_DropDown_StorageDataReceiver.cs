

using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    [DefaultExecutionOrder(1000)]
    public class DHT_DropDown_StorageDataReceiver : DHTStorageDataReceiverGeneric<int>
    {
        internal TMP_Dropdown _dropdown;
        
        new void Start()
        {
            base.Start();
            if (_dropdown is null) _dropdown = GetComponentInChildren<TMP_Dropdown>();
            _dropdown.value = Convert.ToInt32(dataItem.value, CultureInfo.InvariantCulture);
            

            if (_dropdown is not null)
                _dropdown.onValueChanged.AddListener(newName =>
                {
                    if (_isUpdating)
                    {
                        _isUpdating = false;
                    }
                    else
                    {
                        _isUpdating     = true;
                        dataItem.value = (int) Convert.ChangeType(newName, typeof(int));
                    }
                });
            
            dataItem.ChangeEvent += newName =>
            {
                if (!_isUpdating)
                {
                    _isUpdating      = true;
                    _dropdown.value = Convert.ToInt32(newName, CultureInfo.InvariantCulture);
                }
                else
                {
                    _isUpdating = false;
                }
            };
        }
    }
}
