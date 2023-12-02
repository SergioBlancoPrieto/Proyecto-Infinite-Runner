using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{
    public Transform exitPoint;
    [SerializeField] private GameObject _potion;

    private void OnEnable()
    {
        _potion?.SetActive(false);
    }

    private void OnDisable()
    {
        _potion?.SetActive(false);
    }

    public void SpawPotion()
    {
        _potion?.SetActive(true);
    }
}
