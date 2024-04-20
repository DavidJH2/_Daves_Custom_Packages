using System;
using System.Linq;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.Animations;

namespace com.davidhopetech.vr.Run_Time.Scripts.Interaction.States
{
	[Serializable]
	public class DHTInteractionIdleState : DHTInteractionState
	{
		protected override void UpdateStateImpl()
		{
			FindClosestInteractor();
		}

	
		private new void Awake()
		{
			base.Awake();
		}


		protected override void StartExt()
		{
		}


		void FindClosestInteractor()
		{
			var interactorPos = MirrorHand.xrControler.transform.position;
			var interactables = playerController.Interactables;

			if (interactables.Count == 0)
			{
				return;
			}

			var orderedInteractables = interactables.OrderBy(o => o.Dist(interactorPos));

			var interactable = orderedInteractables.First();

			// Debug.Log($"Mirror Hand: {MirrorHand.name}");
			var debugPanelService = DHTServiceLocator.Get<DHTDebugPanel_1_Service>();
			var dist              = interactable.Dist(interactorPos).ToString();

			if (interactable.InRange(interactorPos))
			{
				DebugMiscEvent.Invoke($"Closest Interactable: {interactable.gameObject.name}");

				if (MirrorHand.IsGrabbing)
				{
					if (interactable is DHTStationaryGrabable stationaryGrabable)
					{
						ChangeToStationaryGrabbingState(stationaryGrabable);
						return;
					}

					if (interactable is DHTGrabable grabable)
					{
						ChangeToGrabbingState(grabable);
					}

					if (interactable is DHTSpatialLock spatialLock)
					{
						ChangeToSpatialLockState(spatialLock);
						return;
					}
				}
				else
				{
					DebugMiscEvent.Invoke($"Not In Grab Range");
				}
			}
		}


		private void ChangeToStationaryGrabbingState(DHTStationaryGrabable stationaryGrabable)
		{
			Debug.Log("######  Change to Stationary Grabbing State  ######");
			DebugValue1Event.Invoke("###  Change to Stationary Grabbing State  ###");
			var MirrorHandGO = MirrorHand.gameObject;
		
			DHTInteractionStationaryGrabbingState component = playerController.gameObject.AddComponent<DHTInteractionStationaryGrabbingState>();
			var                                   grabPoint = stationaryGrabable.GetComponentInChildren<DHTGrabPoint>();

			GameObject tempGrabPointGO;
			Transform  grabItemInitialTransform;
			if (grabPoint)
			{
				grabItemInitialTransform = grabPoint.transform;
			}
			else
			{
				grabItemInitialTransform = transform;
			}

			tempGrabPointGO = new GameObject
			{
				transform =
				{
					position = grabItemInitialTransform.position,
					rotation = grabItemInitialTransform.rotation
				}
			};

			component.GrabItemInitialTransform = tempGrabPointGO.transform;

			component.grabedItem        = stationaryGrabable;
			component.MirrorHand        = MirrorHand;
			component.selfHandle        = selfHandle;
			selfHandle.InteractionState = component;
		
			MirrorHandGO.GetComponent<ParentConstraint>().enabled = true;
			MirrorHandGO.DisableAllColliders();
		
			Destroy(this);
		}


		private void ChangeToGrabbingState(DHTGrabable grabable)
		{
			Debug.Log("######  Change to Grabbing State  ######");
			DebugValue1Event.Invoke("###  Change to Grabbing State  ###");
			var MirrorHandGO = MirrorHand.gameObject;

			var       grabPoint                = grabable.GetComponentInChildren<DHTGrabPoint>();
			Transform grabItemInitialTransform = grabPoint ? grabPoint.transform : transform;

			GameObject tempGrabPointGO = new GameObject
			{
				transform =
				{
					position = grabItemInitialTransform.position,
					rotation = grabItemInitialTransform.rotation
				}
			};

		
			DHTInteractionGrabbingState component = playerController.gameObject.AddComponent<DHTInteractionGrabbingState>();
			component.GrabItemInitialTransform = tempGrabPointGO.transform;

			component.grabedItem        = grabable;
			component.MirrorHand        = MirrorHand;
			component.selfHandle        = selfHandle;
			selfHandle.InteractionState = component;
		
			MirrorHandGO.GetComponent<ParentConstraint>().enabled = true;
			MirrorHandGO.DisableAllColliders();
		
			Destroy(this);
		}

	
		private void ChangeToSpatialLockState(DHTSpatialLock spatialLock)
		{
			//return;
			Debug.Log("######  Change to Spatial Lock State  ######");
			DebugValue1Event.Invoke("###  Change to Spatial Lock State  ###");

			handAnimator.SetBool("Near Two Buttons", true);

			var MirrorHandGO = MirrorHand.gameObject;
		
			DHTInteractionSpatialLockingState component = playerController.gameObject.AddComponent<DHTInteractionSpatialLockingState>();
			component.SpatialLock       = spatialLock;
			component.MirrorHand        = MirrorHand;
			component.selfHandle        = selfHandle;
			selfHandle.InteractionState = component;
		
			MirrorHandGO.GetComponent<ParentConstraint>().enabled = true;
		
			Destroy(this);
		}
	}
}

