using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{

    #region Serialized Private Fields
    
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private int maxSpawns;
    [SerializeField] private float spawnDelay;
    
    #endregion
    

    #region Private Fields
    
    private float _nextSpawnTime = 0;
    private int _spawned = 0;

    #endregion

    #region Public Fields
    #endregion

    #region Unity Functions

    private void OnEnable()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void Update()
    {
        if (CanSpawn())
        {
            Spawn();
        }
    }


    #endregion

    #region Public Functions
    #endregion

    #region Private Functions
    
    private bool CanSpawn()
    {
        return Time.time > _nextSpawnTime && _spawned < maxSpawns;
    }
    
    private void Spawn()
    {
        var prefabIndex = Random.Range(0, prefabs.Length);
        Instantiate(prefabs[prefabIndex], transform.position, transform.rotation);
        
        _nextSpawnTime = Time.time + spawnDelay;
        _spawned++;
    }
    
    #endregion
}
