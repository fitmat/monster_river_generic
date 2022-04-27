using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TW_ScoreManager : MonoBehaviour
{
    public static TW_ScoreManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public int playerOneTime, playerTwoTime;
    public float playerOneFitnessPoints, playerTwoFitnessPoints;

    [SerializeField] private TMP_Text playerOneNameText, playerTwoNameText;

    private void Start()
    {
        playerOneTime = 0;
        playerTwoTime = 0;
    }

    public void SetScore()
    {
        playerOneNameText.text = TW_GameController.instance.playerOneName;
        playerTwoNameText.text = TW_GameController.instance.playerTwoName;
    }

}
