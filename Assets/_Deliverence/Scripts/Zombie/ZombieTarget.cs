using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieTarget : MonoBehaviour
{
    [SerializeField] private  ParticleSystem bloodParticles;
    [SerializeField] private  GameObject     blinder;
    [SerializeField] internal int            StartHealth = 100;
    public                    int            health;

    void Start()
    {
        health = StartHealth;
    }


    
    public void TakeDamage(int damage)
    {
        if (health != 0)
        {
            health = Mathf.Max(0, health - damage);
            GameEngine.AddDebugText($"Player Hurt!   Health = {health}\n");
            bloodParticles.Play();

            if (health == 0)
            {
                blinder.SetActive(true);
            }
        }
    }

    
    void Update()
    {
        
    }
}
