using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class DTHLineRenderer : MonoBehaviour
{
    public Vector3[]    points;

    internal LineRenderer _lr;

    private void Awake()
    {
        _lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        _lr.positionCount = points.Length;
        for (var i = 0; i < points.Length; i++)
        {
            var pos = transform.TransformPoint(points[i]);
            _lr.SetPosition(i, pos);
        }
    }
}
