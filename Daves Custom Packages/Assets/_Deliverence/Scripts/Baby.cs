using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    // Start is called before the first frame update
    private float       timer;
    public AudioSource[] _audioSource;
    void Start()
    {
        timer = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!DeliveranceGameEngine.GamePlaying)
        {
            return;
        }
            
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            var clip = Random.Range(0, _audioSource.Length);
            _audioSource[clip].Play();
            timer = Random.Range(1000, 2000);
        }
    }
}
