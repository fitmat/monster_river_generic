using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YipliFMDriverCommunication;
using TMPro;

/* Multiplayer Mayhem Input Controller
 * Handles the Mat Input
 * Reads input from FMDriver
 * Detects current action
 * Calls required function on input
 */

public class MM_InputController : MonoBehaviour
{

    #region Dont destroy on Load Singleton declaration

    public static MM_InputController instance;

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
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    #endregion

    #region Variable declaration

    public bool isTutorialPlayed, isConnected;
    private int player1FMResponseCount, player2FMResponseCount;
    public int p1X1, p1X2, p1X3, p1X4, p2X1, p2X2, p2X3, p2X4;

    private YipliUtils.PlayerActions player1Action, player2Action;
    private FmDriverResponseInfoMP fmData;

    //public TMP_Text //player1ActionText, //player2ActionText;

    public event Action playerTilesEvent;

    #endregion

    #region Unity directives

    private void Start()
    {
        YipliHelper.SetGameMode(1);
        isTutorialPlayed = false;
        isConnected = false;

#if UNITY_EDITOR
        isTutorialPlayed = true;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (isConnected)
        {
            try
            {
                TwoPlayerMatInput();
            }
            catch (Exception e)
            {
                Debug.Log("Action Test- No Mat Input Found " + e.Message);
            }
        }
    }

    #endregion

    #region Mat Input functions

    // Function to set cluster id to 0 to begin detecting Mat inputs
    public void StartInputDetection()
    {
        Debug.Log("Connection Test- Starting Detection");
        YipliHelper.SetGameMode(0);
        Debug.Log("Connection Test- Game Mode Set");
#if UNITY_EDITOR
#else
        // Checks if tutorial has been played before, else plays tutorial
        if (!isTutorialPlayed)
        {
            MM_TutorialController.instance.StartTutorial();
        }
#endif
        isConnected = true;
    }

    // Function to Processes Player One Action input
    private void PlayerOneInput(int i)
    {
        switch (player1Action)
        {
            case YipliUtils.PlayerActions.ENTER:
                Debug.Log("Action Test- New Player 1 Action- ENTER");
                Debug.Log("Event Test- ENTER event detected");
                MM_MatButtonController.instance.SelectButton();
                Debug.Log("Event Test- ENTER event handled");
                Debug.Log("Action Test- New Player 1 Action Invoked");
                break;
            case YipliUtils.PlayerActions.RIGHT:
                Debug.Log("Action Test- New Player 1 Action- RIGHT");
                Debug.Log("Event Test- RIGHT event detected");
                MM_MatButtonController.instance.GetNextButton();
                Debug.Log("Event Test- RIGHT event handled");
                Debug.Log("Action Test- New Player 1 Action Invoked");
                break;
            case YipliUtils.PlayerActions.LEFT:
                Debug.Log("Action Test- New Player 1 Action- LEFT");
                Debug.Log("Event Test- LEFT event detected");
                MM_MatButtonController.instance.GetPreviousButton();
                Debug.Log("Event Test- LEFT event handled");
                Debug.Log("Action Test- New Player 1 Action Invoked");
                break;
            case YipliUtils.PlayerActions.JUMP:
                Debug.Log("Action Test- New Player 1 Action- JUMP");
                Debug.Log("Event Test- Player 1 JUMP event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.penguinpop:
                        PP_InputManager.instance.PlayerOneJump();
                        break;
                    case MM_GameUIManager.Games.treewarrior:
                        TW_InputController.instance.P1Jump();
                        break;
                    case MM_GameUIManager.Games.tugofwar:
                        ToW2_InputController.instance.PlayerOnePull();
                        break;
                    case MM_GameUIManager.Games.puddlehop:
                        PH_InputManager.instance.PlayerOneJump();
                        break;
                    case MM_GameUIManager.Games.fruitslice:
                        FS_InputController.instance.PlayerOneThrow();
                        break;
                    case MM_GameUIManager.Games.boomerang:
                        BM_InputController.instance.P1Throw();
                        break;
                    case MM_GameUIManager.Games.towerbuilder:
                        TB_InputController.instance.PlayerOneJump();
                        break;
                    case MM_GameUIManager.Games.headball:
                        HBv2_InputController.instance.PlayerOneJump();
                        break;
                    case MM_GameUIManager.Games.dragonbreath:
                        DB_InputController.instance.PlayerOneJump();
                        break;
                }

                Debug.Log("Event Test- Player 1 JUMP event handled");
                Debug.Log("Action Test- New Player 1 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.JUMP, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
                break;
            case YipliUtils.PlayerActions.RUNNING:
                Debug.Log("Action Test- New Player 1 Action- RUNNING");
                Debug.Log("Event Test- Player 1 RUNNING event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.tugofwar:
                        ToW2_InputController.instance.PlayerOnePull();
                        break;
                }

                Debug.Log("Event Test- Player 1 RUNNING event handled");
                Debug.Log("Action Test- New Player 1 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.RUNNING, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
                break;
            case YipliUtils.PlayerActions.HIGH_KNEE:
                Debug.Log("Action Test- New Player 1 Action- HIGH_KNEE");
                Debug.Log("Event Test- Player 1 HIGH_KNEE event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.tugofwar:
                        ToW2_InputController.instance.PlayerOnePull();
                        break;
                }

                Debug.Log("Event Test- Player 1 HIGH_KNEE event handled");
                Debug.Log("Action Test- New Player 1 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.HIGH_KNEE, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
                break;
            case YipliUtils.PlayerActions.NINJA_KICK:
                Debug.Log("Action Test- New Player 1 Action- NINJA_KICK");
                Debug.Log("Event Test- Player 1 NINJA_KICK event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.tugofwar:
                        ToW_InputController.instance.PullLeftRun();
                        break;
                }

                Debug.Log("Event Test- Player 1 NINJA_KICK event handled");
                Debug.Log("Action Test- New Player 1 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.NINJA_KICK, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
                break;
            case YipliUtils.PlayerActions.LEFT_TAP:
                Debug.Log("Action Test- New Player 1 Action- LEFT_TAP");
                Debug.Log("Event Test- Player 1 LEFT_TAP event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.treewarrior:
                        TW_InputController.instance.P1Left();
                        break;
                    case MM_GameUIManager.Games.headball:
                        HBv2_InputController.instance.PlayerOneLeft();
                        break;
                }

                Debug.Log("Event Test- Player 1 LEFT_TAP event handled");
                Debug.Log("Action Test- New Player 1 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.LEFT_TAP, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
                break;
            case YipliUtils.PlayerActions.RIGHT_TAP:
                Debug.Log("Action Test- New Player 1 Action- RIGHT_TAP");
                Debug.Log("Event Test- Player 1 RIGHT_TAP event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.treewarrior:
                        TW_InputController.instance.P1Right();
                        break;
                    case MM_GameUIManager.Games.headball:
                        HBv2_InputController.instance.PlayerOneRight();
                        break;
                }

                Debug.Log("Event Test- Player 1 RIGHT_TAP event handled");
                Debug.Log("Action Test- New Player 1 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.RIGHT_TAP, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
                break;
            case YipliUtils.PlayerActions.LEFT_TOUCH:
                Debug.Log("Action Test- New Player 1 Action- LEFT_TOUCH");
                Debug.Log("Event Test- Player 1 LEFT_TOUCH event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.eggcatcher:
                        EC_InputController.instance.PlayerOneLeft();
                        break;
                    case MM_GameUIManager.Games.yiplipong:
                        YP_InputController.instance.PlayerOneLeft();
                        break;
                    case MM_GameUIManager.Games.pinball:
                        PB_InputController.instance.PlayerOneLeft();
                        break;
                    case MM_GameUIManager.Games.fishtrap:
                        FTv2_InputController.instance.PlayerOneLeft();
                        break;
                    case MM_GameUIManager.Games.hungrysnake:
                        HS_InputController.instance.PlayerOneLeftTap();
                        break;
                    case MM_GameUIManager.Games.headball:
                        HBv2_InputController.instance.PlayerOneRight();
                        break;
                }

                Debug.Log("Event Test- Player 1 LEFT_TOUCH event handled");
                Debug.Log("Action Test- New Player 1 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.LEFT_TOUCH, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
                break;
            case YipliUtils.PlayerActions.RIGHT_TOUCH:
                Debug.Log("Action Test- New Player 1 Action- RIGHT_TOUCH");
                Debug.Log("Event Test- Player 1 RIGHT_TOUCH event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.eggcatcher:
                        EC_InputController.instance.PlayerOneRight();
                        break;
                    case MM_GameUIManager.Games.yiplipong:
                        YP_InputController.instance.PlayerOneRight();
                        break;
                    case MM_GameUIManager.Games.pinball:
                        PB_InputController.instance.PlayerOneRight();
                        break;
                    case MM_GameUIManager.Games.fishtrap:
                        FTv2_InputController.instance.PlayerOneRight();
                        break;
                    case MM_GameUIManager.Games.hungrysnake:
                        HS_InputController.instance.PlayerOneRightTap();
                        break;
                }

                Debug.Log("Event Test- Player 1 RIGHT_TOUCH event handled");
                Debug.Log("Action Test- New Player 1 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.RIGHT_TOUCH, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
                break;
            case YipliUtils.PlayerActions.L_LEG_HOPPING:
                Debug.Log("Action Test- New Player 1 Action- L_LEG_HOPPING");
                Debug.Log("Event Test- Player 1 L_LEG_HOPPING event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.monsterriver:
                        MR_InputManager.instance.ShootBackLeft();
                        break;
                    case MM_GameUIManager.Games.theraft:
                        TR_InputManager.instance.LeftShoot();
                        break;
                }

                Debug.Log("Event Test- Player 1 L_LEG_HOPPING event handled");
                Debug.Log("Action Test- New Player 1 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.L_LEG_HOPPING, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
                break;
            case YipliUtils.PlayerActions.R_LEG_HOPPING:
                Debug.Log("Action Test- New Player 1 Action- R_LEG_HOPPING");
                Debug.Log("Event Test- Player 1 R_LEG_HOPPING event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.monsterriver:
                        MR_InputManager.instance.ShootBackRight();
                        break;
                    case MM_GameUIManager.Games.theraft:
                        TR_InputManager.instance.RightShoot();
                        break;
                }

                Debug.Log("Event Test- Player 1 R_LEG_HOPPING event handled");
                Debug.Log("Action Test- New Player 1 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.R_LEG_HOPPING, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
                break;
            case YipliUtils.PlayerActions.TILES:
                Debug.Log("Action Test- New Player 1 Action- TILES");

                string[] tokens = fmData.playerdata[i].fmresponse.properties.Split(',');

                Debug.Log("TOKENS : " + tokens.ToString());

                for (int j = 0; j < tokens.Length; j++)
                {
                    string[] coords = tokens[j].Split(':');

                    switch (coords[0].ToLower())
                    {

                        case "x1":
                            p1X1 = int.Parse(coords[1]); //Convert.ToInt32(coords[1]);
                            break;
                        case "x2":
                            p1X2 = int.Parse(coords[1]);//Convert.ToInt32(coords[1]);
                            break;
                        case "x3":
                            p1X3 = int.Parse(coords[1]);//Convert.ToInt32(coords[1]);
                            break;
                        case "x4":
                            p1X4 = int.Parse(coords[1]);//Convert.ToInt32(coords[1]);
                            break;
                        default:
                            break;
                    }
                }

                playerTilesEvent?.Invoke();
                Debug.Log("Action Test- New Player 1 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.TILES, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
                break;
            default:
                Debug.Log("Action Test- Invalid Player 1 Action");
                break;
        }
    }

    // Function to Processes Player Two Action input
    private void PlayerTwoInput(int i)
    {
        switch (player2Action)
        {
            case YipliUtils.PlayerActions.ENTER:
                Debug.Log("Action Test- New Player 2 Action- ENTER");
                //uiEnterEvent?.Invoke();
                Debug.Log("Action Test- New Player 2 Action Invoked");
                break;
            case YipliUtils.PlayerActions.RIGHT:
                Debug.Log("Action Test- New Player 2 Action- RIGHT");
                //uiRightEvent?.Invoke();
                Debug.Log("Action Test- New Player 2 Action Invoked");
                break;
            case YipliUtils.PlayerActions.LEFT:
                Debug.Log("Action Test- New Player 2 Action- LEFT");
                //uiLeftEvent?.Invoke();
                Debug.Log("Action Test- New Player 2 Action Invoked");
                break;
            case YipliUtils.PlayerActions.JUMP:
                Debug.Log("Action Test- New Player 2 Action- JUMP");
                Debug.Log("Event Test- Player 2 JUMP event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.penguinpop:
                        PP_InputManager.instance.PlayerTwoJump();
                        break;
                    case MM_GameUIManager.Games.treewarrior:
                        TW_InputController.instance.P2Jump();
                        break;
                    case MM_GameUIManager.Games.tugofwar:
                        ToW2_InputController.instance.PlayerTwoPull();
                        break;
                    case MM_GameUIManager.Games.theraft:
                        TR_InputManager.instance.MoveRaft();
                        break;
                    case MM_GameUIManager.Games.puddlehop:
                        PH_InputManager.instance.PlayerTwoJump();
                        break;
                    case MM_GameUIManager.Games.fruitslice:
                        FS_InputController.instance.PlayerTwoThrow();
                        break;
                    case MM_GameUIManager.Games.boomerang:
                        BM_InputController.instance.P2Throw();
                        break;
                    case MM_GameUIManager.Games.towerbuilder:
                        TB_InputController.instance.PlayerTwoJump();
                        break;
                    case MM_GameUIManager.Games.headball:
                        HBv2_InputController.instance.PlayerTwoJump();
                        break;
                    case MM_GameUIManager.Games.dragonbreath:
                        DB_InputController.instance.PlayerTwoJump();
                        break;
                }

                Debug.Log("Event Test- Player 2 JUMP event handled");
                Debug.Log("Action Test- New Player 2 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.JUMP, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
                break;
            case YipliUtils.PlayerActions.RUNNING:
                Debug.Log("Action Test- New Player 2 Action- RUNNING");
                Debug.Log("Event Test- Player 2 RUNNING event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.tugofwar:
                        ToW2_InputController.instance.PlayerTwoPull();
                        break;
                }

                Debug.Log("Event Test- Player 2 RUNNING event handled");
                Debug.Log("Action Test- New Player 2 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.RUNNING, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
                break;
            case YipliUtils.PlayerActions.HIGH_KNEE:
                Debug.Log("Action Test- New Player 2 Action- HIGH_KNEE");
                Debug.Log("Event Test- Player 2 HIGH_KNEE event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.tugofwar:
                        ToW2_InputController.instance.PlayerTwoPull();
                        break;
                }

                Debug.Log("Event Test- Player 2 HIGH_KNEE event handled");
                Debug.Log("Action Test- New Player 2 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.HIGH_KNEE, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
                break;
            case YipliUtils.PlayerActions.NINJA_KICK:
                Debug.Log("Action Test- New Player 2 Action- NINJA_KICK");
                Debug.Log("Event Test- Player 2 NINJA_KICK event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.tugofwar:
                        ToW_InputController.instance.PullRightRun();
                        break;
                }

                Debug.Log("Event Test- Player 2 NINJA_KICK event handled");
                Debug.Log("Action Test- New Player 2 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.NINJA_KICK, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
                break;
            case YipliUtils.PlayerActions.LEFT_TAP:
                Debug.Log("Action Test- New Player 2 Action- LEFT_TAP");
                Debug.Log("Event Test- Player 2 LEFT_TAP event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.treewarrior:
                        TW_InputController.instance.P2Left();
                        break;
                    case MM_GameUIManager.Games.headball:
                        HBv2_InputController.instance.PlayerTwoLeft();
                        break;
                }

                Debug.Log("Event Test- Player 2 LEFT_TAP event handled");
                Debug.Log("Action Test- New Player 2 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.LEFT_TAP, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
                break;
            case YipliUtils.PlayerActions.RIGHT_TAP:
                Debug.Log("Action Test- New Player 2 Action- RIGHT_TAP");
                Debug.Log("Event Test- Player 2 RIGHT_TAP event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.treewarrior:
                        TW_InputController.instance.P2Right();
                        break;
                    case MM_GameUIManager.Games.headball:
                        HBv2_InputController.instance.PlayerTwoRight();
                        break;
                }

                Debug.Log("Event Test- Player 2 RIGHT_TAP event handled");
                Debug.Log("Action Test- New Player 2 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.RIGHT_TAP, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
                break;
            case YipliUtils.PlayerActions.LEFT_TOUCH:
                Debug.Log("Action Test- New Player 2 Action- LEFT_TOUCH");
                Debug.Log("Event Test- Player 2 LEFT_TOUCH event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.eggcatcher:
                        EC_InputController.instance.PlayerTwoLeft();
                        break;
                    case MM_GameUIManager.Games.yiplipong:
                        YP_InputController.instance.PlayerTwoLeft();
                        break;
                    case MM_GameUIManager.Games.pinball:
                        PB_InputController.instance.PlayerTwoLeft();
                        break;
                    case MM_GameUIManager.Games.fishtrap:
                        FTv2_InputController.instance.PlayerTwoLeft();
                        break;
                    case MM_GameUIManager.Games.hungrysnake:
                        HS_InputController.instance.PlayerTwoLeftTap();
                        break;
                }

                Debug.Log("Event Test- Player 2 LEFT_TOUCH event handled");
                Debug.Log("Action Test- New Player 2 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.LEFT_TOUCH, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
                break;
            case YipliUtils.PlayerActions.RIGHT_TOUCH:
                Debug.Log("Action Test- New Player 2 Action- RIGHT_TOUCH");
                Debug.Log("Event Test- Player 2 RIGHT_TOUCH event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.eggcatcher:
                        EC_InputController.instance.PlayerTwoRight();
                        break;
                    case MM_GameUIManager.Games.yiplipong:
                        YP_InputController.instance.PlayerTwoRight();
                        break;
                    case MM_GameUIManager.Games.pinball:
                        PB_InputController.instance.PlayerTwoRight();
                        break;
                    case MM_GameUIManager.Games.fishtrap:
                        FTv2_InputController.instance.PlayerTwoRight();
                        break;
                    case MM_GameUIManager.Games.hungrysnake:
                        HS_InputController.instance.PlayerTwoRightTap();
                        break;
                }

                Debug.Log("Event Test- Player 2 RIGHT_TOUCH event handled");
                Debug.Log("Action Test- New Player 2 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.RIGHT_TOUCH, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
                break;
            case YipliUtils.PlayerActions.L_LEG_HOPPING:
                Debug.Log("Action Test- New Player 2 Action- L_LEG_HOPPING");
                Debug.Log("Event Test- Player 2 L_LEG_HOPPING event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.monsterriver:
                        MR_InputManager.instance.ShootFrontLeft();
                        break;
                }

                Debug.Log("Event Test- Player 2 L_LEG_HOPPING event handled");
                Debug.Log("Action Test- New Player 2 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.L_LEG_HOPPING, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
                break;
            case YipliUtils.PlayerActions.R_LEG_HOPPING:
                Debug.Log("Action Test- New Player 2 Action- R_LEG_HOPPING");
                Debug.Log("Event Test- Player 2 R_LEG_HOPPING event detected");
                MM_GameUIManager.instance.isExiting = false;

                switch (MM_GameUIManager.instance.activeGame)
                {
                    case MM_GameUIManager.Games.monsterriver:
                        MR_InputManager.instance.ShootFrontRight();
                        break;
                }

                Debug.Log("Event Test- Player 2 R_LEG_HOPPING event handled");
                Debug.Log("Action Test- New Player 2 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.R_LEG_HOPPING, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
                break;
            case YipliUtils.PlayerActions.TILES:
                Debug.Log("Action Test- New Player 2 Action- TILES");

                string[] tokens = fmData.playerdata[i].fmresponse.properties.Split(',');

                Debug.Log("TOKENS : " + tokens.ToString());

                for (int j = 0; j < tokens.Length; j++)
                {
                    string[] coords = tokens[j].Split(':');

                    switch (coords[0].ToLower())
                    {

                        case "x1":
                            p2X1 = int.Parse(coords[1]); //Convert.ToInt32(coords[1]);
                            break;
                        case "x2":
                            p2X2 = int.Parse(coords[1]);//Convert.ToInt32(coords[1]);
                            break;
                        case "x3":
                            p2X3 = int.Parse(coords[1]);//Convert.ToInt32(coords[1]);
                            break;
                        case "x4":
                            p2X4 = int.Parse(coords[1]);//Convert.ToInt32(coords[1]);
                            break;
                        default:
                            break;
                    }
                }

                playerTilesEvent?.Invoke();
                Debug.Log("Action Test- New Player 2 Action Invoked");
                PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.TILES, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
                break;
            default:
                Debug.Log("Action Test- Invalid Player 2 Action");
                break;
        }
    }

    // Function to read input in Two player mode
    private void TwoPlayerMatInput()
    {
        Debug.Log("Testing- Mat input function start");
        string fmActionData = InitBLE.GetFMResponse();

        Debug.Log("Action Test- Cluster Id: " + YipliHelper.GetGameClusterId());
        Debug.Log("Action Test- Action data: " + fmActionData);
        Debug.Log("Action Test- Game Mode: " + InitBLE.getGameMode());

        fmData = JsonUtility.FromJson<FmDriverResponseInfoMP>(fmActionData);

        Debug.Log("Action Test- Old FM Response count: " + PlayerSession.Instance.currentYipliConfig.oldFMResponseCount);
        Debug.Log("Action Test- FM data: " + fmData);
        Debug.Log("Action Test- New FM Response count: " + fmData.count);
        Debug.Log("Action Test- Old Player 1 FM Response count: " + player1FMResponseCount);
        Debug.Log("Action Test- New Player 1 FM Response count: " + fmData.playerdata[0].count);
        Debug.Log("Action Test- Old Player 2 FM Response count: " + player2FMResponseCount);
        Debug.Log("Action Test- New Player 2 FM Response count: " + fmData.playerdata[1].count);

        player1Action = ActionAndGameInfoManager.GetActionEnumFromActionID(fmData.playerdata[0].fmresponse.action_id);
        player2Action = ActionAndGameInfoManager.GetActionEnumFromActionID(fmData.playerdata[1].fmresponse.action_id);

        Debug.Log("Action Test- Player One action- " + player1Action);
        Debug.Log("Action Test- Player Two action- " + player2Action);

        if(player1Action == YipliUtils.PlayerActions.PAUSE && player2Action == YipliUtils.PlayerActions.PAUSE)
        {
            MM_GameUIManager.instance.ShowExitingWarning();
        }

        if (PlayerSession.Instance.currentYipliConfig.oldFMResponseCount != fmData.count)
        {
            Debug.Log("Action Test- New Multiplayer Action");
            PlayerSession.Instance.currentYipliConfig.oldFMResponseCount = fmData.count;

            for (int i = 0; i < 2; i++)
            {
                switch (fmData.playerdata[i].id)
                {
                    case 1:
                        if (player1FMResponseCount != fmData.playerdata[i].count)
                        {
                            Debug.Log("Action Test- New Player 1 Action");
                            Debug.Log("Action Test- 'i' value- " + i);
                            Debug.Log("FMResponse " + fmActionData);
                            Debug.Log("Action Test- Player 1 Action reached here");
                            player1FMResponseCount = fmData.playerdata[i].count;

                            PlayerOneInput(i);
                        }
                        else
                        {
                            Debug.Log("Old Player One action");
                        }
                        break;

                    case 2:
                        if (player2FMResponseCount != fmData.playerdata[i].count)
                        {
                            Debug.Log("Action Test- New Player 2 Action");
                            Debug.Log("Action Test- 'i' value- " + i);
                            Debug.Log("FMResponse " + fmActionData);
                            player2FMResponseCount = fmData.playerdata[i].count;
                            Debug.Log("Action Test- Player 2 Action reached here");

                            PlayerTwoInput(i);
                        }
                        else
                        {
                            Debug.Log("Old Player Two action");
                        }
                        break;
                }
            }

        }
        else
        {
            Debug.Log("Old multiplayer action");
        }

    }

#endregion

}

