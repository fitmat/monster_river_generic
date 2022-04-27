using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/*
 * Score Manager script to manage in game score and final game result
 * */
public class CZ_ScoreManager : MonoBehaviour
{
    public static CZ_ScoreManager instance;

    public TMP_Text playerScoreText, playerJumpCountText, targetText, gameTimeText;

    public TMP_Text playerNameText,finalJumpCountText,  finalScoreText, fitnessPointsText;
    
    public string playerName;
    public int jumpTarget;
    public float timeLimit;
    public bool isTargetReached;
    public int playerJumps, playerFitnessPoints;
    public int playerScore, gameTime;

    private void Awake()
    {
        // Declare class as Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        playerJumps = 0;
        isTargetReached = false;
        targetText.text = "Target: " + jumpTarget.ToString() + " Jumps";
    }

    // Update score when enemy object passes through trigger
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            playerScore += 100;
            UpdatePlayerScore();
        }
    }*/

    //  Reflect updated score in UI
    private void UpdatePlayerScore()
    {
        playerScoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateJumpCount(int value)
    {
        playerJumps += value;
        playerJumpCountText.text = "Jumps: " + playerJumps.ToString();
        if (playerJumps == jumpTarget)
        {
            isTargetReached = true;
        }
    }

    // Coroutine to handle in game clock
    private IEnumerator GameTimer()
    {
        gameTimeText.text = ": " + ((int)(gameTime / 60)).ToString("00") + ":" + ((int)(gameTime % 60)).ToString("00");
        yield return new WaitForSecondsRealtime(1f);
        gameTime--;

        if (CZ_GameController.instance.isGameRunning)
        {
            // Recall coroutine if game is still running
            StartCoroutine(GameTimer());
        }
    }

    // Function to reset all score values to 0 on game start
    public void ResetScoreValues()
    {
        playerName = "Player 1";
        playerScore = 0;
        playerFitnessPoints = 0;
        gameTime = Convert.ToInt32(timeLimit);
        StartCoroutine(GameTimer());
    }

    // Function to finalize score calculations and update values in UI
    public void SetScoreValues()
    {
        playerNameText.text = playerName;
        finalScoreText.text = playerScore.ToString();
        finalJumpCountText.text = playerJumps.ToString();
        playerFitnessPoints = playerJumps * 100;
        fitnessPointsText.text = playerFitnessPoints.ToString();
    }
}
