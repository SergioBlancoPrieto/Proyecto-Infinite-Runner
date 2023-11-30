using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private GameObject _resumeButton;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameCanvas;
    [SerializeField] private GameObject _gameManager;
    [SerializeField] private GameObject _pauseMenuButton;
    public static MenuPause sharedInstance;

    private void Awake()
    {
        sharedInstance = this;
    }

    public void Pausa()
    {
        EventSystem.current.SetSelectedGameObject(_resumeButton);
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);
        _gameCanvas.SetActive(false);
    }

    public void Resume()
    {
        EventSystem.current.SetSelectedGameObject(_pauseMenuButton);
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
        _gameCanvas.SetActive(true);
    }

    public void MainMenu()
    {
        _gameCanvas.SetActive(true);
        _gameManager.gameObject.GetComponent<GameManager>().BackToMainMenu();
    }
}
