using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SpikeTrap : Trap
{
    #region Serialized Private Fields
    
    [SerializeField] private float trapResetTime = 3f;
    [SerializeField] private float trapResetCompleteTime = 1f;
    [SerializeField] private float launchForce = 3f;

    #endregion

    #region Private Fields
    
    private List<Enemy> _enemies = new List<Enemy>();
    private Animator _animator;
    private bool _canTrigger = false;
    private ParticleSystem _hitEffect;

    #endregion

    #region Public Fields
    #endregion

    #region Unity Functions

    protected override void OnEnable()
    {
        base.OnEnable();
        
        _enemies = new List<Enemy>();
        _animator = GetComponent<Animator>();
        _hitEffect = GetComponentInChildren<ParticleSystem>();

        StartCoroutine(StartDelay());
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

        if (_enemies.Count > 0)
        {
            _hitEffect.Play();
        }
        
        _audio.Play();

        _enemies = new List<Enemy>();

        StartCoroutine(TrapReset());

        _canTrigger = false;
    }

    private IEnumerator TrapReset()
    {
        yield return new WaitForSeconds(trapResetTime);
        _animator.SetBool("Active", false);
        yield return new WaitForSeconds(trapResetCompleteTime);
        _audio.Play();
        ResetTrap();
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(trapResetTime);
        ResetTrap();
    }

    private void ResetTrap()
    {
        _canTrigger = true;
    }
    
    #endregion

}
