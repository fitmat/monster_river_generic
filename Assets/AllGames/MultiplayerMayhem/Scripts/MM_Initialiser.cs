using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* Multiplayer Mayhem Initialiser
 * Initialises Multiplayer Mayhem game
 * Checks for Mat Connection
 */

public class MM_Initialiser : MonoBehaviour
{

    #region Scriptable declaration

    public static MM_Initialiser instance;

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

        // Sets game name and type in Yipli Config
        currentYipliConfig.gameId = "multiplayermayhem";
        currentYipliConfig.gameType = GameType.MULTIPLAYER_GAMING;
    }

    #endregion

    #region Variable declaration

    public YipliConfig currentYipliConfig;
    public bool isInitialized;
    public TMP_Text version;

    public event Action connectionEstablished;

    #endregion

    #region Unity directives

    // Start is called before the first frame update
    void Start()
    {
        isInitialized = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitialized)
        {
            // Checks connection status of mat and backend
            Debug.Log("Connection Test- Checking");
            if (YipliHelper.GetMatConnectionStatus().Equals("Connected", StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log("Connection Test- Connected");
                isInitialized = true;
                Debug.Log("Connection Test- Initialized");
                connectionEstablished?.Invoke();

                version.text = PlayerSession.Instance.GetDriverAndGameVersion();

                // If connection is established retrieve player data
                try
                {
                    MM_UIController.instance.GetInitialPlayerData();
                    // Setting game id to multiplayermayhem for both players
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.gameId = "multiplayermayhem";
                    PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.gameId = "multiplayermayhem";
                }
                catch (Exception e)
                {
                    Debug.Log("Connection Test Error- " + e);
                }

                // If connection is established begin input detection
                try
                {
                    MM_InputController.instance.StartInputDetection();
                }
                catch (Exception e)
                {
                    Debug.Log("Connection Test Error- " + e);
                }
                Debug.Log("Connection Test- Invoked");
            }
            else
            {
                Debug.Log("Connection Test- Not Connected");
            }
        }
    }

    #endregion

}
