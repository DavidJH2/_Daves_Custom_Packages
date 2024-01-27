using System;using System.Collections;
using System.Collections.Generic;
using com.davidhopetech.core.Run_Time.Extensions;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ExceptionScrollViewDragHandler : MonoBehaviour, IDragHandler
{
    [SerializeField] private bool         scrollToBottom = true;
    [SerializeField] private ScrollRect   _scrollRect;
    private                  DHTLogScreen _logScreen;


    private void Awake()
    {
        _logScreen = DHTServiceLocator.Get<DHTLogScreen>();
    }

    public void OnUpdatePos(Vector2 arg0)
    {
       UpdatePos();
    }

    public void UpdatePos()
    {
        if(scrollToBottom) _scrollRect.verticalNormalizedPosition = 0;
    }

    private void Start()
    {
        if (_scrollRect == null) _scrollRect = GetComponentInChildren<ScrollRect>();
        _scrollRect.onValueChanged.AddListener(OnUpdatePos);
    }


    public void OnScrollBarDraged(Single delta)
    {
        _logScreen.Log($"OnDrag called with data: {delta}\n");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_scrollRect.normalizedPosition.y <=0f)
            scrollToBottom = true;
        else
            scrollToBottom = false;

        var parent = this.GetComponentInParent<DHTLogScreen>();
        if (!parent)
        {
            _logScreen.Log($"OnDrag called with data: {_scrollRect.normalizedPosition}\n");
            // _logScreen.Log($"OnDrag called with data: {eventData}\n");
        }
    }
}

