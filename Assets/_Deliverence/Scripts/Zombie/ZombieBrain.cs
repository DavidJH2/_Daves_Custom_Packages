using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieBrain : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    
    private ZombieTarget target;
    
    void Start()
    {
        target = FindObjectOfType<ZombieTarget>();
        tmp    = FindObjectOfType<DebugText>().GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        var delta = target.transform.position - transform.position;
        delta.y = 0;

        var ang = Vector3.SignedAngle(transform.forward, delta, Vector3.up);
        //tmp.text = $"Ang: {(int) (ang )}";

        var newRot = transform.rotation * Quaternion.Euler(0, ang * .01f, 0);

        transform.rotation = newRot;
    }
}
