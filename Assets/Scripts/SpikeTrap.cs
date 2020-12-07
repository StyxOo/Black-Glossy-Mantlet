using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{

    #region Serialized Private Fields
    
    [SerializeField] private float trapResetTime = 3f;
    [SerializeField] private float trapResetCompleteTime = 1f;
    [SerializeField] private float launchForce = 3f;
    [SerializeField] private int spawnCost = 10;
    
    #endregion

    #region Private Fields
    
    private List<Enemy> _enemies = new List<Enemy>();
    private Animator _animator;
    private bool _canTrigger = true;

    #endregion

    #region Public Fields

    public int SpawnCost => spawnCost;
    
    #endregion

    #region Unity Functions

    private void OnEnable()
    {
        _enemies = new List<Enemy>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_canTrigger)
        {
            Trigger();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            _enemies.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            _enemies.Remove(enemy);
        }
    }

    #endregion

    #region Public Functions
    
    #endregion

    #region Private Functions

    [Button]
    private void Trigger()
    {
        if (!_canTrigger)
        {
            return;
        }
        
        _animator.SetBool("Active", true);
        
        foreach (var enemy in _enemies)
        {
            enemy.Die(Vector3.up * launchForce);
        }

        _enemies = new List<Enemy>();

        StartCoroutine(TrapReset());

        _canTrigger = false;
    }

    private IEnumerator TrapReset()
    {
        yield return new WaitForSeconds(trapResetTime);
        _animator.SetBool("Active", false);
        yield return new WaitForSeconds(trapResetCompleteTime);
        ResetTrap();
    }

    private void ResetTrap()
    {
        _canTrigger = true;
    }
    
    #endregion

}
