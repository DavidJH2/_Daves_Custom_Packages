using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomScrollbar : Scrollbar
{
	public override void OnBeginDrag(PointerEventData eventData)
	{
		base.OnBeginDrag(eventData);
		Debug.Log("Scrollbar drag started");
		// Add your logic here for when drag starts
	}

	public override void OnDrag(PointerEventData eventData)
	{
		base.OnDrag(eventData);
		Debug.Log("Scrollbar being draged");
		// Add your logic here for when drag ends
	}
}