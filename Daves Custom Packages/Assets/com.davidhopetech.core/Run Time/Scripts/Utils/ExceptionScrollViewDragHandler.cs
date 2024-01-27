using System;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ExceptionScrollViewDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [SerializeField] private bool            scrollToBottom = true;
    [SerializeField] private ScrollRect      _scrollRect;
    private                  DHTLogScreen    _logScreen;
    private                  CustomScrollbar _customScrollbar;


    private void Awake()
    {
        _logScreen       = DHTServiceLocator.Get<DHTLogScreen>();
        _customScrollbar = GetComponentInChildren<CustomScrollbar>();
        _customScrollbar.ScrollbarDraggingStateChange.AddListener(OnScrollbarDraggingStateChange);
    }

    public void OnUpdatePos(Vector2 arg0)
    {
        UpdatePos();
    }

    public void UpdatePos()
    {
        if (scrollToBottom && !_customScrollbar.scrollbarBeingDragged) _scrollRect.verticalNormalizedPosition = 0;
    }

    private void Start()
    {
        if (_scrollRect == null) _scrollRect = GetComponentInChildren<ScrollRect>();
        _scrollRect.onValueChanged.AddListener(OnUpdatePos);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // _logScreen.Log($"OnBeginDrag : {eventData}\n");
    }

    public void OnScrollBarDraged(Single delta)
    {
        /*
    _logScreen.Log($"OnDrag called with data: {delta}\n");
    */
    }

    public void OnScrollbarDraggingStateChange(bool state)
    {
        var atBottom = _scrollRect.normalizedPosition.y <= 0.02f;

        if (state == false)
        {
            /*
            _logScreen.Log($"OnDrag called with data: {_scrollRect.normalizedPosition},{atBottom}\n");
            */
            if (atBottom)
                scrollToBottom = true;
            else
                scrollToBottom = false;
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (_scrollRect.normalizedPosition.y <= 0.02f)
            scrollToBottom = true;
        else
            scrollToBottom = false;

        var parent = this.GetComponentInParent<DHTLogScreen>();
        if (!parent)
        {
            /*
            _logScreen.Log($"OnDrag called with data: {_scrollRect.normalizedPosition}\n");
             _logScreen.Log($"OnDrag called with data: {eventData}\n");
             */
        }
    }
}

