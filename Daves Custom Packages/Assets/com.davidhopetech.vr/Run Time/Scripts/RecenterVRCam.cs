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
        var camera = GetComponentInChildren<Camera>();

        if (camera == null)  return; 

        var cameraTransform = camera.gameObject.transform;
        
        var camEuler    = cameraTransform.localRotation.eulerAngles;
        var invRotation = Quaternion.Euler(0, -camEuler.y, 0);
        transform.localPosition = invRotation * -cameraTransform.localPosition;

        var rot = invRotation.eulerAngles;
        transform.localRotation = invRotation;
    }
}

