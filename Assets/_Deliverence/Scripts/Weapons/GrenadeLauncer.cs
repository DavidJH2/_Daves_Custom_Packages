using System.Collections;
using System.Collections.Generic;
using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GrenadeLauncer : DHTInteractable
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform  launchPoint;
    [SerializeField] private float      grenadeVelocity = 5;

    private TextMeshProUGUI tmp;
    
    // Start is called before the first frame update
    void Start()
    {
        var dt = FindObjectOfType<DebugText>();
        tmp = dt.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
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
