using System;
using System.Collections;
using System.Collections.Generic;
using Arcade.Game_3.Scripts;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] private float       RockSpeed = 2;
    internal                 Rigidbody2D rb;
    internal                 Blastoids   gameEngine;
    internal                 float       size;

    
    void Start()
    {
        rb         = GetComponent<Rigidbody2D>();
        gameEngine = GetComponentInParent<Blastoids>();
        InitRock();
    }

    
    private void InitRock()
    {
        var ang   = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
        var speed = RockSpeed;
        var vx    = Mathf.Cos(ang) * speed;
        var vy    = Mathf.Sin(ang) * speed;
        rb.velocity = new Vector3(vx, vy, 0);
    }


    void Update()
    {
        gameEngine.WrapPosition(rb);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        var go     = other.gameObject;
        var bullet = go.GetComponent<Bullet>();

        if (bullet)
        {
            Destroy(go);
            CreateTwoNewRocks();            
        }
        else
        {
            var ship = go.GetComponent<SpaceShip>();

            if (ship)
            {
                ship.Explode();
                CreateTwoNewRocks();
            }
        }
    }

    private void CreateTwoNewRocks()
    {
        if (size >= .5)
        {
            var pos = transform.localPosition;
            gameEngine.CreateRock(pos, size / 2);
            gameEngine.CreateRock(pos, size / 2);
            gameEngine.PlayerCrashed();
        }

        Destroy(gameObject);
    }
}
