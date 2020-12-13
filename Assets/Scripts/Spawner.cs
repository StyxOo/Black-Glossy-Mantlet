using System;
using UnityEngine;
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
    private ParticleSystem _particleSystem;

    #endregion

    #region Public Fields
    #endregion

    #region Unity Functions

    private void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;

        _particleSystem = GetComponentInChildren<ParticleSystem>();
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
        _particleSystem.Play();
    }
    
    #endregion
}
