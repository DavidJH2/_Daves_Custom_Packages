using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
	public class MouseLookController : MonoBehaviour
	{
		[SerializeField] private bool      snapBack = true;
		[SerializeField] private Transform yawTransform;
		[SerializeField] private Transform pitchTransform;
		
		
		Vector2                       camRotation;
		private                  bool mouseLookButton;

	
		void Start()
		{
		
		}


		public void MouseLookButton(InputAction.CallbackContext context)
		{
			mouseLookButton = context.ReadValueAsButton();
			if (!mouseLookButton && snapBack)
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
			Quaternion rot;
			rot                          = Quaternion.Euler(0f, camRotation.x, 0f);
			yawTransform.localRotation   = rot;
			rot                          = Quaternion.Euler(-camRotation.y, 0f, 0f);
			pitchTransform.localRotation = rot;
		}
	}
}
