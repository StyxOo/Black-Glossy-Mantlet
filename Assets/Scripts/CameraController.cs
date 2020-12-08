using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Serialized Private Fields

    [SerializeField] private float speed;
    [SerializeField] private float rotationDelay;

    #endregion

    #region Private Fields

    private Vector3 _startPos;
    private float _startTime;

    #endregion

    #region Public Fields
    #endregion

    #region Unity Functions

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startTime = Time.time;
            _startPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && Time.time >= _startTime + rotationDelay)
        {
            var mousePos = Input.mousePosition;
            Rotate(_startPos.x - mousePos.x);
            _startPos = mousePos;
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
