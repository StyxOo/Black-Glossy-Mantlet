using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    #region Serialized Private Fields
    
    [SerializeField] private int value;
    
    #endregion

    #region Private Fields
    
    private Transform _target;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private bool _dead = false;
    private bool _isCheering = false;

    #endregion

    #region Public Fields
    #endregion

    #region Unity Functions

    private void OnEnable()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();

        _target = FindObjectOfType<Goal>().transform;
        _navMeshAgent.SetDestination(_target.position);

    }

    private void Update()
    {
        _animator.SetBool("Walking", _navMeshAgent.velocity.sqrMagnitude > 0f);

        if (_isCheering)
        {
            transform.LookAt(GameManager.Instance.CameraPosition);
        }

        if (transform.position.y < -1f)
        {
            Die(Vector3.zero);
        }
    }

    #endregion

    #region Public Functions
    
    public void Die(Vector3 launchVelocity)
    {
        if (_dead)
        {
            return;
        }

        _dead = true;
        
        var navMeshVelocity = _navMeshAgent.velocity;
        _navMeshAgent.enabled = false;
        _animator.enabled = false;

        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<Rigidbody>());

        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rigidbodies)
        {
            rb.velocity = launchVelocity + navMeshVelocity;
        }
        
        PlayerStats.Instance.AddCoins(value);

        Destroy(gameObject, 10f);
    }
    
    public void Cheer()
    {
        _animator.SetTrigger("Cheer");

        _isCheering = true;
    }
    
    #endregion

    #region Private Functions
    #endregion

}
