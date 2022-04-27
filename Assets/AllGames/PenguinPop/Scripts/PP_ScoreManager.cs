using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/*
 * Score Manager script to manage in game score and final game result
 * */
public class PP_ScoreManager : MonoBehaviour
{
    public static PP_ScoreManager instance;

    public TMP_Text player1ScoreText, player2ScoreText, gameTimeText;
    public TMP_Text player1NameText, player2NameText, player1FinalScoreText, player2FinalScoreText, player1JumpsText, player2JumpsText, player1FitnessPointsText, player2FitnessPointsText;
    public string player1Name, player2Name;
    public int player1Jumps, player2Jumps;
    public float player1FitnessPoints, player2FitnessPoints;
    public int player1Score, player2Score, gameTime;

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
        player1Name = "Abrar";
        player2Name = "Anonymous";
    }

    // Update score when enemy object passes through trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            player1Score += 1;
            player2Score += 1;
            UpdatePlayerScore();
        }
    }

    //  Reflect updated score in UI
    private void UpdatePlayerScore()
    {
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }

    // Coroutine to handle in game clock
    public IEnumerator GameTimer()
    {
        //gameTimeText.text = ((int)(gameTime / 60)).ToString("00") + ":" + ((int)(gameTime % 60)).ToString("00");
        yield return new WaitForSecondsRealtime(1f);
        // Increment timer to count time if game is in endless mode
        gameTime++;

        if (PP_GameController.instance.isGameRunning && !PP_GameUIController.instance.isPaused)
        {
            // Recall coroutine if game is still running
            StartCoroutine(GameTimer());
        }
    }

    // Function to reset all score values to 0 on game start
    public void ResetScoreValues()
    {
        player1Score = 0;
        player2Score = 0;
        player1FitnessPoints = 0;
        player2FitnessPoints = 0;
        UpdatePlayerScore();
        // Set game timer and start clock
        gameTime = 0;

    }

}
