using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlacer : MonoBehaviour
{
    #region Serialized Private Fields
    
    [SerializeField] private LayerMask groundLayer;
    
    #endregion

    #region Private Fields
    
    private GameObject _currentTrap;
    private Camera _camera;
    
    #endregion

    #region Public Fields
    #endregion

    #region Unity Functions

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        HandleCancel();
        if (_currentTrap != null)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, groundLayer))
            {
                if (hit.collider != null)
                {
                    _currentTrap.transform.position = hit.collider.transform.position + Vector3.up;
                }

                if (Input.GetMouseButtonDown(0))
                {

                    if (hit.collider.GetComponentInChildren<SpikeTrap>() != null)
                    {
                        return;
                    }

                    SpawnTrap(hit.collider.gameObject);
                }
            }
        }
    }


    #endregion

    #region Public Functions

    public void SelectTrap(GameObject prefab)
    {
        _currentTrap = Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }
    
    #endregion

    #region Private Functions
    
    private void SpawnTrap(GameObject floorTile)
    {
        var trap = _currentTrap.GetComponent<SpikeTrap>();

        var canBuy = PlayerStats.Instance.UseCoins(trap.SpawnCost);

        if (canBuy)
        {   
            _currentTrap.transform.parent = floorTile.transform;
            _currentTrap.transform.localPosition = Vector3.up;
            trap.enabled = true;

            _currentTrap = null;
        }
    }
    
    private void HandleCancel()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (_currentTrap != null)
            {
                Destroy(_currentTrap);
            }
        }
    }
    
    #endregion
}
