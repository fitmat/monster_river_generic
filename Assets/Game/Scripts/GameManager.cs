using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RunnerGame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; set; }

        [SerializeField] private TextMeshProUGUI scoreText, coinText, scoreModifierText;
        [SerializeField] private int COIN_SCORE_AMOUNT = 5;
        [SerializeField] private GameObject gameOverScreen;

        public bool isDied { get; set; }
        public bool isGameStarted = false;
        private PlayerController playerController;
        private float score, coin, modifierInfo;
        private int lastScore;

        private void Awake()
        {
            Instance = this;

            score = 0;
            coin = 0;
            lastScore = (int)score;
            modifierInfo = 1f;
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            scoreModifierText.text = Convert.ToInt32(modifierInfo).ToString() + "X";
            scoreText.text = Convert.ToInt32(score).ToString();
            coinText.text = Convert.ToInt32(coin).ToString();
        }

        private void Update()
        {
            //TODO change this to 3,2,1...Go Coroutine
            if (TouchInputController.Instance.Tap && !isGameStarted)
            {
                isGameStarted = true;
                playerController.StartRunning();
            }
            if(isGameStarted && !isDied)
            {
                score += (Time.deltaTime * modifierInfo);
                coinText.text = coin.ToString();                
                if (lastScore != (int)score)
                {
                    lastScore = (int)score;
                    scoreText.text = Convert.ToInt32(score).ToString();
                }
            }
        }

        public void GetCoin()
        {
            coin++;
        }

        public void UpdateModifier(float modifierAmount)
        {
            modifierInfo = 1f + modifierAmount;
            scoreModifierText.text = Convert.ToInt32(modifierInfo).ToString() + "X";
        }

        public void OnDeath()
        {
            FindObjectOfType<CameraController>().enabled = false;
            GameManager.Instance.isDied = true;
            StartCoroutine(ShowGameOverScreen());
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private IEnumerator ShowGameOverScreen()
        {
            yield return new WaitForSecondsRealtime(0.75f);
            gameOverScreen.SetActive(true);
        }

    }
}
