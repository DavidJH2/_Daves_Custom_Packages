using System.Collections.Generic;
using System.Linq;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using com.davidhopetech.core.Run_Time.DTH.Interaction;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using com.davidhopetech.core.Run_Time.Scripts.Interaction.States;
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
		internal DHTInteractionStateRef   LeftHandInteractionStateRef;
		internal DHTInteractionStateRef   RightHandInteractionStateRef;

		void Start()
		{
			var rightHandInteractionState = gameObject.AddComponent<DHTInteractionIdleState>();
			RightHandInteractionStateRef         = new DHTInteractionStateRef(rightHandInteractionState);
			rightHandInteractionState.MirrorHand = rightMirrorHand.GetComponent<MirrorHand>();
			rightHandInteractionState.selfHandle = RightHandInteractionStateRef;
			
			var leftHandInteractionState = gameObject.AddComponent<DHTInteractionIdleState>();
			LeftHandInteractionStateRef         = new DHTInteractionStateRef(leftHandInteractionState);
			leftHandInteractionState.MirrorHand = leftMirrorHand.GetComponent<MirrorHand>();
			leftHandInteractionState.selfHandle = LeftHandInteractionStateRef;
			
			Interactables                = FindObjectsOfType<DTHInteractable>().ToList();

			Debug.Log($"Number of Grabables: {Interactables.Count}");
		}

		// UpdateStateImpl is called once per frame
		void Update()
		{
			RightHandInteractionStateRef.InteractionState.UpdateState();
			LeftHandInteractionStateRef.InteractionState.UpdateState();
		}
	}
}
