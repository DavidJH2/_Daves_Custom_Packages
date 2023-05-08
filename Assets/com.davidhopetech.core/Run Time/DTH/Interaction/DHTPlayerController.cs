using System.Collections.Generic;
using System.Linq;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using com.davidhopetech.core.Run_Time.DTH.ServiceLocator;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.davidhopetech.core.Run_Time.DTH.Interaction
	{
		public class DHTPlayerController : MonoBehaviour
		{
			//[SerializeField] internal InputDeviceCharacteristics controllerCharacteristics;
			[SerializeField] internal GameObject _lefttInteractor;
			[SerializeField] internal GameObject _rightInteractor;
		
			[SerializeField] internal GameObject _leftMirrorHand;
			[SerializeField] internal GameObject _rightMirrorHand;

			[SerializeField] internal float handSpringCoeeff;		// TODO: Move these to Settings Scriptable Object?
			[SerializeField] internal float handDampCoeeff;
		
			// private                   InputDevice                targetDevice;

			internal List<DHTGrabable>   _grabables;
			internal DHTInteractionState _dhtInteractionState;

			void Start()
			{
				Joystick.JoyStickEvent += OnJoystick;

				_dhtInteractionState = gameObject.AddComponent<DHTInteractionStateIdle>();
				_grabables           = GameObject.FindObjectsOfType<DHTGrabable>().ToList();
			
				Debug.Log($"Number of Grabables: {_grabables.Count}");
			}

			// UpdateStateImpl is called once per frame
			void Update()
			{
				_dhtInteractionState?.UpdateState();
			}
			

			
			private void OnJoystick(object sender, Joystick.JoystickEventArgs e)
			{
				DHTServiceLocator.DhtEventService.dhtUpdateDebugTeleportEvent.Invoke($"Value: {e.AngX}, {e.AngY}");
			}
		}
	}
