using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Stage : MonoBehaviour
{

    [ReorderableList] [SerializeField] private List<GameObject> floorTiles = new List<GameObject>();
    [SerializeField] private Spawner spawner;
    [SerializeField] private float activationTime = 0f;
    
    private float blockDelay = .2f;

    public float ActivationTime => activationTime;
    public float ActiveTime { get; private set; } = 0f;
    public bool IsActive { get; private set; }

    public void Hide()
    {
        foreach (var tile in floorTiles)
        {
            tile.SetActive(false);
        }

        spawner.enabled = false;
    }

    public IEnumerator Enable()
    {
        yield return new WaitForSeconds(blockDelay);

        if (floorTiles.Count > 0)
        {
            floorTiles[0].SetActive(true);
            floorTiles.RemoveAt(0);
            StartCoroutine(Enable());
        }
        else
        {
            spawner.enabled = true;
            ActiveTime = Time.time;
            IsActive = true;
        }
    }
}