using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Serialized Private Fields

    [ReorderableList] [SerializeField] private List<Stage> stages;
    
    #endregion

    #region Private Fields
    
    private Camera _camera;
    private bool _lost = false;
    private int _stageIndex = 0;
    
    #endregion

    #region Public Fields

    public static GameManager Instance;

    public Vector3 CameraPosition { get; private set; }

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
        
        ActivateNextStage();
    }

    private void Update()
    {
        var pos = _camera.transform.position;
        pos.y = 0f;
        CameraPosition = pos;

        if (stages.Count > _stageIndex)
        {
            if (stages[_stageIndex - 1].IsActive && Time.time > stages[_stageIndex - 1].ActiveTime + stages[_stageIndex].ActivationTime)
            {
                ActivateNextStage();
            }
        }
    }

    #endregion

    #region Public Functions
    
    public void OnLoose()
    {
        if (_lost)
        {
            return;
        }

        _lost = true;
        Debug.Log($"You reached a score of {PlayerStats.Instance.Score}");
    }

    #endregion

    #region Private Functions

    private void ActivateNextStage()
    {
        StartCoroutine(stages[_stageIndex].Enable());
        _stageIndex++;
    }
    
    #endregion
}
