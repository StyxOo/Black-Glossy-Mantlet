﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class TrapPlacer : MonoBehaviour
{
    #region Serialized Private Fields
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject buildEffect;
    
    #endregion

    #region Private Fields
    
    private GameObject _currentTrap;
    private GameObject _currentSpell;
    private Camera _camera;
    private bool _selling = false;
    private AudioSource _audio;

    #endregion

    #region Public Fields
    #endregion

    #region Unity Functions

    private void Awake()
    {
        _camera = Camera.main;
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            Cancel();
        }

        if (_currentTrap != null)
        {
            PlaceTrap();
        }
        else if (_currentSpell != null)
        {
            PlaceSpell();
        }
        else
        {
            if (_selling)
            {
                Sell();
            }
        }
    }


    #endregion

    #region Public Functions

    public void SelectTrap(GameObject prefab)
    {
        Cancel();
        _currentTrap = Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }

    public void SelectSpell(GameObject prefab)
    {
        Cancel();
        _currentSpell = Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }


    public void SellMode()
    {
        _selling = true;
    }
    
    public void Cancel()
    {
        if (_currentTrap != null)
        {
            Destroy(_currentTrap);
        }

        if (_currentSpell != null)
        {
            Destroy(_currentSpell);
        }

        _selling = false;
    }
    
    #endregion

    #region Private Functions

    private void Sell()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hit = RayCast();
            if (hit.collider != null)
            {
                if (hit.transform.childCount > 0)
                {
                    var trap = hit.transform.GetComponentInChildren<Trap>();
                    PlayerStats.Instance.AddCoins(Mathf.CeilToInt(trap.SpawnCost / 2f));
                    Destroy(trap.gameObject);
                    Instantiate(buildEffect, trap.transform.position, Quaternion.identity);
                }

                _selling = false;
            }
        }
    }

    private void PlaceTrap()
    {
        var hit = RayCast();
        if (hit.collider != null)
        {
            var point = hit.collider.transform.position;
            point.y = 0f;
            _currentTrap.transform.position = point;
        }
        else
        {
            _currentTrap.transform.position = Vector3.up * 100f;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (hit.collider.GetComponentInChildren<Trap>() != null)
            {
                return;
            }

            SpawnTrap(hit.collider.gameObject);
        }
    }
    
    private void PlaceSpell()
    {
        var hit = RayCast();
        if (hit.collider != null)
        {
            var point = hit.point;
            point.y = 0f;
            _currentSpell.transform.position = point;
        }
        else
        {
            _currentSpell.transform.position = Vector3.up * 100f;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (hit.collider != null)
            {
                var spell = _currentSpell.GetComponent<Spell>();

                var canBuy = PlayerStats.Instance.UseCoins(spell.Cost);
                if (canBuy)
                {
                    _audio.Play();
                    _currentSpell = Instantiate(_currentSpell, Vector3.zero, Quaternion.identity);
                    spell.Place();
                }
            }
            else
            {
                Cancel();
            }
        }
    }

    private RaycastHit RayCast()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out var hit, Mathf.Infinity, groundLayer);
        return hit;
    }
    
    private void SpawnTrap(GameObject floorTile)
    {
        var trap = _currentTrap.GetComponent<Trap>();

        var canBuy = PlayerStats.Instance.UseCoins(trap.SpawnCost);

        if (canBuy)
        {   
            _currentTrap.transform.parent = floorTile.transform;
            _currentTrap.transform.localPosition = Vector3.up;

            Instantiate(buildEffect, _currentTrap.transform.position, Quaternion.identity);
            
            _currentTrap = Input.GetKey(KeyCode.LeftShift) ? Instantiate(_currentTrap, Vector3.zero, Quaternion.identity) : null;
            
            _audio.Play();
            
            trap.enabled = true;
        }
    }


    #endregion
}
