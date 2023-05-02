using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using com.davidhopetech.core.Run_Time.Debug;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using UnityEngine.Events;

[Serializable]
class DHTInteractionStateIdle : DHTInteractionState
{
	private DHTUpdateDebugValue1Event _debugValue1;


	internal DHTInteractionStateIdle(DHTPlayerController iController)
	{
		// _debugValue1 = new DHTUpdateDebugValue1Event();
		_debugValue1 = FindObjectOfType<DHTEventContainer>().GetComponent<DHTEventContainer>()
			.dhtUpdateDebugValue1Event;
		_controller = iController;
	}

	public override void UpdateState()
	{
		DistancesToInteractors(_controller._rightInteractor);
	}

	void DistancesToInteractors(GameObject interactor)
	{
		float radius = 0;
		var closeGrabables = _controller._grabables.Where((grabable, index) =>
		{
			var dist = Vector3.Distance(grabable.transform.position, interactor.transform.position);

			radius = grabable.grabRadius;
			return (dist <= grabable.grabRadius);
		});


		if (closeGrabables.Count() > 0)
		{
			_debugValue1.Invoke($"In Grab Range");
		}
		else
		{
			_debugValue1.Invoke($"Not In Grab Range");
		}
	}
}

