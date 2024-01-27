using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomScrollbar : Scrollbar
{
	public  bool          scrollbarBeingDragged = false;
	private DHTLogService logService;

	public UnityEvent<bool> ScrollbarDraggingStateChange;

	
	protected override void Start()
	{
		base.Start();
		logService = DHTServiceLocator.Get<DHTLogService>();
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		base.OnBeginDrag(eventData);
		scrollbarBeingDragged = true;
		ScrollbarDraggingStateChange.Invoke(scrollbarBeingDragged);
		if(logService) logService.Log("Scrollbar drag started\n");
	}

	public override void OnDrag(PointerEventData eventData)
	{
		base.OnDrag(eventData);
		// if(logService) logService.Log("Scrollbar being dragged\n");
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		scrollbarBeingDragged = false;
		ScrollbarDraggingStateChange.Invoke(scrollbarBeingDragged);
		if(logService) logService.Log("Scrollbar drag stopped\n");
	}
}