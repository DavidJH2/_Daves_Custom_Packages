using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.davidhopetech.core.Run_Time.Debug;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using Unity.VisualScripting;
using UnityEngine;

public class DHTPlayerController : MonoBehaviour
{
    [SerializeField] internal GameObject _rightInteractor;
    
    internal List<DHTGrabable> _grabables;
    [SerializeField] private DHTInteractionState  _dhtInteractionState;


    void Start()
    {
        _dhtInteractionState = new DHTInteractionStateIdle(this);
        _grabables           = GameObject.FindObjectsOfType<DHTGrabable>().ToList();
        
        Debug.Log($"Number of Grabables: {_grabables.Count}");
    }

    // UpdateState is called once per frame
    void Update()
    {
        _dhtInteractionState.UpdateState();
    }
}
