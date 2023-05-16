using System.Collections.Generic;
using System.Linq;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using com.davidhopetech.core.Run_Time.DTH.Interaction;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using UnityEngine;

namespace com.davidhopetech.core.Run_Time.Scripts.Interaction
{
	public class DHTPlayerController : MonoBehaviour
	{
		//[SerializeField] internal InputDeviceCharacteristics controllerCharacteristics;
		[SerializeField] internal GameObject leftInteractor;
		[SerializeField] internal GameObject rightInteractor;
		[SerializeField] internal GameObject leftMirrorHand;
		[SerializeField] internal GameObject rightMirrorHand;
		[SerializeField] internal float      handSpringCoeeff; // TODO: Move these to Settings Scriptable Object
		[SerializeField] internal float      handDampCoeeff;

		// private                   InputDevice                targetDevice;

		[SerializeField] DTHJoystick dthJoystick;

		internal List<DTHInteractable> Interactables;
		internal DHTInteractionState   InteractionState;

		void Start()
		{
			InteractionState = gameObject.AddComponent<DHTInteractionIdleState>();
			Interactables    = FindObjectsOfType<DTHInteractable>().ToList();

			Debug.Log($"Number of Grabables: {Interactables.Count}");
		}

		// UpdateStateImpl is called once per frame
		void Update()
		{
			InteractionState?.UpdateState();
		}
	}
}
