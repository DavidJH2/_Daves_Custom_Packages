using System;
using com.davidhopetech.core.Run_Time.Scripts.Service;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using TMPro;
using UnityEngine;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    public class DHTPlayerName : MonoBehaviour
    {
        private TMP_InputField    _inputField;
        private bool              _isUpdating = false;
        private DHTStorageService _storageService;

        void Start()
        {
            _storageService = DHTServiceLocator.Get<DHTStorageService>();
            if (_storageService is null) throw new Exception("No Storage Service available");

            _storageService.playerData.NameChangeEvent += newName =>
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

            if (_inputField is null) _inputField = GetComponentInChildren<TMP_InputField>();
            if (_inputField is not null)
                _inputField.onValueChanged.AddListener(newName =>
                {
                    if (_isUpdating)
                    {
                        _isUpdating = false;
                    }
                    else
                    {
                        _isUpdating                     = true;
                        _storageService.playerData.Name = newName;
                    }
                });
        }
    }
}
