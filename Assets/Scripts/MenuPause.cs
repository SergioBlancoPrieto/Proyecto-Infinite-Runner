using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameCanvas;
    [SerializeField] private GameObject _gameManager;
    
    public void Pausa()
    {
        Time.timeScale = 0f;
        _pauseButton.SetActive(false);
        _pauseMenu.SetActive(true);
        _gameCanvas.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        _pauseButton.SetActive(true);
        _pauseMenu.SetActive(false);
        _gameCanvas.SetActive(true);
    }

    public void MainMenu()
    {
        _pauseButton.SetActive(true);
        _gameCanvas.SetActive(true);
        _gameManager.gameObject.GetComponent<GameManager>().BackToMainMenu();
    }
}