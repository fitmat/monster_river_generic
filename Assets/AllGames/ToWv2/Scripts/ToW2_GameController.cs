using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToW2_GameController : MonoBehaviour
{
    public static ToW2_GameController instance;

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

    public enum GameStates { notStarted, playing, paused, gameOver }
    public GameStates gameState;

    [SerializeField] ToW2_PlayerController rightPlayer, leftPlayer;
    [SerializeField] Transform ropeTransform;
    [SerializeField] GameObject instructionText, timeText, leftCloseUp, rightCloseUp, gamePanel, blurEffect;
    [SerializeField] Animator umpireAnimator, ropeAnimator;
    [SerializeField] Animator greenFlagLeft, greenFlagRight, yellowFlagLeft, yellowFlagRight, redFlagLeft, redFlagRight;
    [SerializeField] GameObject p1Power,p2Power,p1Sweat,p2Sweat;
    [SerializeField] GameObject faceRed,faceBlue;
    [SerializeField] Texture normalTexture, happyTexture, winTexture, sadTexture;

    [Range(-4, 4)]
    public int section;
    [Range(-1, 1)]
    public int advantage;
    private int previousAdvantage;
    private int previousSection;
    public int gameTime;
    private int timeLeft;
    public float ropePosition;
    public float pullStrengthChange;

    public int currentState;

    public bool isNearlyOver;
    [SerializeField] GameObject flashingTimeText, timerBG, gameArea, cinematicObject, countdown;

    private void Start()
    {
        PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.minigameId = "tugofwar";
        Time.timeScale = 1.5f;
        gameState = GameStates.notStarted;

        isNearlyOver = false;
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        CheckRopePosition();
        CheckSection();

        PlayerSession.Instance.UpdateDuration();
        
    }
    public IEnumerator StartGame()
    {
        yield return new WaitForEndOfFrame();
        ToW_AudioManager.instance.PlayAudio("Soundtrack");
        ToW_AudioManager.instance.PlayAudio("Ambient");
        ToW_AudioManager.instance.SetTrackVolume("Soundtrack", 0.5f);
        StartCoroutine(MM_GameUIManager.instance.ShowStartScreen());
        yield return new WaitForSeconds(12.5f);
        Time.timeScale = 1;
        cinematicObject.SetActive(false);
        gameArea.SetActive(true);
        countdown.SetActive(true);
        //ToW_AudioManager.instance.PlayAudio("GameStart");
        yield return new WaitForSeconds(4.5f);

        gameState = GameStates.playing;
        PlayerSession.Instance.StartMPSession();
        MM_GameUIManager.instance.activeGame = MM_GameUIManager.Games.tugofwar;
        
        YipliHelper.SetGameClusterId(202, 202);
        
        timeLeft = gameTime;
        instructionText.SetActive(true);
        timeText.SetActive(true);
        instructionText.GetComponent<Animator>().SetTrigger("Change");
        timeText.GetComponent<Animator>().SetTrigger("Flash");
        greenFlagLeft.SetTrigger("ScaleUp");
        greenFlagRight.SetTrigger("ScaleUp");
        StartCoroutine(GameTimer());
        StartCoroutine(ToW2_InputController.instance.PlayerOneIdling());
        StartCoroutine(ToW2_InputController.instance.PlayerTwoIdling());

        ToW_AudioManager.instance.PlayAudio("StartWhistle");
        StartPulling();

        currentState = 1;

        ToW_AudioManager.instance.SetTrackVolume("Soundtrack", 1f);
    }

    private void StartPulling()
    {
        StartCoroutine(rightPlayer.DisplayEmote(0));
        StartCoroutine(leftPlayer.DisplayEmote(0));
        ropeAnimator.SetTrigger("Start");
        umpireAnimator.SetTrigger("Start");
        leftPlayer.StartAnimation();
        rightPlayer.StartAnimation();
        ropeAnimator.transform.parent.GetComponent<Animator>().SetTrigger("Start");
    }

    public void GameOver()
    {
        if (gameState == GameStates.playing)
        {
            MM_GameUIManager.instance.FlashBlackScreen();
            gameState = GameStates.gameOver;
            ToW_AudioManager.instance.PlayAudio("End");
            ToW_AudioManager.instance.PlayAudio("Cheer");
            ToW_AudioManager.instance.StopAudio("Soundtrack");
            StartCoroutine(EndGame());
        }
    }

    public IEnumerator EndGame()
    {
        // Drop rope
        ropeAnimator.SetTrigger("End");
        ropeAnimator.transform.parent.GetComponent<Animator>().SetTrigger("End");
        // Disable all VFX
        p1Power.SetActive(false);
        p1Sweat.SetActive(false);
        p2Power.SetActive(false);
        p2Sweat.SetActive(false);
        instructionText.SetActive(false);
        timeText.SetActive(false);
        MM_AudioManager.instance.PlayAudio("ResultCheer");
        gameState = GameStates.gameOver;

        // Decide winner
        if (advantage == -1)
        {
            MM_GameUIManager.instance.winnerNumber = 1;
            Debug.Log("Left Player Wins");
            StartCoroutine(rightPlayer.DisplayEmote(-3));
            StartCoroutine(leftPlayer.DisplayEmote(3));
            leftPlayer.WinAnimation();
            rightPlayer.LoseAnimation();
            leftCloseUp.SetActive(true);
        }
        else if (advantage == 1)
        {
            MM_GameUIManager.instance.winnerNumber = 2;
            Debug.Log("Right Player Wins");
            StartCoroutine(rightPlayer.DisplayEmote(3));
            StartCoroutine(leftPlayer.DisplayEmote(-3));
            rightPlayer.WinAnimation();
            leftPlayer.LoseAnimation();
            rightCloseUp.SetActive(true);
        }
        else
        {
            MM_GameUIManager.instance.winnerNumber = 3;
        }

        yield return new WaitForSecondsRealtime(2f);
        gamePanel.SetActive(false);
        
        PlayerSession.Instance.StoreMPSession(1, 1);
        yield return new WaitForSecondsRealtime(2f);
        ToW_AudioManager.instance.StopAllAudio();
        blurEffect.SetActive(true);
        MM_GameUIManager.instance.ShowResultsScreen();
    }


    public IEnumerator GameTimer()
    {
        yield return new WaitForSecondsRealtime(1f);
        timeLeft--;
        timeText.GetComponent<TMP_Text>().text = ((int)(timeLeft / 60)).ToString("00") + ":" + ((int)(timeLeft % 60)).ToString("00");
        flashingTimeText.GetComponent<TMP_Text>().text = ((int)(timeLeft / 60)).ToString("0") + ":" + ((int)(timeLeft % 60)).ToString("00");
        if (timeLeft % 60 == 0)
        {
            timeText.GetComponent<Animator>().SetTrigger("Flash");
        }
        if (timeLeft == 0)
        {
            MM_AudioManager.instance.StopAudio("Ticking");
            GameOver();
        }
        else
        {
            if (timeLeft == 10)
            {
                NearlyOver();
            }
            StartCoroutine(GameTimer());
        }
    }

    public void NearlyOver()
    {
        if (!isNearlyOver)
        {
            isNearlyOver = true;
            MM_AudioManager.instance.PlayAudio("Ticking");
            timerBG.SetActive(true);
            flashingTimeText.SetActive(true);
        }
    }

    // Function to change state and game action of gameplay
    private void IncrementCurrentState()
    {
        currentState++;
        ToW_AudioManager.instance.PlayAudio("RoundWhistle");
        if (currentState == 2)
        {
            // Scale flags to indicate bounds
            greenFlagLeft.SetTrigger("ScaleDown");
            greenFlagRight.SetTrigger("ScaleDown");
            yellowFlagLeft.SetTrigger("ScaleUp");
            yellowFlagRight.SetTrigger("ScaleUp");
            // Change game action
            instructionText.GetComponent<TMP_Text>().text = "High Knee";
            YipliHelper.SetGameClusterId(203, 203);
            instructionText.GetComponent<Animator>().SetTrigger("Change");
        }
        if (currentState == 3)
        {
            yellowFlagLeft.SetTrigger("ScaleDown");
            yellowFlagRight.SetTrigger("ScaleDown");
            redFlagLeft.SetTrigger("ScaleUp");
            redFlagRight.SetTrigger("ScaleUp");
            instructionText.GetComponent<TMP_Text>().text = "Jump";
            YipliHelper.SetGameClusterId(205, 205);
            instructionText.GetComponent<Animator>().SetTrigger("Change");
        }
    }

    // Function to check position of rope and determine current section
    private void CheckRopePosition()
    {
        ropePosition = ropeTransform.localPosition.x;

        if (ropePosition > 0.1)
        {
            advantage = 1;
            if (ropePosition > 1.5)
            {
                if (ropePosition > 3)
                {
                    if (ropePosition > 4.5)
                    {
                        if (ropePosition > 6)
                        {
                            section = 4;
                            return;
                        }
                        section = 3;
                        return;
                    }
                    section = 2;
                    return;
                }
                section = 1;
                return;
            }
            section = 0;
        }
        else if (ropePosition < -0.1)
        {
            advantage = -1;
            if (ropePosition < -1.5)
            {
                if (ropePosition < -3)
                {
                    if (ropePosition < -4.5)
                    {
                        if (ropePosition < -6)
                        {
                            section = -4;
                            return;
                        }
                        section = -3;
                        return;
                    }
                    section = -2;
                    return;
                }
                section = -1;
                return;
            }
            section = 0;
        }
    }

    // Function to check current section and adjust player state
    private void CheckSection()
    {
        // Switch player VFX when changing section if other player gets advantage
        if (previousAdvantage > advantage && currentState == 1)
        {
            p1Power.SetActive(true);
            p2Sweat.SetActive(true);
            p2Power.SetActive(false);
            p1Sweat.SetActive(false);
            faceRed.GetComponent<MeshRenderer>().material.mainTexture = happyTexture;
            faceBlue.GetComponent<MeshRenderer>().material.mainTexture = sadTexture;
        }
        else if (previousAdvantage < advantage && currentState == 1)
        {
            p2Power.SetActive(true);
            p1Sweat.SetActive(true);
            p1Power.SetActive(false);
            p2Sweat.SetActive(false);
            faceRed.GetComponent<MeshRenderer>().material.mainTexture = sadTexture;
            faceBlue.GetComponent<MeshRenderer>().material.mainTexture = happyTexture;
        }
        if (section > previousSection)
        {
            p2Power.SetActive(true);
            p1Sweat.SetActive(true);
            p1Power.SetActive(false);
            p2Sweat.SetActive(false);
            faceRed.GetComponent<MeshRenderer>().material.mainTexture = sadTexture;
            faceBlue.GetComponent<MeshRenderer>().material.mainTexture = happyTexture;
            Debug.Log("Right Player Advantage");
            if (section > 0)
            {
                Debug.Log("Decrease Right Player Pull Strength");
                rightPlayer.pullStrength -= pullStrengthChange;
                if (section == 3)
                {
                    GameOver();
                }
                else if (section == 2)
                {
                    StartCoroutine(rightPlayer.DisplayEmote(2));
                    StartCoroutine(leftPlayer.DisplayEmote(-2));
                    umpireAnimator.SetTrigger("Left");


                    if (currentState == 2)
                    {
                        IncrementCurrentState();
                    }
                }
                else if (section == 1)
                {
                    StartCoroutine(rightPlayer.DisplayEmote(1));
                    StartCoroutine(leftPlayer.DisplayEmote(-1));
                    
                    umpireAnimator.SetTrigger("Left");

                    if (currentState == 1)
                    {
                        IncrementCurrentState();
                    }

                }
                else if (section == 4)
                {
                    Debug.Log("Right Player Wins");
                    Time.timeScale = 0;
                }
            }
            else
            {
                Debug.Log("Increase Left Player Pull Strength");
                leftPlayer.pullStrength += pullStrengthChange;
            }
            if (section == 0)
            {
                StartCoroutine(rightPlayer.DisplayEmote(0));
                StartCoroutine(leftPlayer.DisplayEmote(0));
                umpireAnimator.SetTrigger("Centre");
            }
        }
        else if (section < previousSection)
        {
            p1Power.SetActive(true);
            p2Sweat.SetActive(true);
            p2Power.SetActive(false);
            p1Sweat.SetActive(false);
            faceRed.GetComponent<MeshRenderer>().material.mainTexture = happyTexture;
            faceBlue.GetComponent<MeshRenderer>().material.mainTexture = sadTexture;
            Debug.Log("Left Player Advantage");
            if (section < 0)
            {
                leftPlayer.pullStrength -= pullStrengthChange;
                Debug.Log("Decrease Left Player Pull Strength");
                if (section == -3)
                {
                    GameOver();
                }
                else if (section == -2)
                {
                    StartCoroutine(rightPlayer.DisplayEmote(-2));
                    StartCoroutine(leftPlayer.DisplayEmote(2));

                    umpireAnimator.SetTrigger("Right");

                    if (currentState == 2)
                    {
                        IncrementCurrentState();
                    }
                }
                else if (section == -1)
                {
                    StartCoroutine(rightPlayer.DisplayEmote(-1));
                    StartCoroutine(leftPlayer.DisplayEmote(1));

                    umpireAnimator.SetTrigger("Right");

                    if (currentState == 1)
                    {
                        IncrementCurrentState();
                    }
                }
                else if (section == -4)
                {
                    Debug.Log("Left Player Wins");
                    Time.timeScale = 0;
                }
            }
            else
            {
                rightPlayer.pullStrength += pullStrengthChange;
                StartCoroutine(rightPlayer.DisplayEmote(0));
                StartCoroutine(leftPlayer.DisplayEmote(0));
                Debug.Log("Increase Right Player Pull Strength");
            }
            if (section == 0)
            {
                StartCoroutine(rightPlayer.DisplayEmote(0));
                StartCoroutine(leftPlayer.DisplayEmote(0));
                umpireAnimator.SetTrigger("Centre");
            }
        }
        previousSection = section;
    }
}
