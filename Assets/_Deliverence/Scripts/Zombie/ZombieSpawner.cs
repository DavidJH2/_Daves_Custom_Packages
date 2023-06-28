using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private int          NumZombies;
    [SerializeField] private GameObject[] ZombiePrefabs;

    private BoxCollider _boxCollider;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        SpawnZombies();
    }

    private void SpawnZombies()
    {
        var pos   = transform.position;
        var range = _boxCollider.size;
        
        var numPrefabs = ZombiePrefabs.Length;
        for (var i = 0; i < NumZombies; i++)
        {

            var pfIndex  = Random.Range(0, numPrefabs - 1);
            var prefab   = ZombiePrefabs[pfIndex];
            var x    = pos.x + Random.Range(-range.x, range.x);
            var y        = 0f;
            var z    = pos.z + Random.Range(-range.z, range.z);
            var spawnPos = new Vector3(x, y, z);

            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
