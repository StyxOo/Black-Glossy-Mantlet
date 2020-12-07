using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    #region Serialized Private Fields
    #endregion

    #region Private Fields
    #endregion

    #region Public Fields
    #endregion

    #region Unity Functions

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            GameManager.Instance.OnLoose();
            enemy.Cheer();
        }
    }

    #endregion

    #region Public Functions
    #endregion

    #region Private Functions
    #endregion
}
