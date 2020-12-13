using System;
using System.Collections;
using UnityEngine;

public class DoorTrap : Trap
{

    #region Serialized Private Fields
    
    [SerializeField] private float resetTime;
    [SerializeField] private float closeTime;
    
    #endregion

    #region Private Fields
    
    private float _lastTriggerTime = 0;
    private Animator _animator;
    private bool _isActive = false;

    #endregion

    #region Public Fields
    #endregion

    #region Unity Functions

    protected override void OnEnable()
    {
        base.OnEnable();
        
        _animator = GetComponent<Animator>();

        transform.parent.GetComponent<MeshRenderer>().enabled = false;
        transform.parent.gameObject.layer = 10;

        _isActive = true;
    }

    private void OnDisable()
    {
        transform.parent.GetComponent<MeshRenderer>().enabled = true;
        transform.parent.gameObject.layer = 8;
        
        _isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActive && ReadyToTrigger())
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                Trigger(enemy);
            }
        }
    }


    #endregion

    #region Public Functions

    #endregion

    #region Private Functions
    
    private bool ReadyToTrigger()
    {
        return Time.time > _lastTriggerTime + resetTime;
    }
    
    private void Trigger(Enemy enemy)
    {
        _lastTriggerTime = Time.time;
        enemy.Die(Vector3.zero);

        _animator.SetBool("Open", true);
        // transform.parent.gameObject.layer = 10;

        _audio.Play();

        StartCoroutine(TrapClose());
    }

    private IEnumerator TrapClose()
    {
        yield return new WaitForSeconds(closeTime);
        _animator.SetBool("Open", false);
        // transform.parent.gameObject.layer = 8;
    }
    
    #endregion
}
