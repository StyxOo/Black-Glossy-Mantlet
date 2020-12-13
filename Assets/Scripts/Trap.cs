using System;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int spawnCost = 10;
    
    protected AudioSource _audio;
    
    public int SpawnCost => spawnCost;

    protected virtual void OnEnable()
    {
        _audio = GetComponent<AudioSource>();
    }
}