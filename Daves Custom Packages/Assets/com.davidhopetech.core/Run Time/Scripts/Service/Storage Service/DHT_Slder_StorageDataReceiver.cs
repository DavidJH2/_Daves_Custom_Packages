
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    [DefaultExecutionOrder(1000)]
    public class DHT_Slder_StorageDataReceiver : DHTStorageDataReceiverGeneric<float>
    {
        internal Slider _slider;
        
        new void Start()
        {
            base.Start();
            if (_slider is null) _slider = GetComponentInChildren<Slider>();
            _slider.value = Convert.ToSingle(dataItem.value, CultureInfo.InvariantCulture);
            

            if (_slider is not null)
                _slider.onValueChanged.AddListener(newName =>
                {
                    if (_isUpdating)
                    {
                        _isUpdating = false;
                    }
                    else
                    {
                        _isUpdating     = true;
                        dataItem.value = (float) Convert.ChangeType(newName, typeof(float));
                    }
                });
            
            dataItem.ChangeEvent += newName =>
            {
                if (!_isUpdating)
                {
                    _isUpdating      = true;
                    _slider.value = Convert.ToInt32(newName, CultureInfo.InvariantCulture);
                }
                else
                {
                    _isUpdating = false;
                }
            };
        }
    }
}
