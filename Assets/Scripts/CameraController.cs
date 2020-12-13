using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    #region Serialized Private Fields

    [SerializeField] private float autoSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float rotationDelay;

    #endregion

    #region Private Fields

    private Vector3? _startPos;
    private float _startTime;
    private bool _autoTurn = true;
    private GameManager _gm;

    #endregion

    #region Public Fields
    #endregion

    #region Unity Functions

    private void Awake()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, Random.Range(0, 360));
        _gm = GameManager.Instance;
    }

    private void Update()
    {
        if (_gm.IsRunning)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startTime = Time.time;
            }

            if (Input.GetMouseButton(0) && Time.time >= _startTime + rotationDelay)
            {
                _autoTurn = false;
                
                if (!_startPos.HasValue)
                {
                    _startPos = Input.mousePosition;
                }

                var mousePos = Input.mousePosition;
                Rotate(_startPos.Value.x - mousePos.x);
                _startPos = mousePos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _startPos = null;
                _autoTurn = true;
            }
        }

        if (_autoTurn)
        {
            transform.RotateAround(Vector3.zero, Vector3.up,  autoSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region Public Functions
    #endregion

    #region Private Functions

    private void Rotate(float mouseDiff)
    {
        transform.RotateAround(Vector3.zero, Vector3.up, -mouseDiff * speed * Time.deltaTime);
    }

    #endregion
}
