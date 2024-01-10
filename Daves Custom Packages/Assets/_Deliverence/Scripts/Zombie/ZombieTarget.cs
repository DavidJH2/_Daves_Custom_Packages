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

    public                                  float Radius;
    [FormerlySerializedAs("health")] public int   Health;
    
    private XROrigin   _xrOrigin;
    private DeliveranceGameEngine _deliveranceGameEngine;
    private Material   blinderMat;
    
    
    void Start()
    {
        List<Material> mats = new List<Material>();
        Health     = StartHealth;

        if (blinder != null)
        {
            blinder.GetComponent<Renderer>().GetMaterials(mats);
            blinderMat = mats[0];
        }

        _deliveranceGameEngine = FindObjectOfType<DeliveranceGameEngine>();
        _xrOrigin   = GetComponentInParent<XROrigin>();
    }


    public float Dist(Vector3 pos)
    {
        return Vector3.Distance(pos, transform.position);
    }

    public void TakeDamage(int damage)
    {
        var baby = GetComponentInParent<Baby>();

        if (baby != null)
        {
            _deliveranceGameEngine.EndGame("The Zombies Got Your Baby!");
        }
        else
        {
            if (Health != 0)
            {
                Health = Mathf.Max(0, Health - damage);
                // GameEngine.AddDebugText($"Player Hurt!   Health = {Health}\n");
                bloodParticles.Play();

                blinderMat.color = new Color(1, 0, 0, ((float)StartHealth - Health) / StartHealth / 3.0f);
                if (Health == 0)
                {
                    //grenadeLauncher.transform.SetParent(null);
                    var collider = grenadeLauncher.GetComponent<Collider>();
                    collider.enabled = true;
                    grenadeLauncher.AddComponent<Rigidbody>();

                    _deliveranceGameEngine.EndGame("You Died!");
                }
            }
        }
    }


    void Update()
    {
        
    }
}
