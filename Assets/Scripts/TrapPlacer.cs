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
    private GameObject _currentSpell;
    private Camera _camera;
    private bool _selling = false;

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
                    var trap = hit.transform.GetComponentInChildren<SpikeTrap>();
                    PlayerStats.Instance.AddCoins(Mathf.CeilToInt(trap.SpawnCost / 2f));
                    Destroy(trap.gameObject);
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
    
    private void PlaceSpell()
    {
        var hit = RayCast();
        if (hit.collider != null)
        {
            _currentSpell.transform.position = hit.point;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            var spell = _currentSpell.GetComponent<Spell>();

            var canBuy = PlayerStats.Instance.UseCoins(spell.Cost);
            if (canBuy)
            {
                _currentSpell = Input.GetKey(KeyCode.LeftShift)
                    ? Instantiate(_currentSpell, Vector3.zero, Quaternion.identity)
                    : null;
                spell.Place();
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
        var trap = _currentTrap.GetComponent<SpikeTrap>();

        var canBuy = PlayerStats.Instance.UseCoins(trap.SpawnCost);

        if (canBuy)
        {   
            _currentTrap.transform.parent = floorTile.transform;
            _currentTrap.transform.localPosition = Vector3.up;
            
            _currentTrap = Input.GetKey(KeyCode.LeftShift) ? Instantiate(_currentTrap, Vector3.zero, Quaternion.identity) : null;
            
            trap.enabled = true;
        }
    }


    #endregion
}
