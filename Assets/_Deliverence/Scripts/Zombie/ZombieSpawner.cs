using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private int          NumZombies;
    [SerializeField] private GameObject[] ZombiePrefabs;
    [SerializeField] private Terrain      _terrain;

    private BoxCollider _boxCollider;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _terrain     = FindObjectOfType<Terrain>();
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
            var x        = pos.x + Random.Range(-range.x / 2, range.x / 2);
            var y        = 0;
            var z        = pos.z + Random.Range(-range.z / 2, range.z / 2);
            var spawnPos = new Vector3(x, y, z);

            var th = _terrain.SampleHeight(spawnPos);
            spawnPos.y = th;
            
            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
