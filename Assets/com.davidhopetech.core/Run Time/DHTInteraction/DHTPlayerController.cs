using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DHTPlayerController : MonoBehaviour
{
    private List<DHTGrabable> _grabables;
    
    private DHTInteractionState  _dhtInteractionState = new DhtDhtInteractionStateIdle();


    void Start()
    {
        _grabables = GameObject.FindObjectsOfType<DHTGrabable>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
