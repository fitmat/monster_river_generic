using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YipliFMDriverCommunication;

namespace Yipli.MatSystems
{
    public class MatController : MonoBehaviour
    {
        // required variables
        const string LEFT = "left";
        const string RIGHT = "right";
        const string ENTER = "enter";

        private List<Button> currentMenuButtons;
        Button currentB;

        int currentButtonIndex;

        YipliUtils.PlayerActions detectedAction;

        public YipliUtils.PlayerActions DetectedAction { get => detectedAction; set => detectedAction = value; }
        public List<Button> CurrentMenuButtons { get => currentMenuButtons; set => currentMenuButtons = value; }
        public int CurrentButtonIndex { get => currentButtonIndex; set => currentButtonIndex = value; }
        public Button CurrentB { get => currentB; set => currentB = value; }

        private void Update()
        {
            ManageMatActions();
        }

        private void ManageMatActions()
        {
            string fmActionData = InitBLE.PluginClass.CallStatic<string>("_getFMResponse");
            Debug.Log("Json Data from Fmdriver : " + fmActionData);

            /* New FmDriver Response Format
               {
                  "count": 1,                          # Updates every time new action is detected
                  "timestamp": 1597237057689,          # Time at which response was packaged/created by Driver
                  "playerdata": [                      # Array containing player data
                    {
                      "id": 1,                         # Player ID (For Single-player-1 , Multiplayer it could be 1 or 2 )
                      "fmresponse": {
                        "action_id": "9D6O",           # Action ID-Unique ID for each action. Refer below table for all action IDs
                        "action_name": "Jump",         # Action Name for debugging (Gamers should strictly check action ID)
                        "properties": "null"           # Any properties action has - ex. Running could have Step Count, Speed
                      }
                    },
                    {null}
                  ]
                }
            */

            FmDriverResponseInfo singlePlayerResponse = JsonUtility.FromJson<FmDriverResponseInfo>(fmActionData);

            if (singlePlayerResponse == null) return;

            if (PlayerSession.Instance.currentYipliConfig.oldFMResponseCount < singlePlayerResponse.count)
            {
                PlayerSession.Instance.currentYipliConfig.oldFMResponseCount = singlePlayerResponse.count;

                YipliUtils.PlayerActions providedAction = ActionAndGameInfoManager.GetActionEnumFromActionID(singlePlayerResponse.playerdata[0].fmresponse.action_id);
                DetectedAction = providedAction;

                switch (providedAction)
                {
                    case YipliUtils.PlayerActions.PAUSE:
                        break;

                    case YipliUtils.PlayerActions.RUNNINGSTOPPED:
                        break;

                    case YipliUtils.PlayerActions.STOP:
                        break;

                    // other actions
                    case YipliUtils.PlayerActions.LEFTMOVE:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LEFTMOVE);
                        break;

                    case YipliUtils.PlayerActions.RIGHTMOVE:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.RIGHTMOVE);
                        break;

                    case YipliUtils.PlayerActions.JUMP:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.JUMP);
                        break;

                    case YipliUtils.PlayerActions.RUNNING:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.RUNNING);
                        break;

                    case YipliUtils.PlayerActions.JUMPING_JACK:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.JUMPING_JACK);
                        break;

                    case YipliUtils.PlayerActions.SKIER_JACK:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SKIER_JACK);
                        break;

                    case YipliUtils.PlayerActions.CROSSOVER_JUMPING_JACK:
                        break;

                    case YipliUtils.PlayerActions.LUNGES_RUN:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LUNGES_RUN);
                        break;

                    case YipliUtils.PlayerActions.MOUNTAIN_CLIMBING:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.MOUNTAIN_CLIMBING);
                        break;

                    case YipliUtils.PlayerActions.PLANK_STARTED:
                        break;

                    case YipliUtils.PlayerActions.PLANK_STOPPED:
                        break;

                    case YipliUtils.PlayerActions.MULE_KICK:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.MULE_KICK);
                        break;

                    case YipliUtils.PlayerActions.R_LEG_HOPPING:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.R_LEG_HOPPING);
                        break;

                    case YipliUtils.PlayerActions.L_LEG_HOPPING:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.L_LEG_HOPPING);
                        break;

                    case YipliUtils.PlayerActions.BURPEE:
                        break;

                    case YipliUtils.PlayerActions.JUMPS_180:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.JUMPS_180);
                        break;

                    case YipliUtils.PlayerActions.DIAGONAL_JUMP:
                        break;

                    case YipliUtils.PlayerActions.FORWARD_JUMP:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.FORWARD_JUMP);
                        break;

                    case YipliUtils.PlayerActions.BACKWARD_JUMP:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.BACKWARD_JUMP);
                        break;

                    case YipliUtils.PlayerActions.RIGHT_JUMP:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.RIGHT_JUMP);
                        break;

                    case YipliUtils.PlayerActions.LEFT_JUMP:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LEFT_JUMP);
                        break;

                    case YipliUtils.PlayerActions.STAR_JUMP:
                        break;

                    case YipliUtils.PlayerActions.CHEST_JUMP:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.CHEST_JUMP);
                        break;

                    case YipliUtils.PlayerActions.HOPSCOTCH:
                        break;

                    case YipliUtils.PlayerActions.BALANCE_STARTED:
                        break;

                    case YipliUtils.PlayerActions.BALANCE_STOPPED:
                        break;

                    case YipliUtils.PlayerActions.ARM_STARTED_1:
                        break;

                    case YipliUtils.PlayerActions.ARM_STOPPED_1:
                        break;

                    case YipliUtils.PlayerActions.NINJA_KICK:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.NINJA_KICK);
                        break;

                    case YipliUtils.PlayerActions.HIGH_KNEE:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.HIGH_KNEE);
                        break;

                    case YipliUtils.PlayerActions.SQUATS_180:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SQUATS_180);
                        break;

                    case YipliUtils.PlayerActions.SQUAT_AND_JUMP:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SQUAT_AND_JUMP);
                        break;

                    case YipliUtils.PlayerActions.SQUAT_AND_KICK:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SQUAT_AND_KICK);
                        break;

                    case YipliUtils.PlayerActions.SQUATS:
                        break;

                    case YipliUtils.PlayerActions.SQUAT_AND_JUMPING_JACK:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.SQUAT_AND_JUMPING_JACK);
                        break;

                    case YipliUtils.PlayerActions.LATERAL_SQUATS:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LATERAL_SQUATS);
                        break;

                    case YipliUtils.PlayerActions.PLANK_JUMP_INS:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.PLANK_JUMP_INS);
                        break;

                    case YipliUtils.PlayerActions.LEG_DOG_3:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.LEG_DOG_3);
                        break;

                    case YipliUtils.PlayerActions.BANARSANA:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.BANARSANA);
                        break;

                    case YipliUtils.PlayerActions.AEROPLANE_POSE:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.AEROPLANE_POSE);
                        break;

                    case YipliUtils.PlayerActions.VIKRASANA:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.VIKRASANA);
                        break;

                    case YipliUtils.PlayerActions.ARDHA_CHANDRASANA:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.ARDHA_CHANDRASANA);
                        break;

                    case YipliUtils.PlayerActions.MALASANA:
                        PlayerSession.Instance.AddPlayerAction(YipliUtils.PlayerActions.MALASANA);
                        break;

                    // UI input executions
                    case YipliUtils.PlayerActions.LEFT:
                        ProcessMatInputs(LEFT);
                        break;

                    case YipliUtils.PlayerActions.RIGHT:
                        ProcessMatInputs(RIGHT);
                        break;

                    case YipliUtils.PlayerActions.ENTER:
                        ProcessMatInputs(ENTER);
                        break;

                    default:
                        Debug.LogError("Wrong Action is detected : " + providedAction.ToString());
                        break;
                }
            }
        }

        private void ProcessMatInputs(string matInput)
        {
            switch (matInput)
            {
                case LEFT:
                    CurrentButtonIndex = GetPreviousButton();
                    ManageCurrentButton();
                    break;

                case RIGHT:
                    CurrentButtonIndex = GetNextButton();
                    ManageCurrentButton();
                    break;

                case ENTER:
                    CurrentB.onClick.Invoke();
                    break;

                default:
                    Debug.Log("Wrong Input");
                    break;
            }
        }

        private int GetNextButton()
        {
            if ((CurrentButtonIndex + 1) == CurrentMenuButtons.Count)
            {
                return 0;
            }
            else
            {
                return CurrentButtonIndex + 1;
            }
        }

        private int GetPreviousButton()
        {
            if (CurrentButtonIndex == 0)
            {
                return CurrentMenuButtons.Count - 1;
            }
            else
            {
                return CurrentButtonIndex - 1;
            }
        }

        private void ManageCurrentButton()
        {
            for (int i = 0; i < CurrentMenuButtons.Count; i++)
            {
                if (i == CurrentButtonIndex)
                {
                    // animate button
                }
                else
                {
                    // do nothing
                }
            }
        }

        private void GetMatUIKeyboardInputs()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ProcessMatInputs(LEFT);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ProcessMatInputs(RIGHT);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                ProcessMatInputs(ENTER);
            }
        }
    }
}
