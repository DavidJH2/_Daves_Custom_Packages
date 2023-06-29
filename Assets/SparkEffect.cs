using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class SparkEffect : MonoBehaviour
{
    [SerializeField] private Transform    SparkSource;
    [SerializeField] private VisualEffect _visualEffect;
    [SerializeField] private float        depth;
    [SerializeField] private int        rate;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _visualEffect.SetVector3("SourceTransform_position" , SparkSource.position);
        _visualEffect.SetVector3("SourceTransform_angles" , SparkSource.transform.eulerAngles);
        _visualEffect.SetInt("Rate", rate);
        _visualEffect.SetFloat("Depth", depth);
    }
}
