using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using com.davidhopetech.core.Run_Time.Debug;
using com.davidhopetech.core.Run_Time.DHTInteraction;
using UnityEngine.Events;

[Serializable]
class DHTInteractionStateGrabing : DHTInteractionState
{
	private DHTUpdateDebugValue1Event _debugValue1;


	internal DHTInteractionStateGrabing(DHTPlayerController iController)
	{
		_debugValue1 = FindObjectOfType<DHTEventContainer>().GetComponent<DHTEventContainer>()
			.dhtUpdateDebugValue1Event;
		_controller = iController;
	}

	public override void UpdateState()
	{
	}
}

