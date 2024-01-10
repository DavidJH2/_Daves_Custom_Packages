using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    internal Rigidbody2D rb;
    internal Blastoids gameEngine;
    internal float     time;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
