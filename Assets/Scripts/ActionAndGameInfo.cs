using System;
using UnityEngine;

namespace YipliFMDriverCommunication
{
    public static class ActionAndGameInfoManager
    {
        public static YipliUtils.PlayerActions GetActionEnumFromActionID(string actionID)
        {
            switch (actionID)
            {
                case "9GO5":
                    return YipliUtils.PlayerActions.LEFT;
                case "3KWN":
                    return YipliUtils.PlayerActions.RIGHT;
                case "PLW3":
                    return YipliUtils.PlayerActions.ENTER;
                case "UDH0":
                    return YipliUtils.PlayerActions.PAUSE;
                case "SWLO":
                    return YipliUtils.PlayerActions.RUNNING;
                case "7RCE":
                    return YipliUtils.PlayerActions.RUNNINGSTOPPED;
                case "9D6O":
                    return YipliUtils.PlayerActions.JUMP;
                case "DMEY":
                    return YipliUtils.PlayerActions.RIGHTMOVE;
                case "38UF":
                    return YipliUtils.PlayerActions.LEFTMOVE;
                case "EUOA":
                    return YipliUtils.PlayerActions.JUMPIN;
                case "QRTY":
                    return YipliUtils.PlayerActions.JUMPOUT;
                case "TIL3":
                    return YipliUtils.PlayerActions.TILES;
                case "3DIN":
                    return YipliUtils.PlayerActions.R_LEG_HOPPING;
                case "3DI1":
                    return YipliUtils.PlayerActions.L_LEG_HOPPING;
                case "99XR":
                    return YipliUtils.PlayerActions.JUMPING_JACK;
                case "NWCH":
                    return YipliUtils.PlayerActions.SKIER_JACK;
                case "VUFO":
                    return YipliUtils.PlayerActions.CROSSOVER_JUMPING_JACK;
                case "386I":
                    return YipliUtils.PlayerActions.LUNGES_RUN;
                case "BGM4":
                    return YipliUtils.PlayerActions.MOUNTAIN_CLIMBING;
                case "58GH":
                    return YipliUtils.PlayerActions.PLANK_STARTED;
                case "0DLA":
                    return YipliUtils.PlayerActions.PLANK_STOPPED;
                case "WBUT":
                    return YipliUtils.PlayerActions.MULE_KICK;
                case "FN1S":
                    return YipliUtils.PlayerActions.BURPEE;
                case "V56G":
                    return YipliUtils.PlayerActions.JUMPS_180;
                case "6JJR":
                    return YipliUtils.PlayerActions.DIAGONAL_JUMP;
                case "UJ3J":
                    return YipliUtils.PlayerActions.FORWARD_JUMP;
                case "U10J":
                    return YipliUtils.PlayerActions.BACKWARD_JUMP;
                case "B8X7":
                    return YipliUtils.PlayerActions.RIGHT_JUMP;
                case "18X7":
                    return YipliUtils.PlayerActions.LEFT_JUMP;
                case "LPM0":
                    return YipliUtils.PlayerActions.STAR_JUMP;
                case "JASL":
                    return YipliUtils.PlayerActions.CHEST_JUMP;
                case "U8W2":
                    return YipliUtils.PlayerActions.HOPSCOTCH;
                case "UWC6":
                    return YipliUtils.PlayerActions.BALANCE_STARTED;
                case "1WC1":
                    return YipliUtils.PlayerActions.BALANCE_STOPPED;
                case "ISJD":
                    return YipliUtils.PlayerActions.ARM_STARTED_1;
                case "EJ02":
                    return YipliUtils.PlayerActions.ARM_STOPPED_1;
                case "90DM":
                    return YipliUtils.PlayerActions.NINJA_KICK;
                case "HXCQ":
                    return YipliUtils.PlayerActions.HIGH_KNEE;
                case "FYN1":
                    return YipliUtils.PlayerActions.SQUATS_180;
                case "6CTM":
                    return YipliUtils.PlayerActions.SQUAT_AND_JUMP;
                case "E0CB":
                    return YipliUtils.PlayerActions.SQUAT_AND_KICK;
                case "OYMP":
                    return YipliUtils.PlayerActions.SQUATS;
                case "O12U":
                    return YipliUtils.PlayerActions.SQUAT_AND_JUMPING_JACK;
                case "X5IW":
                    return YipliUtils.PlayerActions.LATERAL_SQUATS;
                case "WBTW":
                    return YipliUtils.PlayerActions.PLANK_JUMP_INS;
                case "8G3J":
                    return YipliUtils.PlayerActions.LEG_DOG_3;
                case "UWHX":
                    return YipliUtils.PlayerActions.BANARSANA;
                case "3JCQ":
                    return YipliUtils.PlayerActions.ARDHA_CHANDRASANA;
                case "3J11":
                    return YipliUtils.PlayerActions.MALASANA;
                case "9015":
                    return YipliUtils.PlayerActions.LEFT_TAP;
                case "3L1N":
                    return YipliUtils.PlayerActions.RIGHT_TAP;
                case "A075":
                    return YipliUtils.PlayerActions.LEFT_TOUCH;
                case "AL0N":
                    return YipliUtils.PlayerActions.RIGHT_TOUCH;
                case "TRBL":
                    return YipliUtils.PlayerActions.TROUBLESHOOTING;
            }
            Debug.Log("Invalid action. Returning null Action ID.");
            return YipliUtils.PlayerActions.INVALID_ACTION;
        }

        public static string getActionIDFromActionName(YipliUtils.PlayerActions actionName)
        {
            switch (actionName)
            {
                case YipliUtils.PlayerActions.LEFT:
                    return "9GO5";

                case YipliUtils.PlayerActions.RIGHT:
                    return "3KWN";

                case YipliUtils.PlayerActions.ENTER:
                    return "PLW3";

                case YipliUtils.PlayerActions.PAUSE:
                    return "UDH0";

                case YipliUtils.PlayerActions.RUNNING:
                    return "SWLO";

                case YipliUtils.PlayerActions.RUNNINGSTOPPED:
                    return "7RCE";

                case YipliUtils.PlayerActions.JUMP:
                    return "9D6O";

                case YipliUtils.PlayerActions.RIGHTMOVE:
                    return "DMEY";

                case YipliUtils.PlayerActions.LEFTMOVE:
                    return "38UF";

                case YipliUtils.PlayerActions.JUMPIN:
                    return "EUOA";

                case YipliUtils.PlayerActions.JUMPOUT:
                    return "QRTY";

                case YipliUtils.PlayerActions.TILES:
                    return "TIL3";

                case YipliUtils.PlayerActions.R_LEG_HOPPING:
                    return "3DIN";

                case YipliUtils.PlayerActions.L_LEG_HOPPING:
                    return "3DI1";

                case YipliUtils.PlayerActions.JUMPING_JACK:
                    return "99XR";

                case YipliUtils.PlayerActions.SKIER_JACK:
                    return "NWCH";

                case YipliUtils.PlayerActions.CROSSOVER_JUMPING_JACK:
                    return "VUFO";

                case YipliUtils.PlayerActions.LUNGES_RUN:
                    return "386I";

                case YipliUtils.PlayerActions.MOUNTAIN_CLIMBING:
                    return "BGM4";

                case YipliUtils.PlayerActions.PLANK_STARTED:
                    return "58GH";

                case YipliUtils.PlayerActions.PLANK_STOPPED:
                    return "0DLA";

                case YipliUtils.PlayerActions.MULE_KICK:
                    return "WBUT";

                case YipliUtils.PlayerActions.BURPEE:
                    return "FN1S";

                case YipliUtils.PlayerActions.JUMPS_180:
                    return "V56G";

                case YipliUtils.PlayerActions.DIAGONAL_JUMP:
                    return "6JJR";

                case YipliUtils.PlayerActions.FORWARD_JUMP:
                    return "UJ3J";

                case YipliUtils.PlayerActions.BACKWARD_JUMP:
                    return "U10J";

                case YipliUtils.PlayerActions.RIGHT_JUMP:
                    return "B8X7";

                case YipliUtils.PlayerActions.LEFT_JUMP:
                    return "18X7";

                case YipliUtils.PlayerActions.STAR_JUMP:
                    return "LPM0";

                case YipliUtils.PlayerActions.CHEST_JUMP:
                    return "JASL";

                case YipliUtils.PlayerActions.HOPSCOTCH:
                    return "U8W2";

                case YipliUtils.PlayerActions.BALANCE_STARTED:
                    return "UWC6";

                case YipliUtils.PlayerActions.BALANCE_STOPPED:
                    return "1WC1";

                case YipliUtils.PlayerActions.ARM_STARTED_1:
                    return "ISJD";

                case YipliUtils.PlayerActions.ARM_STOPPED_1:
                    return "EJ02";

                case YipliUtils.PlayerActions.NINJA_KICK:
                    return "90DM";

                case YipliUtils.PlayerActions.HIGH_KNEE:
                    return "HXCQ";

                case YipliUtils.PlayerActions.SQUATS_180:
                    return "FYN1";

                case YipliUtils.PlayerActions.SQUAT_AND_JUMP:
                    return "6CTM";

                case YipliUtils.PlayerActions.SQUAT_AND_KICK:
                    return "E0CB";

                case YipliUtils.PlayerActions.SQUATS:
                    return "OYMP";

                case YipliUtils.PlayerActions.SQUAT_AND_JUMPING_JACK:
                    return "O12U";

                case YipliUtils.PlayerActions.LATERAL_SQUATS:
                    return "X5IW";

                case YipliUtils.PlayerActions.PLANK_JUMP_INS:
                    return "WBTW";

                case YipliUtils.PlayerActions.LEG_DOG_3:
                    return "8G3J";

                case YipliUtils.PlayerActions.BANARSANA:
                    return "UWHX";

                case YipliUtils.PlayerActions.ARDHA_CHANDRASANA:
                    return "3JCQ";

                case YipliUtils.PlayerActions.MALASANA:
                    return "3J11";

                case YipliUtils.PlayerActions.LEFT_TAP:
                    return "9015";

                case YipliUtils.PlayerActions.RIGHT_TAP:
                    return "3L1N";

                case YipliUtils.PlayerActions.LEFT_TOUCH:
                    return "A075";

                case YipliUtils.PlayerActions.RIGHT_TOUCH:
                    return "AL0N";

                case YipliUtils.PlayerActions.TROUBLESHOOTING:
                    return "TRBL";
            }

            Debug.Log("Invalid action. Returning null Action ID.");
            return null;
        }

        //Pass here name of the game
        public static void SetYipliGameInfo(string strGameName)
        {
            switch (strGameName.ToLower())
            {
                case "unleash":
                    YipliHelper.SetGameClusterId(2);
                    PlayerSession.Instance.intensityLevel = "medium";
                    break;

                case "trapped":
                    YipliHelper.SetGameClusterId(1);
                    PlayerSession.Instance.intensityLevel = "medium";
                    break;

                case "joyfuljumps":
                    YipliHelper.SetGameClusterId(1);
                    PlayerSession.Instance.intensityLevel = "medium";
                    break;

                case "eggcatcher":
                    YipliHelper.SetGameClusterId(6);
                    PlayerSession.Instance.intensityLevel = "low";
                    break;

                case "metrorush":
                    YipliHelper.SetGameClusterId(6);
                    PlayerSession.Instance.intensityLevel = "medium";
                    break;

                case "dancingball":
                    YipliHelper.SetGameClusterId(6);
                    PlayerSession.Instance.intensityLevel = "medium";
                    break;

                case "rollingball":
                    YipliHelper.SetGameClusterId(6);
                    PlayerSession.Instance.intensityLevel = "medium";
                    break;

                case "skater":
                    YipliHelper.SetGameClusterId(3);
                    PlayerSession.Instance.intensityLevel = "medium";
                    break;

                case "matbeats":
                    YipliHelper.SetGameClusterId(5);
                    PlayerSession.Instance.intensityLevel = "low";
                    break;

                default:
                    YipliHelper.SetGameClusterId(0);
                    PlayerSession.Instance.intensityLevel = "";
                    break;
            }
        }

        public static void SetYipliMultiplayerGameInfo(string strGameName)
        {
            switch (strGameName.ToLower())
            {

                case "penguinpop":
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.minigameId = strGameName;
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.minigameId = strGameName;
                    YipliHelper.SetGameClusterId(4);
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.intensityLevel = "medium";
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.intensityLevel = "medium";
                    break;

                case "treewarrior":
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.minigameId = strGameName;
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.minigameId = strGameName;
                    YipliHelper.SetGameClusterId(2);
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.intensityLevel = "medium";
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.intensityLevel = "medium";
                    break;

                case "tugofwar":
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.minigameId = strGameName;
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.minigameId = strGameName;
                    YipliHelper.SetGameClusterId(4);
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.intensityLevel = "medium";
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.intensityLevel = "medium";
                    break;

                case "icehopper":
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.minigameId = strGameName;
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.minigameId = strGameName;
                    YipliHelper.SetGameClusterId(211);
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.intensityLevel = "medium";
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.intensityLevel = "medium";
                    break;

                case "fruitblast":
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.minigameId = strGameName;
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.minigameId = strGameName;
                    YipliHelper.SetGameClusterId(211);
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.intensityLevel = "medium";
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.intensityLevel = "medium";
                    break;

                case "boomerang":
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.minigameId = strGameName;
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.minigameId = strGameName;
                    YipliHelper.SetGameClusterId(211);
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.intensityLevel = "medium";
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.intensityLevel = "medium";
                    break;

                case "eggcatcher":
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.minigameId = strGameName;
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.minigameId = strGameName;
                    YipliHelper.SetGameClusterId(7);
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.intensityLevel = "low";
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.intensityLevel = "low";
                    break;

                case "pingpong":
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.minigameId = strGameName;
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.minigameId = strGameName;
                    YipliHelper.SetGameClusterId(7);
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.intensityLevel = "low";
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.intensityLevel = "low";
                    break;

                case "fishtrap":
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.minigameId = strGameName;
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.minigameId = strGameName;
                    YipliHelper.SetGameClusterId(7);
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.intensityLevel = "low";
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.intensityLevel = "low";
                    break;

                case "spacetower":
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.minigameId = strGameName;
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.minigameId = strGameName;
                    YipliHelper.SetGameClusterId(7);
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.intensityLevel = "medium";
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.intensityLevel = "medium";
                    break;

                case "beachball":
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.minigameId = strGameName;
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.minigameId = strGameName;
                    YipliHelper.SetGameClusterId(6);
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.intensityLevel = "medium";
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.intensityLevel = "medium";
                    break;

                case "dragonbreath":
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.minigameId = strGameName;
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.minigameId = strGameName;
                    YipliHelper.SetGameClusterId(6);
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.intensityLevel = "medium";
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.intensityLevel = "medium";
                    break;

                default:
                    PlayerSession.Instance.intensityLevel = "";
                    break;
            }
        }
    }

    [Serializable]
    public class FmDriverResponseInfo
    {
        public int count;
        public double timestamp;
        public PlayerData[] playerdata;
    }

    [Serializable]
    public class PlayerData
    {
        public int id;
        public FMResponse fmresponse;
    }

    [Serializable]
    public class FMResponse
    {
        public string action_id;          // Action ID-Unique ID for each action. Refer below table for all action IDs
        public string action_name;         //Action Name for debugging (Gamers should strictly check action ID)
        public string properties;          //Any properties action has - ex. Running could have Step Count, Speed
    }

    #region Multiplayer Classes

    [Serializable]
    public class FmDriverResponseInfoMP
    {
        public int count;
        public double timestamp;
        public MultiPlayerData[] playerdata;
    }
    [Serializable]
    public class MultiPlayerData
    {
        public int id;
        public int count;
        public FMResponse fmresponse;
    }

    #endregion
}


