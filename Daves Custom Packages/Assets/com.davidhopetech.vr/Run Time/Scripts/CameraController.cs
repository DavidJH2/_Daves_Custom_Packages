using UnityEngine;
using UnityEngine.InputSystem;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    public class CameraController : MonoBehaviour
    {
        Vector2      camRotation;
        private bool mouseLookButton;

    
        void Start()
        {
        
        }


        public void MouseLookButton(InputAction.CallbackContext context)
        {
            mouseLookButton = context.ReadValueAsButton();
            if (!mouseLookButton)
            {
                camRotation = Vector2.zero;
            }
        }


        public void MouseLook(InputAction.CallbackContext context)
        {
            if (!mouseLookButton)
                return;
        
            var delta = context.ReadValue<Vector2>();
            delta.x     /= 4.0f;
            delta.y     /= 8.0f;
            camRotation += delta;
        }

        // Update is called once per frame
        void Update()
        {
            var rot = Quaternion.Euler(-camRotation.y, camRotation.x, 0.0f);
            transform.localRotation = rot;
        }
    }
}
