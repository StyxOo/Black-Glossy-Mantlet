using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Serialized Private Fields
    
    [SerializeField] private GameObject inGameUi;
    [SerializeField] private GameObject newHighscoreUI;
    [SerializeField] private TMP_Text newHighscoreText;
    [SerializeField] private GameObject endScreenUI;
    [SerializeField] private TMP_Text endScoreText;
    [SerializeField] private TMP_Text highscoreText;
    [SerializeField] private GameObject mutedButton;
    [SerializeField] private GameObject unmutedButton;

    #endregion

    #region Private Fields
    #endregion

    #region Public Fields

    public static UIManager Instance { get; private set; }
    
    #endregion

    #region Unity Functions

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        var muted = GameManager.Instance.Muted;
        mutedButton.SetActive(muted);
        unmutedButton.SetActive(!muted);
    }

    #endregion

    #region Public Functions

    public void NewHighscore(int score)
    {
        inGameUi.SetActive(false);
        newHighscoreUI.SetActive(true);
        newHighscoreText.text = score.ToString();
    }

    public void EndScreen(int score, int highscore)
    {
        inGameUi.SetActive(false);
        endScreenUI.SetActive(true);
        endScoreText.text = score.ToString();
        highscoreText.text = highscore.ToString();
    }

    public void Mute()
    {
        GameManager.Instance.Mute();
        var muted = GameManager.Instance.Muted;
        mutedButton.SetActive(muted);
        unmutedButton.SetActive(!muted);
    }

    #endregion

    #region Private Functions
    #endregion
}
