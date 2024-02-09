using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecenterVRCam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Recenter()
    {
        var camTransform = GetComponentInChildren<Camera>().gameObject.transform;
        
        var posOffset = -camTransform.localPosition;
        // posOffset.y             = 0;

        var camEuler    = camTransform.localRotation.eulerAngles;
        var invRotation = Quaternion.Euler(0, -camEuler.y, 0);
        transform.localPosition = invRotation * -camTransform.localPosition;
        
        var rot = invRotation.eulerAngles;
        transform.localRotation = invRotation;
    }
}
