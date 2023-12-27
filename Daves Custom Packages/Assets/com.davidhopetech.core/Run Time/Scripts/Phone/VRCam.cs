using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.davidhopetech.phone.run_time.scripts
{
    public class VRCam : MonoBehaviour
    {
        private Phone phone;


        void Start()
        {
            if (phone == null)
            {
                var go = GameObject.Find("Phone");
                if (go != null)
                {
                    Debug.Log("Error: Phone not found");
                    return;
                }

                phone = go.GetComponent<Phone>();
                if (phone == null)
                {
                    Debug.Log("Error: Phone Script not found");
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!Object.ReferenceEquals(phone, null))
            {
                transform.rotation = phone.InvGyroAtt;
            }
        }
    }
}