using Firebase.Database;

#if UNITY_STANDALONE_WIN
using FMInterface_Windows;
#endif

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YipliFMDriverCommunication;

public class PlayerSession : MonoBehaviour
{
    private float gamePoints; // Game points / coins.
    private float duration;
    private float calories;
    private float fitnesssPoints;
    private int xp;
    public string intensityLevel = ""; // to be decided by the game.
    private IDictionary<YipliUtils.PlayerActions, int> playerActionCounts; // to be updated by the player movements
    private IDictionary<string, string> playerGameData; // to be used to store the player gameData like Highscore, last played level etc.


    public TextMeshProUGUI bleErrorText;

    public TextMeshProUGUI playerNameGreetingText;

    public GameObject YipliBackgroundPanel;
    public GameObject BleErrorPanel;
    public GameObject retryBleConnectionButton;
    public GameObject LoadingScreen;
    private GameObject instantiatedBleErrorPanel;
    public GameObject netErrorPanel;

    private bool bIsBleCheckRunning = false;

    [JsonIgnore]
    public YipliConfig currentYipliConfig;

    [JsonIgnore]
    private bool bIsPaused; // to be set, when game is paused.

    [JsonIgnore]
    private static PlayerSession _instance;

    [JsonIgnore]
    public static PlayerSession Instance { get { return _instance; } }

    public float GetGameplayDuration { get => duration; set => duration = value; }
    public float Calories { get => calories; set => calories = value; }
    public float FitnesssPoints { get => fitnesssPoints; set => fitnesssPoints = value; }

    //Delegates for Firebase Listeners
    public delegate void OnDefaultMatChanged();
    public static event OnDefaultMatChanged NewMatFound;

    [Header("InformationPanel")]
    [SerializeField] private GameObject yipliInfoPanel = null;
    [SerializeField] private TextMeshProUGUI infoPaneltext = null;

    private void Awake()
    {
        //SetMatPlayMode();
        
        if (_instance != null && _instance != this)
        {
            Debug.Log("Destroying current instance of playersession and reinitializing");
            Destroy(gameObject);
            _instance = this;
        }
        else
        {
            _instance = this;
        }

        if (currentYipliConfig.gameType == GameType.MULTIPLAYER_GAMING)
        {
            if (currentYipliConfig.userId == null || currentYipliConfig.userId.Length < 1)
            {
                _instance.currentYipliConfig.callbackLevel = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("yipli_lib_scene");
            }
        }
        else if (currentYipliConfig.playerInfo == null && currentYipliConfig.gameType != GameType.MULTIPLAYER_GAMING)
        {
            // Call Yipli_GameLib_Scene
            _instance.currentYipliConfig.callbackLevel = SceneManager.GetActiveScene().name;
            Debug.Log("Updating the callBackLevel Value to :" + _instance.currentYipliConfig.callbackLevel);
            Debug.Log("Loading Yipli scene for player Selection...");

            currentYipliConfig.bIsRetakeTutorialFlagActivated = false;
            if (!_instance.currentYipliConfig.callbackLevel.Equals("Yipli_Testing_harness"))
                SceneManager.LoadScene("yipli_lib_scene");
        }
        else
        {
            Debug.Log("Current player is not null. Not calling yipli_lib_scene");
        }
    }

    public void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //You are here, means PlayerInfo is found.
        //Invoke the player found event, to get the player data.
        if (currentYipliConfig.gameId.Length > 1 && (Application.platform == RuntimePlatform.Android))
        {
            //NewPlayerFound();
            NewMatFound();
        }
        else
        {
            Debug.LogError("Game Id not Set");
        }

        if (currentYipliConfig.gameType != GameType.MULTIPLAYER_GAMING)
        {
            playerNameGreetingText.text = "Hi, " + GetCurrentPlayer();
        }
        
        Debug.Log("Starting the BLE routine check in PlayerSession Start()");

        StartCoroutine(CheckInternetConnection());
    }

    public void Update()
    {
        Debug.Log("Game Cluster Id : " + YipliHelper.GetGameClusterId());
        
        if (currentYipliConfig.onlyMatPlayMode)
        {
            CheckMatConnection();
        }
    }

    public string GetCurrentPlayer()
    {
        return currentYipliConfig.playerInfo.playerName;
    }

    public string GetCurrentPlayerId()
    {
        return currentYipliConfig.playerInfo.playerId;
    }

    public void ChangePlayer()
    {
        _instance.currentYipliConfig.callbackLevel = SceneManager.GetActiveScene().name;
        Debug.Log("Updating the callBackLevel Value to :" + _instance.currentYipliConfig.callbackLevel);
        Debug.Log("Loading Yipli scene for player Selection...");

        firebaseDBListenersAndHandlers.SetGameDataForCurrenPlayerQueryStatus(QueryStatus.NotStarted);

        currentYipliConfig.bIsChangePlayerCalled = true;
        currentYipliConfig.pId = string.Empty;
        currentYipliConfig.playerInfo = new YipliPlayerInfo();
        SceneManager.LoadScene("yipli_lib_scene");
    }
    
    public void UpdateGameData(Dictionary<string, string> update)
    {
        if (update != null)
        {
            playerGameData = new Dictionary<string, string>();
            playerGameData = update;
        }
    }

    //To be called from void awake/start of the games 1st scene
    public void SetGameId(string gameName)
    {
        currentYipliConfig.gameId = gameName;
    }

    // Get player game data
    public DataSnapshot GetGameData()
    {
        return currentYipliConfig.gameDataForCurrentPlayer ?? null;
    }

    //First function to be called only once when the game starts()
    //To be used for error handling
    //Call in case of exception while playing game.
    public void CloseSPSession()
    {
        if (!currentYipliConfig.onlyMatPlayMode)
        {
            Debug.LogError("onlyMatPlayMode is on, returning");
            return;
        }

        //Destroy current player session data
        Calories = 0;
        FitnesssPoints = 0;
        gamePoints = 0;
        duration = 0;
        Debug.Log("Aborting current player session.");
    }

    //to be called from all the player movment actions handled script
    //To be called from GameObject FixedUpdate
    public void UpdateDuration()
    {
        Debug.Log("Updating duration for current player session.");
        if (bIsPaused == false)
        {
            duration += Time.deltaTime;
        }

        if (YipliHelper.GetMatConnectionStatus().Equals("Connected", StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("In UpdateDuration : Ble connected");
            if (BleErrorPanel.activeSelf)
            {
                FindObjectOfType<YipliAudioManager>().Play("BLE_success");
                BleErrorPanel.SetActive(false);
                YipliBackgroundPanel.SetActive(false);
            }
        }
    }

    private void CheckMatConnection()
    {
        Debug.Log("Before Processing : BleErrorPanel.activeSelf = " + BleErrorPanel.activeSelf);

        if (YipliHelper.GetMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Mat connection is established.");
            
            if (BleErrorPanel.activeSelf)
            {
                YipliBackgroundPanel.SetActive(false);
                BleErrorPanel.SetActive(false);
                FindObjectOfType<YipliAudioManager>().Play("BLE_success");
            }
        }
        else
        {
            Debug.Log("Mat connection is lost.");
            if (!BleErrorPanel.activeSelf)
            {
                // Different mat connection (error)message based on Operating system and connectivity type.
#if UNITY_ANDROID
                if (currentYipliConfig.isDeviceAndroidTV) {
                    bleErrorText.text = ProductMessages.Err_mat_connection_android_tv;
                } else {
                    bleErrorText.text = ProductMessages.Err_mat_connection_android_phone;
                }
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
                bleErrorText.text = ProductMessages.Err_mat_connection_pc;
#endif

                FindObjectOfType<YipliAudioManager>().Play("BLE_failure");
                YipliBackgroundPanel.SetActive(true);
                BleErrorPanel.SetActive(true);
            }
        }
    }

    public void StartCoroutineForBleReConnection()
    {
        try
        {
            Debug.Log("In StartCoroutineForBleReConnection.");
            if (!bIsBleCheckRunning)
                StartCoroutine(ReconnectBleFromGame());
        }
        catch(Exception e)
        {
            Debug.Log("Exception in Retrying ble connection." + e.Message);
        }
    }

    private IEnumerator ReconnectBleFromGame()
    {
        bIsBleCheckRunning = true;
        retryBleConnectionButton.SetActive(false);
        Debug.Log("In ReconnectBleFromGame.");
        try
        {
            //Initiate mat connection with last set GameCluterId
            Debug.Log("ReconnectBle with Game clster ID : " + YipliHelper.GetGameClusterId());
#if UNITY_ANDROID
            InitBLE.InitBLEFramework(currentYipliConfig.matInfo?.macAddress ?? "", YipliHelper.GetGameClusterId() != 1000 ? YipliHelper.GetGameClusterId() : 0, currentYipliConfig.matInfo?.matAdvertisingName ?? LibConsts.MatTempAdvertisingNameOnlyForNonIOS, currentYipliConfig.isDeviceAndroidTV);
#elif UNITY_IOS
            InitBLE.InitBLEFramework(currentYipliConfig.matInfo?.macAddress ?? "", 0, currentYipliConfig.matInfo?.matAdvertisingName ?? LibConsts.MatTempAdvertisingNameOnlyForNonIOS);
#else
            InitBLE.InitBLEFramework(currentYipliConfig.matInfo?.macAddress ?? "", 0);
            //InitBLE.reconnectMat();
#endif
        }
        catch (Exception exp)
        {
            Debug.Log("Exception in InitBLEFramework from ReconnectBleFromGame" + exp.Message);
        }

        //Block this function for next 5 seconds by disabling the retry Button.
        //Dont allow user to initiate Bluetooth connection for atleast 5 secs, as 1 connecteion initiation is enough.
        yield return new WaitForSecondsRealtime(5f);
        retryBleConnectionButton.SetActive(true);
        bIsBleCheckRunning = false;
    }

    public void LoadingScreenSetActive(bool bOn)
    {
        Debug.Log("Loading Screen called : " + bOn);
        YipliBackgroundPanel.SetActive(bOn);
        LoadingScreen.SetActive(bOn);
    }

    // Update store data witout gameplay. To be called by games Shop Manager.
    public void UpdateStoreData(Dictionary<string, object> dStoreData)
    {
        FirebaseDBHandler.UpdateStoreData(
            currentYipliConfig.userId,
            currentYipliConfig.playerInfo.playerId,
            currentYipliConfig.gameId,
            dStoreData,
            () => { Debug.Log("Got Game data successfully"); }
        );
    }

    public void GotoYipli()
    {
        YipliHelper.GoToYipli(ProductMessages.openYipliApp);
    }

#region Single Player Session Functions

    public void ReInitializeSPSession()
    {
        if (!currentYipliConfig.onlyMatPlayMode)
        {
            Debug.LogError("onlyMatPlayMode is on, returning");
            return;
        }

        gamePoints = 0;
        duration = 0;
        bIsPaused = false;
        ActionAndGameInfoManager.SetYipliGameInfo(currentYipliConfig.gameId);
    }

    public IDictionary<YipliUtils.PlayerActions, int> getPlayerActionCounts()
    {
        return playerActionCounts;
    }
    
    public Dictionary<string, dynamic> GetPlayerSessionDataJsonDic()
    {
        Dictionary<string, dynamic> x;
        x = new Dictionary<string, dynamic>();
        x.Add("game-id", currentYipliConfig.gameId);
        x.Add("user-id", currentYipliConfig.userId);
        x.Add("player-id", currentYipliConfig.playerInfo.playerId);
        x.Add("age", int.Parse(currentYipliConfig.playerInfo.playerAge));
        x.Add("points", (int)gamePoints);
        x.Add("height", currentYipliConfig.playerInfo.playerHeight);
        x.Add("duration", (int)duration);
        x.Add("intensity", intensityLevel);
        x.Add("player-actions", playerActionCounts);
        x.Add("timestamp", ServerValue.Timestamp);
        x.Add("calories", (int)GetCaloriesBurned());
        x.Add("fitness-points", (int)GetFitnessPoints());

        if (playerGameData != null)
        {
            if (playerGameData.Count > 0)
            {
                x.Add("game-data", playerGameData);
            }
            else
            {
                Debug.Log("Game-data is empty");
            }
        }
        else
        {
            Debug.Log("Game-data is null");
        }

        // firebase function ignores and deletes folowing 2 lines. Uncomment lines once FB function is updated
        //x.Add("os", Application.platform);
        //x.Add("game-version", GetDriverAndGameVersion());

        //Removed following, since mat-id and mac-address couldnt be got on windows
        //x.Add("mat-id", currentYipliConfig.matInfo.matId);
        //x.Add("mac-address", currentYipliConfig.matInfo.macAddress);

        #if UNITY_ANDROID
        if (currentYipliConfig.isDeviceAndroidTV) {
            x.Add("os", "atv");
        } else {
            x.Add("os", "a");
        }
#elif UNITY_IOS
        x.Add("os", "i");
#elif UNITY_STANDALONE_WIN
        x.Add("os", "w");
#endif

        x.Add("game-version", Application.version);

        return x;
    }

    public void StartSPSession()
    {
        if (!currentYipliConfig.onlyMatPlayMode)
        {
            Debug.LogError("onlyMatPlayMode is on, returning");
            return;
        }

        Debug.Log("Starting current player session.");
        playerActionCounts = new Dictionary<YipliUtils.PlayerActions, int>();
        gamePoints = 0;
        duration = 0;
        bIsPaused = false;
        ActionAndGameInfoManager.SetYipliGameInfo(currentYipliConfig.gameId);
    }

    public void StoreSPSession(float gamePoints)
    {
        if (!currentYipliConfig.onlyMatPlayMode)
        {
            Debug.LogError("onlyMatPlayMode is on, returning");
            return;
        }

        Debug.Log("Storing current player session to backend database.");
        this.gamePoints = gamePoints;

        //Calories = YipliUtils.GetCaloriesBurned(getPlayerActionCounts());
        //FitnesssPoints = YipliUtils.GetFitnessPointsWithRandomization(getPlayerActionCounts());
        xp = YipliUtils.GetXP(Math.Ceiling(duration));

        if (0 == ValidateSessionBeforePosting())
        {
            //Store the session data to backend.
            FirebaseDBHandler.PostPlayerSession(Instance, () => { Debug.Log("Session stored in db"); });
            Debug.Log("Single player session stored successfully.");
        }
        else
        {
            Debug.Log("Session not posted : Validation failed for sessoin data.");
        }
    }


    //Function to validate all the session parameters before writing to DB
    //To be called from GamePause function
    //To be called from GameResume function
    private int ValidateSessionBeforePosting()
    {
        if (currentYipliConfig.gameId == null || currentYipliConfig.gameId == "")
        {
            Debug.Log("gameId is not set");
            return -1;  
        }
        if (currentYipliConfig.playerInfo.playerId == null || currentYipliConfig.playerInfo.playerId == "")
        {
            Debug.Log("playerId is not set");
            return -1;
        }
        if (playerActionCounts.Count == 0)
        {
            Debug.Log("playerActionCounts is not set");
            return -1;
        }
        if (duration == 0)
        {
            Debug.Log("duration is 0");
            return -1;
        }
        if (intensityLevel == "")
        {
            Debug.Log("intensityLevel is not set");
            return -1;
        }
        return 0;
    }
    public void PauseSPSession()
    {
        if (!currentYipliConfig.onlyMatPlayMode)
        {
            Debug.LogError("onlyMatPlayMode is on, returning");
            return;
        }

        Debug.Log("Pausing current player session.");
        bIsPaused = true; // only set the paused flat to true. Fixed update will take care of halting the time counter
                          //Ble check
        if (!YipliHelper.GetMatConnectionStatus().Equals("Connected", StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("In PauseSPSession : Ble disconnected");
            if (!BleErrorPanel.activeSelf)
            {
                FindObjectOfType<YipliAudioManager>().Play("BLE_failure");
                YipliBackgroundPanel.SetActive(true);
                BleErrorPanel.SetActive(true);
            }
        }
    }
    public void ResumeSPSession()
    {
        if (!currentYipliConfig.onlyMatPlayMode)
        {
            Debug.LogError("onlyMatPlayMode is on, returning");
            return;
        }

        Debug.Log("Resuming current player session.");
        bIsPaused = false;
    }
    public void AddPlayerAction(YipliUtils.PlayerActions action, int count = 1)
    {
        if (!currentYipliConfig.onlyMatPlayMode)
        {
            Debug.LogError("onlyMatPlayMode is on, returning");
            return;
        }

        if (count < 1) return;

        Debug.Log("Adding action in current player session.");
        if (playerActionCounts.ContainsKey(action))
        {
            playerActionCounts[action] = playerActionCounts[action] + count;
        }
        else
        {
            playerActionCounts.Add(action, count);
        }

        FitnesssPoints += YipliUtils.GetFitnessPointsPerAction(action) * count * UnityEngine.Random.Range(0.92f, 1.04f); // this is to hide direct mapping between calories and fitnesspoint. small random multiplier is added fitness points to keep it random on single action level
        Calories += YipliUtils.GetCaloriesPerAction(action) * count;
    }

    #endregion

    #region Multi Player Session Functions

    public IDictionary<YipliUtils.PlayerActions, int> getMultiPlayerActionCounts(PlayerDetails playerDetails)
    {
        return playerDetails.playerActionCounts;
    }
    public Dictionary<string, dynamic> GetMultiPlayerSessionDataJsonDic(PlayerDetails playerDetails, string mpSessionUUID)
    {
        Debug.Log("UUID= " + mpSessionUUID);

        Dictionary<string, dynamic> x;
        x = new Dictionary<string, dynamic>();

        x.Add("game-id", playerDetails.gameId);
        x.Add("user-id", playerDetails.userId);
        x.Add("player-id", playerDetails.playerId);
        x.Add("minigame-id", playerDetails.minigameId);
        x.Add("age", int.Parse(playerDetails.playerAge));
        x.Add("points", (int)playerDetails.points);
        x.Add("height", playerDetails.playerHeight);
        x.Add("duration", (int)playerDetails.duration);
        x.Add("intensity", playerDetails.intensityLevel);
        x.Add("player-actions", playerDetails.playerActionCounts);
        x.Add("timestamp", ServerValue.Timestamp);
        x.Add("mp-session-id", mpSessionUUID);
        x.Add("calories", (int)playerDetails.calories);
        x.Add("fitness-points", (int)playerDetails.fitnesssPoints);
        x.Add("xp", xp);

        if (playerGameData != null)
        {
            if (playerGameData.Count > 0)
            {
                x.Add("game-data", playerGameData);
            }
            else
            {
                Debug.Log("Game-data is empty");
            }
        }
        else
        {
            Debug.Log("Game-data is null");
        }

        return x;
    }
    public void StartMPSession()
    {
        if (!currentYipliConfig.onlyMatPlayMode)
        {
            Debug.LogError("onlyMatPlayMode is on, returning");
            return;
        }

        Debug.Log("Starting multi player session.");
        currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.playerActionCounts = new Dictionary<YipliUtils.PlayerActions, int>();
        currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.playerActionCounts = new Dictionary<YipliUtils.PlayerActions, int>();

        ActionAndGameInfoManager.SetYipliMultiplayerGameInfo(currentYipliConfig.MP_GameStateManager.minigameId);

        currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.points = 0;
        currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.points = 0;
        currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.duration = 0;
        currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.duration = 0;

        currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.calories = 0;
        currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.fitnesssPoints = 0;
        currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.calories = 0;
        currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.fitnesssPoints = 0;

        duration = 0;

        bIsPaused = false;
    }
    public void StoreMPSession(float playerOneGamePoints, float playerTwoGamePoints)
    {
        if (!currentYipliConfig.onlyMatPlayMode)
        {
            Debug.LogError("onlyMatPlayMode is on, returning");
            return;
        }

        Debug.LogError("Duration is " + duration);
        currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.duration = duration;
        currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.duration = duration;

        Debug.Log("Storing current player session to backend database.");

        Debug.Log("Count Test- " + currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.playerActionCounts.Count + " , " + currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.playerActionCounts.Count);


        currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails.points = playerOneGamePoints;
        currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails.points = playerTwoGamePoints;

        string mpSessionUUID = Guid.NewGuid().ToString();

        if (0 == ValidateMPSessionBeforePosting(currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails))
        {
            //Store the session data to backend.
            FirebaseDBHandler.PostMultiPlayerSession(Instance, currentYipliConfig.MP_GameStateManager.playerData.PlayerOneDetails, mpSessionUUID, () => { Debug.Log("Session stored in db"); });
            Debug.Log("Player 1 session stored successfully.");
        }
        else
        {
            Debug.Log("Session not posted : Validation failed for sessoin data.");
        }

        if (0 == ValidateMPSessionBeforePosting(currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails))
        {
            //Store the session data to backend.
            FirebaseDBHandler.PostMultiPlayerSession(Instance, currentYipliConfig.MP_GameStateManager.playerData.PlayerTwoDetails, mpSessionUUID, () => { Debug.Log("Session stored in db"); });
            Debug.Log("Player 2 session stored successfully.");
        }
        else
        {
            Debug.Log("Session not posted : Validation failed for sessoin data.");
        }
    }
    private int ValidateMPSessionBeforePosting(PlayerDetails playerDetails)
    {
        if (playerDetails.gameId == null || playerDetails.gameId == "")
        {
            Debug.Log("gameId is not set");
            return -1;
        }
        if (playerDetails.playerId == null || playerDetails.playerId == "")
        {
            Debug.Log("playerId is not set");
            return -1;
        }
        if (playerDetails.playerActionCounts.Count == 0)
        {
            Debug.Log("playerActionCounts is not set");
            return -1;
        }
        /* TODO : Confirm with kurus
        else if (playerDetails.playerActionCounts.Count < 1)
        {
            playerDetails.calories = 1;
        }
        */
        
        if (playerDetails.duration == 0)
        {
            Debug.Log("duration is 0");
            return -1;
        }
        return 0;
    }
    public void PauseMPSession()
    {
        if (!currentYipliConfig.onlyMatPlayMode)
        {
            Debug.LogError("onlyMatPlayMode is on, returning");
            return;
        }

        PauseSPSession();
    }
    public void ResumeMPSession()
    {
        if (!currentYipliConfig.onlyMatPlayMode)
        {
            Debug.LogError("onlyMatPlayMode is on, returning");
            return;
        }

        ResumeSPSession();
    }
    public void AddMultiPlayerAction(YipliUtils.PlayerActions action, PlayerDetails playerDetails, int count = 1)
    {
        if (!currentYipliConfig.onlyMatPlayMode)
        {
            Debug.LogError("onlyMatPlayMode is on, returning");
            return;
        }

        Debug.Log("Adding action in current player session.");
        if (playerDetails.playerActionCounts.ContainsKey(action))
            playerDetails.playerActionCounts[action] = playerDetails.playerActionCounts[action] + count;
        else
            playerDetails.playerActionCounts.Add(action, count);

        playerDetails.calories += YipliUtils.GetCaloriesPerAction(action) * count;
        playerDetails.fitnesssPoints += YipliUtils.GetFitnessPointsPerAction(action) * count * UnityEngine.Random.Range(0.92f, 1.04f); // this is to hide direct mapping between calories and fitnesspoint. small random multiplier is added fitness points to keep it random on single action level
    }

    #endregion

    // get game and driver version
    public string GetDriverAndGameVersion()
    {
        return YipliHelper.GetFMDriverVersion() + " : " + Application.version;
    }

    // get fitness poins
    public float GetFitnessPoints()
    {
        return FitnesssPoints;
    }

    // get calories
    public float GetCaloriesBurned()
    {
        if (Calories < 1f)
        {
            return 1f;
        }

        return Calories;
    }

    // network connection panel management
    private IEnumerator CheckInternetConnection()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(1f);

            if (YipliHelper.checkInternetConnection())
            {
                if (netErrorPanel.activeSelf)
                {
                    YipliBackgroundPanel.SetActive(false);
                    netErrorPanel.SetActive(false);
                    FindObjectOfType<YipliAudioManager>().Play("BLE_success");
                }
            }
            else
            {
                Debug.Log("Internect connection is lost.");
                if (!netErrorPanel.activeSelf)
                {
                    FindObjectOfType<YipliAudioManager>().Play("BLE_failure");
                    YipliBackgroundPanel.SetActive(true);
                    netErrorPanel.SetActive(true);
                }
            }
        }
    }

    // quit from playersession canvas
    public void QuitApplication()
    {
        Application.Quit();
    }

    // retake tutorial
    public void RetakeMatControlsTutorial()
    {
        if (currentYipliConfig.onlyMatPlayMode)
        {
            _instance.currentYipliConfig.callbackLevel = SceneManager.GetActiveScene().name;
            //currentYipliConfig.bIsRetakeTutorialFlagActivated = true;
            //SceneManager.LoadScene("yipli_lib_scene");
            SceneManager.LoadScene("gameLibTutorial");
        }
        else
        {
            // info panel text management
            infoPaneltext.text = "Mat Tutorial is not available in preview mode.";
            yipliInfoPanel.SetActive(true);
        }
    }

    public void YipliInfoPanleOkayButton()
    {
        // info panel text management
        infoPaneltext.text = "";
        yipliInfoPanel.SetActive(false);
    }

    // set mat play mode
    public void SetMatPlayMode()
    {
#if UNITY_EDITOR
        currentYipliConfig.onlyMatPlayMode = false;
#elif UNITY_ANDROID || UNITY_IOS
        currentYipliConfig.onlyMatPlayMode = true;
#elif UNITY_STANDALONE_WIN
        currentYipliConfig.onlyMatPlayMode = true;   
#endif
    }

    // TroubleShoot System
    public void TroubleShootSystem()
    {
        SceneManager.LoadScene("Troubleshooting");
    }

    // Ticket system
    // Update current ticket data.
    public void UpdateCurrentTicketData(Dictionary<string, object> currentTicketData)
    {
        FirebaseDBHandler.UpdateCurrentTicketData(
            currentYipliConfig.userId,
            currentTicketData,
            () => { Debug.Log("Ticket Generated successfully"); }
        );
    }

   // #if UNITY_STANDALONE_WIN
        // application quit systems
        void OnApplicationQuit()
        {
            #if UNITY_STANDALONE_WIN
                Debug.LogError("Inside OnApplicationQuit");
                DeviceControlActivity._disconnect();
                DeviceControlActivity.readThread.Abort();
            #elif UNITY_IOS
                InitBLE.DisconnectMat();
            #endif
        }
    //#endif

    // Application State Management
    void OnApplicationFocus(bool focus)
    {
#if UNITY_IOS
        if (focus)
        {
            Debug.LogError("Test Poc is in focus : " + focus + " .. Reconnecting mat");
            InitBLE.ConnectPeripheral(InitBLE.MAT_UUID);
        }
        else
        {
            Debug.LogError("Test Poc is in focus : " + focus + " .. Disconnecting mat");
            InitBLE.DisconnectMat();
        }
#endif
    }

    // Test functions
    public void PrintBundleIdentifier() {
        PlayerPrefs.SetString("skippedDate", new DateTime(2021, 07, 21).ToString());
    }

    private void TimeDifferenceManager() {
        PlayerPrefs.SetString("skippedDate", DateTime.Today.ToString());

        DateTime todaysDate = new DateTime(2021, 07, 21);
        DateTime skippedDate = DateTime.Parse(PlayerPrefs.GetString("skippedDate"));

        int totalDaysSinceLastSkipped = (int)(todaysDate - skippedDate).TotalDays;

        Debug.LogError("todaysDate : " + totalDaysSinceLastSkipped);
    }
}