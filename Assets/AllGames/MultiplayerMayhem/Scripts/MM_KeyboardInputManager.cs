using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM_KeyboardInputManager : MonoBehaviour
{
    #region Dont destroy on Load Singleton declaration

    public static MM_KeyboardInputManager instance;

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

    #region Unity directives

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        PlayerOneKeyboardToMatInput();
        PlayerTwoKeyboardToMatInput();
#endif
    }

    #endregion

    #region Keyboard Input functions

    // Function to check Keyboard Input corresponding to Player One
    private void PlayerOneKeyboardToMatInput()
    {
        // A -> P1 Left Tap
        if (Input.GetKeyDown(KeyCode.A))
        {
            switch (MM_GameUIManager.instance.activeGame)
            {
                case MM_GameUIManager.Games.treewarrior:
                    TW_InputController.instance.P1Left();
                    break;
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
                    HBv2_InputController.instance.PlayerOneLeft();
                    break;
            }
            PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.LEFT_TAP, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
        }
        // A -> P1 Right Tap
        if (Input.GetKeyDown(KeyCode.D))
        {
            switch (MM_GameUIManager.instance.activeGame)
            {
                case MM_GameUIManager.Games.treewarrior:
                    TW_InputController.instance.P1Right();
                    break;
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
                case MM_GameUIManager.Games.headball:
                    HBv2_InputController.instance.PlayerOneRight();
                    break;
            }
            PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.RIGHT_TAP, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
        }
        // W -> P1 Right Leg Hop
        if (Input.GetKeyDown(KeyCode.W))
        {
            switch (MM_GameUIManager.instance.activeGame)
            {
                case MM_GameUIManager.Games.monsterriver:
                    MR_InputManager.instance.ShootBackRight();
                    break;
                case MM_GameUIManager.Games.theraft:
                    TR_InputManager.instance.RightShoot();
                    break;
                case MM_GameUIManager.Games.headball:
                    HBv2_InputController.instance.PlayerOneJump();
                    break;
            }
            PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.R_LEG_HOPPING, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
        }
        // S -> P1 Left Leg Hop
        if (Input.GetKeyDown(KeyCode.S))
        {
            switch (MM_GameUIManager.instance.activeGame)
            {
                case MM_GameUIManager.Games.monsterriver:
                    MR_InputManager.instance.ShootBackLeft();
                    break;
                case MM_GameUIManager.Games.theraft:
                    TR_InputManager.instance.LeftShoot();
                    break;
            }
            PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.L_LEG_HOPPING, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
        }
        // Space -> P1 Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
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
                case MM_GameUIManager.Games.dragonbreath:
                    DB_InputController.instance.PlayerOneJump();
                    break;
            }
            PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.JUMP, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
        }
        // LeftControl -> P1 Running
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            switch (MM_GameUIManager.instance.activeGame)
            {
                case MM_GameUIManager.Games.tugofwar:
                    ToW_InputController.instance.PullLeftRun();
                    break;
            }
            PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.RUNNING, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerOneDetails);
        }
    }

    // Function to check Keyboard Input corresponding to Player Two
    private void PlayerTwoKeyboardToMatInput()
    {
        // LeftArrow -> P2 Left Tap
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            switch (MM_GameUIManager.instance.activeGame)
            {
                case MM_GameUIManager.Games.treewarrior:
                    TW_InputController.instance.P2Left();
                    break;
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
                case MM_GameUIManager.Games.headball:
                    HBv2_InputController.instance.PlayerTwoLeft();
                    break;
            }
            PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.LEFT_TAP, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
        }
        // RightArrow -> P2 Right Tap
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            switch (MM_GameUIManager.instance.activeGame)
            {
                case MM_GameUIManager.Games.treewarrior:
                    TW_InputController.instance.P2Right();
                    break;
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
                case MM_GameUIManager.Games.headball:
                    HBv2_InputController.instance.PlayerTwoRight();
                    break;
            }
            PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.RIGHT_TAP, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
        }
        // UpArrow -> P2 Right Leg Hop
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            switch (MM_GameUIManager.instance.activeGame)
            {
                case MM_GameUIManager.Games.monsterriver:
                    MR_InputManager.instance.ShootFrontRight();
                    break;
                case MM_GameUIManager.Games.headball:
                    HBv2_InputController.instance.PlayerTwoJump();
                    break;
            }
            PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.R_LEG_HOPPING, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
        }
        // DownArrow -> P2 Left Leg Hop
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            switch (MM_GameUIManager.instance.activeGame)
            {
                case MM_GameUIManager.Games.monsterriver:
                    MR_InputManager.instance.ShootFrontLeft();
                    break;
            }
            PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.L_LEG_HOPPING, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
        }
        // Return -> P2 Jump
        if (Input.GetKeyDown(KeyCode.Return))
        {
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
                case MM_GameUIManager.Games.dragonbreath:
                    DB_InputController.instance.PlayerTwoJump();
                    break;
            }
            PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.JUMP, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
        }
        // RightControl -> P2 Running
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            switch (MM_GameUIManager.instance.activeGame)
            {
                case MM_GameUIManager.Games.tugofwar:
                    ToW_InputController.instance.PullRightRun();
                    break;
            }
            PlayerSession.Instance.AddMultiPlayerAction(YipliUtils.PlayerActions.RUNNING, PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerTwoDetails);
        }
    }

    #endregion

}
