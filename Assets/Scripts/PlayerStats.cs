using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    #region Serialized Private Fields
    
    [SerializeField] private int startCoins;
    [SerializeField] private TMP_Text coinText;
    
    #endregion

    #region Private Fields
    
    private int _coins = 50;

    #endregion

    #region Public Fields

    public static PlayerStats Instance { get; private set; }
    
    public int Score => _coins;

    #endregion

    #region Unity Functions

    private void Awake()
    {
        Instance = this;
        _coins = startCoins;
        
        UpdateCoinUi();
    }

    #endregion

    #region Public Functions

    public void AddCoins(int amount)
    {
        _coins += amount;
        UpdateCoinUi();
    }

    public bool UseCoins(int amount)
    {
        if (_coins >= amount)
        {
            _coins -= amount;
            UpdateCoinUi();
            return true;
        }

        return false;
    }
    
    #endregion

    #region Private Functions

    private void UpdateCoinUi()
    {
        coinText.text = _coins.ToString();
    }
    
    #endregion
}
