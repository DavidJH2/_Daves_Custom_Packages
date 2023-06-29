using System.Collections;
using System.Collections.Generic;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class GrenadeLauncer : DHTInteractable
{
    [SerializeField] private GameObject   grenadePrefab;
    [SerializeField] private Transform    launchPoint;
    [SerializeField] private float        grenadeVelocity = 5;
    //[SerializeField] private VisualEffect _visualEffect;
    public ParticleSystem _particleSystem;

    [SerializeField] private float ParitcleStartSpeed    = 0.4f;
    [SerializeField] private float MaxParitcleStartSpeed = 2.0f;
    [SerializeField] private float ParitcleStartSpeedIncreaseRate = 2f;


    private float           _chargeTime;
    private TextMeshProUGUI tmp;
    
    // Start is called before the first frame update
    void Start()
    {
        var dt = FindObjectOfType<DebugText>();
        if (dt)
        {
            tmp = dt.GetComponent<TextMeshProUGUI>();
        }
    }

    public void ResetParticleSystem()
    {
        _chargeTime = 0;
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }

    public void ChargeGrenade()
    {
        ParticleSystem.MainModule  psMain  = _particleSystem.main;
        ParticleSystem.ShapeModule psShape = _particleSystem.shape;

        var elapsedTime = Time.fixedDeltaTime;
        _chargeTime += elapsedTime;
        
        var newStartSpeed = Mathf.Min(MaxParitcleStartSpeed, ParitcleStartSpeed + ParitcleStartSpeedIncreaseRate * _chargeTime);
        psMain.startSpeed = newStartSpeed;

    }

    public override void Activate()
    {
        // tmp.text += "Launch Grenade!\n";
        var newGrenade = Instantiate(grenadePrefab, launchPoint.position, Quaternion.identity);
        var rb         = newGrenade.GetComponent<Rigidbody>();

        var newVel = transform.forward * grenadeVelocity;
        rb.velocity = newVel;
    }
}
