using System.Collections;
using System.Collections.Generic;
using com.davidhopetech.core.Run_Time.Scripts.Interaction;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropDown;
    
    private                  DHTPlayerController _playerController;
    private 
    
    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<DHTPlayerController>();
        _playerController.SetVRMode(dropDown);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
