using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateGameOverCanvas : MonoBehaviour
{
    public static UpdateGameOverCanvas sharedInstance;
    
    public TextMeshProUGUI coinsNumber;
    public TextMeshProUGUI scorePoints;

    private void Awake()
    {
        sharedInstance = this;
    }

    public void SetScorePointsAndCoins()
    {
        scorePoints.text = PlayerController.sharedInstance.GetDistanceTravelled().ToString("f0");
        coinsNumber.text = GameManager.sharedInstance.GetCollectedCoins().ToString();
    }
}
