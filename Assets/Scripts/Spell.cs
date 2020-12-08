using System;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private int cost = 5;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] private float triggerDelay;
    
    private bool _armed = false;
    private float _armingTime;
    
    public int Cost => cost;

    public virtual void Place()
    {
        _armed = true;
        _armingTime = Time.time;
    }
    
    
    protected virtual void Update()
    {
        if (ReadyToExplode())
        {
            Trigger();
        }
    }
    
        
    protected virtual void Trigger()
    {
        throw new NotImplementedException();
    }

    private bool ReadyToExplode()
    {
        if (!_armed)
        {
            return false;
        }

        return Time.time > _armingTime + triggerDelay;
    }

}