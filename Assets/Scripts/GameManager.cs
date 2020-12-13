using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Serialized Private Fields

    [SerializeField] private float winTime;
    [ReorderableList] [SerializeField] private List<Stage> stages;
    [SerializeField] private GameObject gameAudio;
    
    #endregion

    #region Private Fields
    
    private Camera _camera;
    private bool _lost = false;
    private int _stageIndex = 0;
    private List<Spawner> _spawners = new List<Spawner>();
    
    #endregion

    #region Public Fields

    public static GameManager Instance;

    public Vector3 CameraPosition { get; private set; }
    public bool IsRunning { get; private set; }
    public bool Muted { get; private set; }

    #endregion

    #region Unity Functions

    private void Awake()
    {
        Instance = this;

        _camera = Camera.main;

        foreach (var stage in stages)
        {
            stage.Hide();
        }

        IsRunning = false;

        Muted = PlayerPrefs.GetInt("Mute", 1) > 0;
        gameAudio.SetActive(Muted);
    }

    private void Update()
    {
        if (IsRunning)
        {
            var pos = _camera.transform.position;
            pos.y = 0f;
            CameraPosition = pos;

            if (!_lost && stages.Count > _stageIndex)
            {
                if (stages[_stageIndex - 1].IsActive &&
                    Time.time > stages[_stageIndex - 1].ActiveTime + stages[_stageIndex].ActivationTime)
                {
                    ActivateNextStage();
                }
            }
        }
    }

    #endregion

    #region Public Functions

    public void StartGame()
    {
        IsRunning = true;
        ActivateNextStage();
        StartCoroutine(WinTimer());
    }


    public void OnLoose()
    {
        if (_lost)
        {
            return;
        }

        _lost = true;
        
        GetComponent<AudioSource>().Play();

        var score = PlayerStats.Instance.Score;
        Debug.Log($"You reached a score of {score}");

        var highscore = PlayerPrefs.GetInt("Highscore", 0);

        if (score > highscore)
        {
            Debug.Log("New Highscore!!");
            PlayerPrefs.SetInt("Highscore", score);
            UIManager.Instance.NewHighscore(score);
        }
        else
        {
            UIManager.Instance.EndScreen(score, highscore);
        }

        foreach (var spawner in _spawners)
        {
            Destroy(spawner);
        }
    }

    public void AddSpawner(Spawner spawner)
    {
        _spawners.Add(spawner);
    }

    public void Reload()
    {
        SceneManager.LoadScene(0);
    }

    public void Mute()
    {
        Muted = !Muted;
        gameAudio.SetActive(Muted);
        PlayerPrefs.SetInt("Mute", Muted ? 1 : 0);
    }

    #endregion

    #region Private Functions

    private void ActivateNextStage()
    {
        Debug.Log($"Activate Stage {stages[_stageIndex].name}");
        StartCoroutine(stages[_stageIndex].Enable());
        _stageIndex++;
    }

    private IEnumerator WinTimer()
    {
        yield return new WaitForSeconds(winTime);
        OnLoose();
    }

    #endregion
}
