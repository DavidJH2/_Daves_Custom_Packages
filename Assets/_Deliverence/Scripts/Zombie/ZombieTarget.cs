using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class ZombieTarget : MonoBehaviour
{
    [SerializeField] private  ParticleSystem bloodParticles;
    [SerializeField] private  GameObject     blinder;
    [SerializeField] private  GameObject     grenadeLauncher;
    [SerializeField] internal int            StartHealth = 100;

    private XROrigin   _xrOrigin;
    public  int        health;
    private GameEngine _gameEngine;
    private Material   blinderMat;
    
    
    void Start()
    {
        List<Material> mats = new List<Material>();
        health     = StartHealth;
        blinder.GetComponent<Renderer>().GetMaterials(mats);
        blinderMat  = mats[0];
        _gameEngine = FindObjectOfType<GameEngine>();
        _xrOrigin   = GetComponentInParent<XROrigin>();
    }


    
    public void TakeDamage(int damage)
    {
        if (health != 0)
        {
            health = Mathf.Max(0, health - damage);
            // GameEngine.AddDebugText($"Player Hurt!   Health = {health}\n");
            bloodParticles.Play();

            blinderMat.color = new Color(1, 0, 0, ((float)StartHealth - health) / StartHealth / 3.0f);
            if (health == 0)
            {
                //grenadeLauncher.transform.SetParent(null);
                var collider = grenadeLauncher.GetComponent<Collider>();
                collider.enabled = true;
                grenadeLauncher.AddComponent<Rigidbody>();
                
                var gameOverGO = _gameEngine._gameOver;
                gameOverGO.SetActive(true);

                var messageSpawnPos = _xrOrigin.GetComponentInChildren<MessageSpawnPos>();
                var trans             = messageSpawnPos.transform;
                gameOverGO.transform.position = trans.position;
                gameOverGO.transform.rotation = trans.rotation;
            }
        }
    }

    
    void Update()
    {
        
    }
}
