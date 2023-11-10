using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    menu,
    inTheGame,
    gameOver
}
public class GameManager : MonoBehaviour
{
    //shareInstance será única y compartida con toda la escena de Unity
    public static GameManager sharedInstance;
    
    //declaramos currentGameState del tipo enumerado GameState y lo inicializamos al valor menu
    public GameState currentGameState = GameState.menu;

	public Canvas menuCanvas;
	public Canvas gameCanvas;
	public Canvas gameOverCanvas;
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

    /*private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentGameState != GameState.inTheGame)
            {
               StartGame(); 
            }
        }
    }*/

	public void CollectItem(int objectValue)
	{
		collectObject+=objectValue;
		UpdateGameCanvas.sharedInstance.SetCoinsNumber();
	}

    public void StartGame() //Se llama para iniciar la partida
    {
		collectObject = 0;
		LevelGenerator.sharedInstance.GenerateInitialBlocks();
		UpdateGameCanvas.sharedInstance.SetRecordPoints();
		UpdateGameCanvas.sharedInstance.SetCoinsNumber();
        PlayerController.sharedInstance.StartGame();
        ChangeGameState(GameState.inTheGame);
    }
    public void GameOver() //Se llama cuando el jugador muere
    {
        ChangeGameState(GameState.gameOver);
		LevelGenerator.sharedInstance.RemoveAllTheBlocks();
		UpdateGameOverCanvas.sharedInstance.SetScorePointsAndCoins();
    }
    
    //lo llamamos cuando el jugador decide finalizar y volver a menú principal
    public void BackToMainMenu()
    {
        ChangeGameState(GameState.menu);
    }

    void ChangeGameState(GameState newGameState)
    {
        if (newGameState == GameState.menu)
        {
			menuCanvas.enabled = true;
			menuCanvas.GetComponent<AudioSource>().Play();
			gameCanvas.enabled = false;
			gameCanvas.GetComponent<AudioSource>().Stop();
			gameOverCanvas.enabled = false;
			gameOverCanvas.GetComponent<AudioSource>().Stop();
			_pauseButton.SetActive(false);
			_pauseMenu.SetActive(false);
            //La escena de Unity deberá mostrar el menú principal
            currentGameState = GameState.menu;
        }
        else if (newGameState == GameState.inTheGame)
            {
				menuCanvas.enabled = false;
				menuCanvas.GetComponent<AudioSource>().Stop();
				gameCanvas.enabled = true;
				gameCanvas.GetComponent<AudioSource>().Play();
				gameOverCanvas.enabled = false;
				gameOverCanvas.GetComponent<AudioSource>().Stop();
				_pauseButton.SetActive(true);
				_pauseMenu.SetActive(false);
                //La escena de Unity deberá configurarse para mostrar el juego en si
                currentGameState = GameState.inTheGame;
                Time.timeScale = 1f;
            }
            else if (newGameState == GameState.gameOver)
            {
				menuCanvas.enabled = false;
				menuCanvas.GetComponent<AudioSource>().Stop();
				gameCanvas.enabled = false;
				gameCanvas.GetComponent<AudioSource>().Stop();
				gameOverCanvas.enabled = true;
				gameOverCanvas.GetComponent<AudioSource>().Play();
				_pauseButton.SetActive(false);
				_pauseMenu.SetActive(false);
                //La escena de Unity deberá mostrar el menú de fin de partida
                currentGameState = GameState.gameOver;
            }
    }

	public int GetCollectedCoins()
    {
        return collectObject;
    }
}