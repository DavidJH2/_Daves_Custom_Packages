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
                var go = GameObject.Find("Pnone");
                if (go != null)
                {
                    phone = go.GetComponent<Phone>();
                    if (phone == null)
                    {
                        Debug.Log("Error: Phone not found");
                    }
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