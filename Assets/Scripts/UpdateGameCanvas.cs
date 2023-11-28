using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateGameCanvas : MonoBehaviour
{
	public static UpdateGameCanvas sharedInstance;

    public TextMeshProUGUI coinsNumber;
    public TextMeshProUGUI scorePoints;
	public TextMeshProUGUI recordPoints;

	[SerializeField] private Sprite vidaLlena;
	[SerializeField] private Sprite vidaVacia;

	public Image[] vidas;

	private void Awake() 
	{
		sharedInstance = this;
	}

	public void SetRecordPoints()
	{
		recordPoints.text = PlayerPrefs.GetFloat("highscore", 0).ToString("f0");
	}

	public void SetCoinsNumber()
	{
		coinsNumber.text = GameManager.sharedInstance.GetCollectedCoins().ToString();
	}

    // Update is called once per frame
    void Update()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inTheGame)
        {
            scorePoints.text = PlayerController.sharedInstance.GetDistanceTravelled().ToString("f0");
        }
    }

    public void AddHealth(int indice)
    {
	    ChangeContainer(indice, true);
    }

    public void TakeHealth(int indice)
    {
		ChangeContainer(indice, false);
    }

    public void RestartHealth()
    {
	    for (int i = 0; i < vidas.Length; i++)
	    {
			ChangeContainer(i, true);
	    }
    }

	private void ChangeContainer(int indice, bool estado) 
	{
		var spriteEstado = estado ? vidaLlena : vidaVacia;
		if (indice >= 0 && indice < vidas.Length) 
		{
			vidas[indice].sprite = spriteEstado;
		}
	}
}
