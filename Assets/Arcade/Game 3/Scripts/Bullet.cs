using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    internal Rigidbody rb;
    internal Blastoids gameEngine;
    internal float     time;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            Destroy(this.gameObject);
            return;
        }
        
        gameEngine.WrapPosition(rb);
    }
}
