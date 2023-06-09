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
    internal                 int         generation = 1;


    void Awake()
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


    private void OnTriggerEnter2D(Collider2D other) // Todo: Add more rocks after all are destroyed
    {
        var go     = other.gameObject;

        var bullet = go.GetComponent<Bullet>();

        if (!gameEngine.SpaceShip.alive)
        {
            return;
        }
        
        if (bullet)
        {
            Destroy(go);
            DestroyThisRock();

            int points;

            switch (generation)
            {
                case 1:
                    points = 10;
                    break;
                case 2:
                    points = 25;
                    break;
                case 3:
                    points = 50;
                    break;
                case 4:
                    points = 100;
                    break;
                default:
                    points = 0;
                    break;
            }

            gameEngine.AddScore((int)points);
        }
        else
        {
            var ship = go.GetComponent<SpaceShip>();

            if (ship)
            {
                gameEngine.PlayerCrashed();
                ship.Explode();
                
                DestroyThisRock();
            }
        }
    }

    
    void DestroyThisRock()
    {
        Destroy(gameObject);
        if (size >= .5)
        {
            CreateTwoNewRocks();
        }
        else
        {
            gameEngine.CheckRoundComplete();
        }
    }


    private void CreateTwoNewRocks()
    {
        var pos = transform.localPosition;
        gameEngine.CreateRock(pos, size / 2, generation + 1);
        gameEngine.CreateRock(pos, size / 2, generation + 1);
    }
}
