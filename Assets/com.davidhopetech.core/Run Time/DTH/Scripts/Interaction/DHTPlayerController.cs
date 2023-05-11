using System.Collections.Generic;
using System.Linq;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using UnityEngine;

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

			[SerializeField]  Joystick _joystick;
			
			internal List<DTHInteractable> _Interactables;
			internal DHTInteractionState   _dhtInteractionState;

			void Start()
			{
				_dhtInteractionState = gameObject.AddComponent<DHTInteractionIdleState>();
				_Interactables           = GameObject.FindObjectsOfType<DTHInteractable>().ToList();
			
				Debug.Log($"Number of Grabables: {_Interactables.Count}");
			}

			// UpdateStateImpl is called once per frame
			void Update()
			{
				_dhtInteractionState?.UpdateState();
			}
		}
	}
