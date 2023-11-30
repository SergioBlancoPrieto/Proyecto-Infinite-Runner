﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    menu,
    inTheGame,
    gameOver,
    credits
}
public class GameManager : MonoBehaviour
{
    //shareInstance será única y compartida con toda la escena de Unity
    public static GameManager sharedInstance;
    
    //declaramos currentGameState del tipo enumerado GameState y lo inicializamos al valor menu
    public GameState currentGameState = GameState.menu;

    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject creditsCanvas;
	[SerializeField] private GameObject _pauseButton;
	[SerializeField] private GameObject _pauseMenu;

	private int collectObject;

    void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(this);
        }
        else
        {
            sharedInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        ChangeGameState(GameState.menu);
    }

	public void CollectItem(int objectValue)
	{
		collectObject+=objectValue;
		UpdateGameCanvas.sharedInstance.SetCoinsNumber();
	}

    public void StartGame() //Se llama para iniciar la partida
    {
		collectObject = 0;
        PlayerController.sharedInstance.StartGame();
		LevelGenerator.sharedInstance.RemoveAllTheBlocks();
		LevelGenerator.sharedInstance.GenerateInitialBlocks();
		UpdateGameCanvas.sharedInstance.SetRecordPoints();
		UpdateGameCanvas.sharedInstance.SetCoinsNumber();
		UpdateGameCanvas.sharedInstance.RestartHealth();
        ChangeGameState(GameState.inTheGame);
    }
    public void GameOver() //Se llama cuando el jugador muere
    {
        ChangeGameState(GameState.gameOver);
		//LevelGenerator.sharedInstance.RemoveAllTheBlocks();
		UpdateGameOverCanvas.sharedInstance.SetScorePointsAndCoins();
		PlayerController.sharedInstance.StopAllCoroutines();
    }
    
    //lo llamamos cuando el jugador decide finalizar y volver a menú principal
    public void BackToMainMenu()
    {
        ChangeGameState(GameState.menu);
    }

    public void DisplayCreditsMenu()
    {
	    ChangeGameState(GameState.credits);
    }

    void ChangeGameState(GameState newGameState)
    {
        if (newGameState == GameState.menu)
        {
			menuCanvas.SetActive(true);
			gameCanvas.SetActive(false);
			gameOverCanvas.SetActive(false);
			creditsCanvas.SetActive(false);
			_pauseButton.SetActive(false);
			_pauseMenu.SetActive(false);
            //La escena de Unity deberá mostrar el menú principal
            currentGameState = GameState.menu;
        }
        else if (newGameState == GameState.inTheGame)
        {
			menuCanvas.SetActive(false);
			gameCanvas.SetActive(true);
			gameOverCanvas.SetActive(false);
			creditsCanvas.SetActive(false);
			_pauseButton.SetActive(true);
			_pauseMenu.SetActive(false);
            //La escena de Unity deberá configurarse para mostrar el juego en si
            currentGameState = GameState.inTheGame;
            Time.timeScale = 1f;
        }
        else if (newGameState == GameState.gameOver)
        {
			menuCanvas.SetActive(false);
			gameCanvas.SetActive(false);
			gameOverCanvas.SetActive(true);
			creditsCanvas.SetActive(false);
			_pauseButton.SetActive(false);
			_pauseMenu.SetActive(false);
            //La escena de Unity deberá mostrar el menú de fin de partida
            currentGameState = GameState.gameOver;
        }
        else if (newGameState == GameState.credits)
        {
	        menuCanvas.SetActive(false);
	        gameCanvas.SetActive(false);
	        gameOverCanvas.SetActive(false);
	        creditsCanvas.SetActive(true);
	        _pauseButton.SetActive(false);
	        _pauseMenu.SetActive(false);
	        //La escena de Unity deberá mostrar el menú de creditos
	        currentGameState = GameState.credits;
        }
    }

	public int GetCollectedCoins()
    {
        return collectObject;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}