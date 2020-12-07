using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Serialized Private Fields

    [SerializeField] private List<Stage> stages;
    
    #endregion

    #region Private Fields
    
    private Camera _camera;
    private bool _lost = false;
    
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
    }

    private void Update()
    {
        var pos = _camera.transform.position;
        pos.y = 0f;
        CameraPosition = pos;

        if (stages.Count > 0)
        {
            if (Time.time > stages[0].activationTime)
            {
                stages[0].Enable();
                stages.RemoveAt(0);
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
    
    
    #endregion
    
    [Serializable]
    private class Stage
    {
        public List<GameObject> gameObjects = new List<GameObject>();
        public float activationTime = 0f;

        public void Enable()
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
