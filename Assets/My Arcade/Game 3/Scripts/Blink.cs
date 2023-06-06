using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Blink : MonoBehaviour
{
    [SerializeField] private float    alternateTime;
    private                  Renderer _renderer;
    private                  Material _material;
    private                  bool     _on = true;
    private                  float    timer;
    
    
    

    void Start()
    {
        timer     = alternateTime;
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
    }

    
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = alternateTime;
            _on   = !_on;

            if (_on)
            {
                _material.EnableKeyword("_EMISSION");
            }
            else
            {
                _material.DisableKeyword("_EMISSION");
            }
        }
    }
}
