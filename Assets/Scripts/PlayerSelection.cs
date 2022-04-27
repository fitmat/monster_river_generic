using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Threading.Tasks;
using Firebase.DynamicLinks;
using UnityEngine.SceneManagement;

#if UNITY_STANDALONE_WIN
using yipli.Windows;
#endif

public class PlayerSelection : MonoBehaviour
{
    public GameObject phoneHolderInfo;
    public GameObject LaunchFromYipliAppPanel;
    public GameObject phoneAnimationOBJ;
    public GameObject stickAnimationOBJ;
    public GameObject pcAnimationOBJ;
    public GameObject playerSelectionPanel;
    //public TextMeshProUGUI playerNameText;
    //public TextMeshProUGUI onlyOnePlayerText;
    public GameObject PlayersContainer;
    public GameObject PlayersMenuObject;
    public GameObject switchPlayerPanel;
    //public GameObject zeroPlayersPanel;
    public GameObject GuestUserPanel;
    //public TextMeshProUGUI zeroPlayersText;
    public YipliConfig currentYipliConfig;
    public GameObject PlayerButtonPrefab;
    //public GameObject onlyOnePlayerPanel;
    public GameObject LoadingPanel;
    public GameObject noNetworkPanel;
    //public TextMeshProUGUI noNetworkPanelText;
    public MatSelection matSelectionScript;
    //public Image profilePicImage;
    //public GameObject RemotePlayCodePanel;
    public GameObject Minimum2PlayersPanel;
    //public GameObject GameVersionUpdatePanel;
    public GameObject TutorialPanel;

    //public SecondTutorialManager secondTutorialManager;

    //public TextMeshProUGUI GameVersionUpdateText;
    //public TextMeshProUGUI RemotePlayCodeErrorText;

    public TextMeshProUGUI gameAndDriverVersionText;
    //public TextMeshProUGUI learnMatControlText;
    public TextMeshProUGUI switchPlayerPanelText;

    //[Header("Retries Button")]
    //[SerializeField] private Button onlyOnePlayerPanelRetryButton;

    private string PlayerName;

    private List<GameObject> generatedObjects = new List<GameObject>();
    private bool bIsCheckingForIntents;
    private bool bIsProfilePicLoaded = false;
    private Sprite defaultProfilePicSprite;

    private bool isSkipUpdateCalled = false;
    private bool isTutorialDoneWithoutPlayerInfo = false;

    private bool learnMatControlIsClicked = false;

    private float currentTimePassed = 0;
    private bool allowPhoneHolderAudioPlay = false;

    //Delegates for Firebase Listeners
    public delegate void OnUserFound();
    public static event OnUserFound NewUserFound;

    public delegate void OnPlayerChanged();
    public static event OnPlayerChanged DefaultPlayerChanged;

    public delegate void OnGameLaunch();
    public static event OnUserFound GetGameInfo;

    public delegate void OnTicketData();
    public static event OnTicketData TicketData;

    public static event OnUserFound GetAllMats;

    [Header("UIMangers")]
    [SerializeField] private NewUIManager newUIManager = null;
    [SerializeField] private NewMatInputController newMatInputController = null;
    [SerializeField] private MatInputController matInputController;

    // link informations
    string uId = string.Empty;
    //string pId = string.Empty;
    string pName = string.Empty;
    string pDOB = string.Empty;
    string pHt = string.Empty;
    string pWt = string.Empty;
    string pPicUrl = string.Empty;
    string mId = string.Empty;
    string mMac = string.Empty;
    string mName = string.Empty;
    string pTutDone = string.Empty;

    bool dataSetsAreFilled = false;

    // new bool flags for process verifications
    private bool userIDInitiated = false;
    private bool defaultPlayerIDInitiated = false;
    private bool phoneHolderTUTPlayed = false;
    private bool psFlowForCPcalled = false;
    private bool startDataManagement = false;
    private bool onlyMatPlayModeIsSet = false;
    private bool matConnectionStarted = false;

    public void OnEnable()
    {
        //defaultProfilePicSprite = profilePicImage.sprite;
    }

    // When the game starts
#if UNITY_STANDALONE_WIN
        private async void Start()
#else
    private void Start()
#endif
    {
        currentYipliConfig.allowMainGameSceneToLoad = false;

        //Todo : Verify that following line is not needed.
        //newMatInputController.DisableMatParentButtonAnimator();

        TurnOffAllDeviceSpecificTextObject();

        //StartCoroutine(CheckNoInternetConnection());

        //Todo: Can shift this to onEnable ?
        UpdateGameAndDriverVersionText();

        // Game info and update check before player selection
        GetGameInfo();


        //Todo: Shift the following code to llistner basis. Avoid a get call.
        //Data could be ready already before reaching here.
#if UNITY_STANDALONE_WIN
        //keep yipli app Download Url ready
        FileReadWrite.YipliAppDownloadUrl = await FirebaseDBHandler.GetYipliWinAppUpdateUrl();

        FetchUserDetailsForWindowsAndEditor();

        //FetchUserAndInitializePlayerEnvironment();
#endif


        // comment below part is current platform is not UNITY_STANDALONE_WIN
//#if UNITY_EDITOR
//        //FetchUserDetailsForWindowsAndEditor();

//        if (!currentYipliConfig.bIsChangePlayerCalled)
//        {
//            FetchUserDetailsForWindowsAndEditor();
//        }
//#endif


#if UNITY_ANDROID || UNITY_IOS
        StartCoroutine(RelaunchgameFromYipliApp());

        //if (currentYipliConfig.bIsChangePlayerCalled) {
        //    FetchUserAndInitializePlayerEnvironment();
        //}
#endif

        if (currentYipliConfig.bIsChangePlayerCalled)
        {
            ManageChangePlayer();
        }

        // set link data first
        //await SetLinkData();

        //FetchUserAndInitializePlayerEnvironment();

        // turn of all devicespecific tutorial objects invisible
    }

    private void Update()
    {
        if (!onlyMatPlayModeIsSet)
        {
            SetOnlyMatPlayMode();
        }

        if (allowPhoneHolderAudioPlay)
        {
            PlayComeAndJumpAudio();
        }

        if (!playerSelectionPanel.activeSelf)
        {
            DestroyPlayerSelectionButtons();
        }

        if (playerSelectionPanel.activeSelf)
        {
            matInputController.IsThisPlayerSelectionPanel = true;
        }
        else
        {
            matInputController.IsThisPlayerSelectionPanel = false;
        }

        if (startDataManagement)
        {
            ManageDataMaintanence();
        }

        // panels with buttons
        // noInternetPanel, noMatPanel(Guest User panel), maintanencePanel, noMatConnectionPanel, phoneHolderTutorialPanel, minimum2Player
        // if (noNetworkPanel.activeSelf || GuestUserPanel.activeSelf || phoneHolderInfo.activeSelf || Minimum2PlayersPanel.activeSelf) {
        //     newUIManager.TurnOnMainCommonButton();
        // } else {
        //     newUIManager.TurnOffMainCommonButton();
        // }

        //ManageRetriesButtonOnDifferentPanel();
    }

    // turn of all devicespecific tutorial objects invisible
    private void TurnOffAllDeviceSpecificTextObject()
    {
        phoneAnimationOBJ.SetActive(false);
        stickAnimationOBJ.SetActive(false);
        pcAnimationOBJ.SetActive(false);
    }

    public void OnUpdateGameClick()
    {
        string gameAppId = Application.identifier;
        Debug.Log("App Id is : " + gameAppId);
        YipliHelper.GoToPlaystoreUpdate(gameAppId);
    }

    public void OnSkipUpdateClick()
    {
        isSkipUpdateCalled = true;
        FetchUserAndInitializePlayerEnvironment();
    }

    //Whenever the Yipli App launches the game, the user will be found and next flow will be called automatically.
    //No need to keep retrying.
    //Start this coroutine to check for intents till a valid user is not found.
    IEnumerator KeepFindingUserId()
    {
        yield return new WaitForSeconds(.5f);
        Debug.Log("Started Coroutine : KeepCheckingForIntents");
        while (currentYipliConfig.userId == null || currentYipliConfig.userId.Length < 1)
        {
            Debug.Log("Calling CheckIntentsAndInitializePlayerEnvironment()");

            // Read intents and Initialize defaults
            FetchUserDetailsForAndroidAndIOS();

            yield return new WaitForSeconds(0.2f);
        }
        Debug.Log("Ending Coroutine. UserId = " + currentYipliConfig.userId);
        //StartCoroutine(InitializeAndStartPlayerSelection());
    }

    private void FetchUserAndInitializePlayerEnvironment()
    {
        Debug.Log("Deep link : In CheckIntentsAndInitializePlayerEnvironment()");

        // Read intents and Initialize defaults
        //FetchUserDetailsForAndroidAndIOS();

        //StartCoroutine(InitializeAndStartPlayerSelection());
        startDataManagement = true;
    }

    public void playDeviceSpecificMatTutorial()
    {
        //Debug.LogError("Retake tutorial : currentYipliConfig.bIsChangePlayerCalled : " + currentYipliConfig.bIsChangePlayerCalled);

        if (currentYipliConfig.bIsChangePlayerCalled) return;

        if (phoneHolderTUTPlayed) return;

        //Debug.LogError("Retake tutorial : next line to turn off all panels");
        TurnOffAllPanels();
        //Debug.LogError("Retake tutorial : all panels are off");

        phoneHolderInfo.SetActive(true);
        newUIManager.UpdateButtonDisplay(phoneHolderInfo.gameObject.tag);

        //Debug.LogError("Retake tutorial : holder panels are activated");

        if (YipliHelper.GetMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase))
        {
            phoneHolderInfo.GetComponent<AudioSource>().Play();
        }

        //Debug.LogError("Retake tutorial : audio is played");

        allowPhoneHolderAudioPlay = true;

        Debug.LogError("phoneHolderTUTPlayed : " + phoneHolderTUTPlayed);

        phoneHolderTUTPlayed = true;

#if UNITY_ANDROID || UNITY_IOS
        //StartCoroutine(ChangeTextMessageAndoridPhone());
        if (currentYipliConfig.isDeviceAndroidTV)
        {
            PlayTVStickTutStartAnimation();
        }
        else
        {
            PlayPhoneTutStartAnimation();
        }
        //Debug.LogError("Retake tutorial : proper animation is activated");
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        //StartCoroutine(ChangeTextMessageWindowsPC());
        PlayPCTutStartAnimation();
#endif
    }

    private void PlayComeAndJumpAudio()
    {
        if (YipliHelper.GetMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase))
        {
            currentTimePassed += Time.deltaTime;

            if (currentTimePassed > 30f)
            {
                phoneHolderInfo.GetComponent<AudioSource>().Play();
            }
        }
    }

    void PlayPhoneTutStartAnimation()
    {
        phoneAnimationOBJ.SetActive(true);
    }

    void PlayTVStickTutStartAnimation()
    {
        stickAnimationOBJ.SetActive(true);
    }

    void PlayPCTutStartAnimation()
    {
        pcAnimationOBJ.SetActive(true);
    }

    private void NoUserFoundInGameFlow()
    {
        //Go to Yipli Panel
        TurnOffAllPanels();

        //playerNameText.gameObject.SetActive(false);

        //Depending upon if Main Yipli App is installed or not, take a decision to show No user panel / or Guest User Panel
        if (false == YipliHelper.IsYipliAppInstalled())
        {
            FindObjectOfType<YipliAudioManager>().Play("BLE_failure");
            //newUIManager.UpdateButtonDisplay(GuestUserPanel.tag);
            GuestUserPanel.SetActive(true);
        }
        else
        {
            Debug.Log("Calling RedirectToYipliAppForNoUserFound()");
            //Automatically redirect to Yipli App
            StartCoroutine(RedirectToYipliAppForNoUserFound());
        }
    }

    IEnumerator RedirectToYipliAppForNoUserFound()
    {
        Debug.Log("In RedirectToYipliAppForNoUserFound()");

        LaunchFromYipliAppPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        FindObjectOfType<YipliAudioManager>().Play("BLE_failure");
        yield return new WaitForSecondsRealtime(4f);

        LaunchFromYipliAppPanel.SetActive(false);
        LoadingPanel.SetActive(true);

        Debug.Log("Calling KeepFindingUserId()");
        //Currently User isnt found. Start a coroutine which keeps on checking for the user details.
        //StartCoroutine(KeepFindingUserId());

        Debug.Log("Redirecting to Yipli App");
        //YipliHelper.GoToYipli(ProductMessages.noUserFound);
        YipliHelper.GoToYipli(ProductMessages.relaunchGame, currentYipliConfig.gameId);
    }

    public void OnEnterPressedAfterCodeInput()
    {
        //string codeEntered = RemotePlayCodePanel.GetComponentInChildren<InputField>().text;
        //if (codeEntered.Length != 6)
        //{
        //    FindObjectOfType<YipliAudioManager>().Play("BLE_failure");
        //    Debug.Log("Please enter valid 6 digit code. Code : " + codeEntered);
        //    //RemotePlayCodeErrorText.gameObject.SetActive(true);
        //    //RemotePlayCodeErrorText.text = "Enter valid 6 digit code";
        //}
        //else
        //{
        //    //RemotePlayCodeErrorText.text = "";
        //    //RemotePlayCodePanel.SetActive(false);
        //    LoadingPanel.SetActive(true);
        //    // Write logic to check the code with the backend and retrive the UserId
        //    string UserId = await FirebaseDBHandler.GetUserIdFromCode(codeEntered);
        //    LoadingPanel.SetActive(false);
        //    Debug.Log("Got UserId : " + UserId);
        //    currentYipliConfig.userId = UserId;
        //    currentYipliConfig.playerInfo = new YipliPlayerInfo();
        //    currentYipliConfig.matInfo = new YipliMatInfo();
        //    PlayerSelectionFlow();
        //}
    }

    //Here default player object is filled with the intents passed.
    //If no intents found, it is filled with the device persisted default player object.
    private void InitDefaultPlayer()
    {
        if (currentYipliConfig.playerInfo != null && (currentYipliConfig.pId != null || currentYipliConfig.pId != "" || currentYipliConfig.pId != string.Empty))
        {
            //If PlayerInfo is found in the Intents as an argument.
            //This code block will be called when the game App is launched from the Yipli app.
            //UserDataPersistence.SavePlayerToDevice(currentYipliConfig.playerInfo);

            PlayerSelectionFlow();
        }
        else
        {
            //If there is no PlayerInfo found in the Intents as an argument.
            //This code block will be called when the game App is not launched from the Yipli app.
            //currentYipliConfig.playerInfo = UserDataPersistence.GetSavedPlayer();

            if (currentYipliConfig.pId != null || currentYipliConfig.pId != "" || currentYipliConfig.pId != string.Empty)
            {
                // seting player form all player list based on received pId
                foreach (YipliPlayerInfo thisListCurrentPlayerInfo in currentYipliConfig.allPlayersInfo)
                {
                    if (thisListCurrentPlayerInfo.playerId == currentYipliConfig.pId)
                    {
                        //currentYipliConfig.playerInfo = new YipliPlayerInfo(pId, pName, pDOB, pHt, pWt, pPicUrl, YipliHelper.StringToIntConvert(pTutDone));
                        currentYipliConfig.playerInfo = thisListCurrentPlayerInfo;
                    }
                }
            }
            else
            {
                PlayerSelectionFlow();
            }
        }

        if (currentYipliConfig.playerInfo != null)
        {
            //Notify the listeners to start gathering the players gamedata
            DefaultPlayerChanged();
            defaultPlayerIDInitiated = true;
        }
        else
        {
            Debug.Log("Deep link : currentYipliConfig.playerInfo is still null");
        }
    }

    //Here default player object is filled with the intents passed.
    //If no intents found, it is filled with the device persisted default player object.
    private void InitDefaultMat()
    {
        //Not storing Default player in scriptable object as it would be done later.
        if (currentYipliConfig.matInfo != null)
        {
            //If PlayerInfo is found in the Intents as an argument.
            //This code block will be called when the game App is launched from the Yipli app.
            UserDataPersistence.SaveMatToDevice(currentYipliConfig.matInfo);
        }
        else
        {
            //If there is no PlayerInfo found in the Intents as an argument.
            //This code block will be called when the game App is not launched from the Yipli app.
            currentYipliConfig.matInfo = UserDataPersistence.GetSavedMat();
        }
    }

    private void InitUserId()
    {
        /*
        if (currentYipliConfig.userId != null && currentYipliConfig.userId.Length > 1)
        {
            //If UserId is found in the Intents as an argument.
            //This code block will be called when the game App is launched from the Yipli app.
            //UserDataPersistence.SavePropertyValue("user-id", currentYipliConfig.userId);
            //PlayerPrefs.Save();
        }
        else
        {
            //If there is no UserId found in the Intents as an argument.
            //This code block will be called when the game App is not launched from the Yipli app.
            //currentYipliConfig.userId = UserDataPersistence.GetPropertyValue("user-id");
        }
        */

        if (currentYipliConfig.userId != null && currentYipliConfig.userId.Length > 1)
        {
            //Trigger the database listeners as sson as the user is found
            NewUserFound();
            TicketData();

            userIDInitiated = true;
        }
    }

    private void ReadAndroidIntents()
    {
        Debug.Log("Deep link : Reading intents.");
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
        AndroidJavaObject extras = intent.Call<AndroidJavaObject>("getExtras");

        currentYipliConfig.userId = extras.Call<string>("getString", "uId");
        if (currentYipliConfig.userId == null)
        {
            Debug.Log("Deep link : Returning from readIntents as no userId found.");
            throw new Exception("Deep link : UserId is not found");
        }

        string pId = extras.Call<string>("getString", "pId");
        string pName = extras.Call<string>("getString", "pName");
        string pDOB = extras.Call<string>("getString", "pDOB");
        string pHt = extras.Call<string>("getString", "pHt");
        string pWt = extras.Call<string>("getString", "pWt");
        string pic = extras.Call<string>("getString", "pPicUrl");
        string mId = extras.Call<string>("getString", "mId");
        string mMac = extras.Call<string>("getString", "mMac");
        string pTutDone = extras.Call<string>("getString", "pTutDone");

        Debug.Log("Deep link : Found intents : " + currentYipliConfig.userId + ", " + pId + ", " + pDOB + ", " + pHt + ", " + pWt + ", " + pName + ", " + mId + ", " + mMac + "," + pic);

        if (pId != null && pName != null)
        {
            currentYipliConfig.playerInfo = new YipliPlayerInfo(pId, pName, pDOB, pHt, pWt, pic, YipliHelper.StringToIntConvert(pTutDone));
        }

        if (mId != null && mMac != null)
        {
            currentYipliConfig.matInfo = new YipliMatInfo(mId, mMac);
        }
    }

#if UNITY_STANDALONE_WIN
        public void SetLinkData()
#else
    public async void SetLinkData()
#endif
    {
        //Debug.Log("Found intents : " + currentYipliConfig.userId + ", " + pId + ", " + pDOB + ", " + pHt + ", " + pWt + ", " + pName + ", " + mId + ", " + mMac + "," + pPicUrl);

        /*
        if (pId != null && pName != null)
        {
            currentYipliConfig.playerInfo = new YipliPlayerInfo(pId, pName, pDOB, pHt, pWt, pPicUrl, YipliHelper.StringToIntConvert(pTutDone));
        }
        */

        Debug.Log("Deep link : from set link data");

        if (currentYipliConfig.userId == null || currentYipliConfig.userId == string.Empty || currentYipliConfig.userId == "")
        {
            Debug.Log("Deep link : from set link data : currentYipliConfig.userId is empty, returning");
            return;
        }

#if UNITY_ANDROID || UNITY_IOS
        // get all mat list og this user
        //GetAllMats();

        currentYipliConfig.currentMatID = await FirebaseDBHandler.GetCurrentMatIdOfUserId(currentYipliConfig.userId);
        Debug.LogError("Retake Tutorial : current mat ID : " + currentYipliConfig.currentMatID);
        currentYipliConfig.currentMatDetails = await FirebaseDBHandler.GetMatDetailsOfUserId(currentYipliConfig.userId, currentYipliConfig.currentMatID);
        Debug.LogError("Retake Tutorial : current mat address : " + currentYipliConfig.currentMatDetails.Child("mac-address").Value.ToString());
#endif

#if UNITY_ANDROID
        //currentYipliConfig.matInfo = new YipliMatInfo(mId, mMac);

        if (currentYipliConfig.isDeviceAndroidTV)
        {
            Debug.Log("Android TV  =  " + currentYipliConfig.isDeviceAndroidTV);

            // Do specific Android TV stuff if required as this is just user info setting initialisation from here.

        }
        else
        {
            Debug.Log("Android TV is not detected, this is from Android OS part from set link Data  =  " + currentYipliConfig.isDeviceAndroidTV);

            currentYipliConfig.matInfo = new YipliMatInfo(currentYipliConfig.currentMatID, currentYipliConfig.currentMatDetails.Child("mac-address").Value.ToString());
        }
#elif UNITY_IOS
        // mac-name
        //currentYipliConfig.matInfo = new YipliMatInfo(mId, mMac, mName);
        currentYipliConfig.matInfo = new YipliMatInfo(currentYipliConfig.currentMatID, currentYipliConfig.currentMatDetails.Child("mac-address").Value.ToString(), currentYipliConfig.currentMatDetails.Child("mac-name").Value.ToString() ?? LibConsts.MatTempAdvertisingNameOnlyForNonIOS);
#endif

        // logs only
        Debug.Log("Deep link : Found intents : " + currentYipliConfig.userId + ", " + currentYipliConfig.pId + ", " + pDOB + ", " + pHt + ", " + pWt + ", " + pName + ", " + mId + ", " + mMac + "," + mName + "," + pPicUrl);

        FetchUserAndInitializePlayerEnvironment();
    }

    private void TurnOffAllPanels()
    {
        phoneHolderInfo.SetActive(false);
        switchPlayerPanel.SetActive(false);
        playerSelectionPanel.SetActive(false);
        newMatInputController.HideMainMat();
        //onlyOnePlayerPanel.SetActive(false);
        Minimum2PlayersPanel.SetActive(false);
        //zeroPlayersPanel.SetActive(false);
        noNetworkPanel.SetActive(false);
        GuestUserPanel.SetActive(false);
        LaunchFromYipliAppPanel.SetActive(false);
        LoadingPanel.SetActive(false);
        //GameVersionUpdatePanel.SetActive(false);
        TutorialPanel.SetActive(false);

        //newUIManager.TurnOffMainCommonButton();
    }

    private void TurnOffAllPanelsExceptLoading()
    {
        phoneHolderInfo.SetActive(false);
        switchPlayerPanel.SetActive(false);
        playerSelectionPanel.SetActive(false);
        newMatInputController.HideMainMat();
        //onlyOnePlayerPanel.SetActive(false);
        Minimum2PlayersPanel.SetActive(false);
        //zeroPlayersPanel.SetActive(false);
        noNetworkPanel.SetActive(false);
        GuestUserPanel.SetActive(false);
        LaunchFromYipliAppPanel.SetActive(false);
        //GameVersionUpdatePanel.SetActive(false);
        TutorialPanel.SetActive(false);

        //newUIManager.TurnOffMainCommonButton();
    }


    private void FetchUserDetailsForAndroidAndIOS()
    {
        try
        {
            Debug.Log("Deep link : FetchUserDetails : " + currentYipliConfig.playerInfo + " : " + currentYipliConfig.matInfo);
#if UNITY_ANDROID
            if (currentYipliConfig.userId == null || currentYipliConfig.userId == "")
            {
                Debug.Log("Deep link : Reading intents as currentyipliConfig is empty");
                //ReadAndroidIntents();

                Debug.Log("Calling NoUserFoundInGameFlow()");
                //This code block will be called when the game App is not launched from the Yipli app even once.
                NoUserFoundInGameFlow();
            }
            else
            {
                Debug.Log("Deep link : no need to read intents as currentYipliConfig has data : " + currentYipliConfig.userId);
            }
#elif UNITY_IOS
            /*
            currentYipliConfig.userId = "lC4qqZCFEaMogYswKjd0ObE6nD43"; // vismay
            currentYipliConfig.playerInfo = new YipliPlayerInfo("-MSX--0uyqI7KgKmNOIY", "Nasha Mukti kendra", "07-01-1990", "172", "64", "-MSX--0uyqI7KgKmNOIY.jpg"); // vismay user
            currentYipliConfig.matInfo = new YipliMatInfo("-MUMyYuLTeqXB_K7RT_L", "A4:DA:32:4F:C2:54");
            */

#endif
        }
        catch (System.Exception exp)// handling of game directing opening, without yipli app
        {
            Debug.Log("Deep link : Exception occured in GetIntent!!!");
            Debug.Log(exp.Message);

            currentYipliConfig.userId = null;
            currentYipliConfig.playerInfo = null;
            currentYipliConfig.matInfo = null;
        }
    }

    private void FetchUserDetailsForWindowsAndEditor()
    {
#if UNITY_STANDALONE_WIN
        ReadFromWindowsFile();

        currentYipliConfig.pId = FileReadWrite.CurrentPlayerID;
        currentYipliConfig.matInfo = null;

        SetLinkData();
#endif

#if UNITY_EDITOR // uncoment following lines to test in editor. only one user id uncomment.
        // currentYipliConfig.userId = "F9zyHSRJUCb0Ctc15F9xkLFSH5f1"; // saurabh
        //currentYipliConfig.userId = "lC4qqZCFEaMogYswKjd0ObE6nD43"; // vismay
        //currentYipliConfig.pId = "-MSX--0uyqI7KgKmNOIY"; // vismay player
        // currentYipliConfig.userId = "rKE4pP03qwdZtRsIw0QjrYpYyXm1"; // vismay test only for vismay
        // currentYipliConfig.pId = "-MbGAzHTOONlQefgKklU"; // vismay player // smae above comment
        //currentYipliConfig.playerInfo = new YipliPlayerInfo("-MSX--0uyqI7KgKmNOIY", "Nasha Mukti kendra", "07-01-1990", "172", "64", "-MSX--0uyqI7KgKmNOIY.jpg", 1); // vismay user
        //currentYipliConfig.matInfo = new YipliMatInfo("-MUMyYuLTeqXB_K7RT_L", "A4:DA:32:4F:C2:54", "YIPLI");

        //SetLinkData();

        //GetAllMats();

        FetchUserAndInitializePlayerEnvironment();
#endif
    }

    private void SetOnlyMatPlayMode()
    {
        if (firebaseDBListenersAndHandlers.GetGameInfoQueryStatus() != QueryStatus.Completed) return;

        if (currentYipliConfig.gameInventoryInfo.onlyMatPlayMode.Contains("a") && Application.platform == RuntimePlatform.Android)
        {
            //Debug.LogError("Executing a");
            currentYipliConfig.onlyMatPlayMode = false;
            currentYipliConfig.allowMainGameSceneToLoad = true;
        }
        else if (currentYipliConfig.gameInventoryInfo.onlyMatPlayMode.Contains("s") && Application.platform == RuntimePlatform.Android && currentYipliConfig.isDeviceAndroidTV)
        {
            //Debug.LogError("Executing atv");
            currentYipliConfig.onlyMatPlayMode = false;
            currentYipliConfig.allowMainGameSceneToLoad = true;
        }
        else if (currentYipliConfig.gameInventoryInfo.onlyMatPlayMode.Contains("i") && Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //Debug.LogError("Executing i");
            currentYipliConfig.onlyMatPlayMode = false;
            currentYipliConfig.allowMainGameSceneToLoad = true;
        }
        else if (currentYipliConfig.gameInventoryInfo.onlyMatPlayMode.Contains("w") && Application.platform == RuntimePlatform.WindowsPlayer)
        {
            // for testing in editor (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            //Debug.LogError("Executing w");
            currentYipliConfig.onlyMatPlayMode = false;
            currentYipliConfig.allowMainGameSceneToLoad = true;
        }
        // testing purpose only
        // else if (currentYipliConfig.gameInventoryInfo.onlyMatPlayMode.Contains("a") && Application.platform == RuntimePlatform.WindowsEditor)
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            currentYipliConfig.onlyMatPlayMode = false;
            currentYipliConfig.allowMainGameSceneToLoad = true;
        }
        else
        {
            // defalt value reset
            //Debug.LogError("Executing final else");
            currentYipliConfig.onlyMatPlayMode = true;
        }

        onlyMatPlayModeIsSet = true;
    }

    private void ManageChangePlayer()
    {
        if (psFlowForCPcalled) return;

        // change player flow
        if (currentYipliConfig.bIsChangePlayerCalled && !psFlowForCPcalled)
        {
            psFlowForCPcalled = true;
            PlayerSelectionFlow();
            return;
        }
    }

    private void ManageDataMaintanence()
    {
        if (currentYipliConfig.bIsChangePlayerCalled) return;

        // game info status check
        if (firebaseDBListenersAndHandlers.GetGameInfoQueryStatus() != QueryStatus.Completed) return;

        //SetOnlyMatPlayMode();

        //Setting User Id in the scriptable Object
        if (!userIDInitiated)
        {
            InitUserId();
        }

        //First get all the players
        if (firebaseDBListenersAndHandlers.GetPlayersQueryStatus() != QueryStatus.Completed) return;

        //Todo: use this function in update if change player is not called
        if (!defaultPlayerIDInitiated)
        {
            InitDefaultPlayer();
        }

        // wait for only matplay mode to be set
        if (!onlyMatPlayModeIsSet) return;

        Debug.LogError("stut : only mat play mode : " + currentYipliConfig.onlyMatPlayMode);

        // only once start the connection flow
        if (!matConnectionStarted && currentYipliConfig.onlyMatPlayMode)
        {
            matConnectionStarted = true;
            matSelectionScript.MatConnectionFlow();
        }

        // no need to execute further if mat connection is not available
        if (currentYipliConfig.onlyMatPlayMode)
        {
            if (!YipliHelper.GetMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase)) return;
        }

        //Special handling in case of Multiplayer games
        if (currentYipliConfig.gameType == GameType.MULTIPLAYER_GAMING)
        {
            // Check if atleast 2 players are available for playing the multiplayer game
            if (currentYipliConfig.allPlayersInfo.Count < 2)
            {
                //Set active a panel to handle atleast 2 players should be there to play
                TurnOffAllPanels();
                Minimum2PlayersPanel.SetActive(true);
                //newUIManager.UpdateButtonDisplay(Minimum2PlayersPanel.tag);
            }
            else
            {
                LoadingPanel.SetActive(false);
                //Skip player selection as it will be handled by game side, directly go to the mat selection flow
                // TODO : this needs to be updated to direct game scene launch. No need to go through entire connection process
                //matSelectionScript.MatConnectionFlow();
                matSelectionScript.LoadMainGameSceneIfMatIsConnected();
            }
        }
        else
        {
            if (currentYipliConfig.playerInfo != null && currentYipliConfig.playerInfo.isMatTutDone == 0 && currentYipliConfig.onlyMatPlayMode)
            {
                LoadingPanel.SetActive(false);
                //Debug.LogError("Retake Tutorial : next line is playdevice specific tutorial");
                playDeviceSpecificMatTutorial();
            }
            else
            {
                SwitchPlayerFlow();
            }
        }
    }

    private IEnumerator InitializeAndStartPlayerSelection()
    {
        //Debug.LogError("Retake Tutorial : InitializeAndStartPlayerSelection start, next is GetGameInfo query status while loop");

        // game info status check
        while (firebaseDBListenersAndHandlers.GetGameInfoQueryStatus() != QueryStatus.Completed)
        {
            //Debug.LogError("Retake Tutorial : waiting for GetGameInfo to finish");
            yield return new WaitForSecondsRealtime(0.1f);
        }

        //Setting User Id in the scriptable Object
        InitUserId();

        //First get all the players
        while (firebaseDBListenersAndHandlers.GetPlayersQueryStatus() != QueryStatus.Completed)
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }

        //Todo: Revisit this and restructure.
        if (!currentYipliConfig.bIsChangePlayerCalled)
        {
            //Setting default Player in the scriptable Object
            InitDefaultPlayer();
        }

        //Debug.LogError("Retake Tutorial : initdefault player is done next is to set mat info");

#if UNITY_ANDROID
        if (!currentYipliConfig.isDeviceAndroidTV)
        {
            while (currentYipliConfig.matInfo == null)
            {
                //Debug.Log("Waiting until currentYipliConfig.matInfo setup is finished");
                yield return new WaitForSecondsRealtime(0.1f);
            }

            //Debug.Log("Wait is over as currentYipliConfig.matInfo setup is finished");
        }
#endif

        //Mat connection would be required for Mat tutorials and Gamelib Navigation
        if (!YipliHelper.GetMatConnectionStatus().Equals("Connected", StringComparison.OrdinalIgnoreCase))
        {
            matSelectionScript.EstablishMatConnection();
        }

        //Check if current player has completed the Mat tutorial or not.
        //GetPlayersMatTutorialCompletionStatus();

        //Debug.LogError("Retake Tutorial : from else before all if");

        //Special handling in case of Multiplayer games
        if (currentYipliConfig.gameType == GameType.MULTIPLAYER_GAMING)
        {
            //Debug.LogError("Retake Tutorial : from multiplayer gaming if");

            // Check if atleast 2 players are available for playing the multiplayer game
            if (currentYipliConfig.allPlayersInfo.Count < 2)
            {
                //Set active a panel to handle atleast 2 players should be there to play
                TurnOffAllPanels();
                Minimum2PlayersPanel.SetActive(true);
                //newUIManager.UpdateButtonDisplay(Minimum2PlayersPanel.tag);
            }
            else
            {
                LoadingPanel.SetActive(false);
                //Skip player selection as it will be handled by game side, directly go to the mat selection flow
                //matSelectionScript.MatConnectionFlow();
                matSelectionScript.LoadMainGameSceneIfMatIsConnected();
            }
        }
        else if (currentYipliConfig.bIsChangePlayerCalled == true)
        {
            //Debug.LogError("Retake Tutorial : from change player if");
            //currentYipliConfig.bIsChangePlayerCalled = false;

            /*
            if (currentYipliConfig.allPlayersInfo.Count > 1)
            {
                PlayerSelectionFlow();
            }
            else // If No then throw a new panel to tell the Gamer that there is only 1 player currently
            {
                TurnOffAllPanels();
                onlyOnePlayerPanel.SetActive(true);
            }
            */

            PlayerSelectionFlow();
        }
        else
        {
            if (currentYipliConfig.playerInfo == null || currentYipliConfig.playerInfo.isMatTutDone == 0 || currentYipliConfig.bIsRetakeTutorialFlagActivated)
            {
                while (!YipliHelper.GetMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase))
                {
                    yield return new WaitForSecondsRealtime(0.1f);
                }

                LoadingPanel.SetActive(false);
                //Debug.LogError("Retake Tutorial : next line is playdevice specific tutorial");
                playDeviceSpecificMatTutorial();
            }
            else
            {
                SwitchPlayerFlow();
            }
        }
    }

    public void retryUserCheck()
    {
        GuestUserPanel.SetActive(false);
        //newUIManager.TurnOffMainCommonButton();
        FetchUserAndInitializePlayerEnvironment();
    }

    public void retryPlayersCheck()
    {
        Minimum2PlayersPanel.SetActive(false);
        //newUIManager.TurnOffMainCommonButton();
        //StartCoroutine(InitializeAndStartPlayerSelection());
    }

    public void SwitchPlayerFlow()//Call this for every StartGame()/Game Session
    {
        Debug.Log("Checking current player.");

        TurnOffAllPanels();
        LoadingPanel.SetActive(true);

        currentYipliConfig.allowMainGameSceneToLoad = true;

        if (currentYipliConfig.playerInfo != null)
        {
            //This means we have the default Player info from backend.
            //In this case we need to call the player change screen and not the player selection screen
            Debug.Log("Found current player : " + currentYipliConfig.playerInfo.playerName);

            //StartCoroutine(ImageUploadAndPlayerUIInit());

            //Since default player is there, directly go to the mat selection flow
            //matSelectionScript.MatConnectionFlow();
            matSelectionScript.LoadMainGameSceneIfMatIsConnected();
        }
        else //Current player not found in Db.
        {
            //Force to switch player as no default player found.
            OnSwitchPlayerPress(true);
        }
    }

    private IEnumerator ImageUploadAndPlayerUIInit()
    {
        //Activate the PlayerName and Image display object
        if (currentYipliConfig.gameType != GameType.MULTIPLAYER_GAMING)
        {
            //if (!bIsProfilePicLoaded)
            //    loadProfilePicAsync(profilePicImage, currentYipliConfig.playerInfo.profilePicUrl);
            //playerNameText.text = "Hi, " + currentYipliConfig.playerInfo.playerName;
            //playerNameText.gameObject.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(0.00001f);
    }

    public void PlayerSelectionFlow()
    {
        Debug.Log("In Player selection flow.");

        // first of all destroy all PlayerButton prefabs. This is required to remove stale prefabs.
        //if (generatedObjects.Count > 0)
        //{
        //    Debug.Log("Destroying all the stale --PlayerName-- prefabs, before spawning new ones.");
        //    foreach (var obj1 in generatedObjects)
        //    {
        //        Destroy(obj1);
        //    }
        //}

        if (currentYipliConfig.allPlayersInfo != null && currentYipliConfig.allPlayersInfo.Count != 0) //Atleast 1 player found for the corresponding userId
        {
            Debug.Log("Player/s found from firebase : " + currentYipliConfig.allPlayersInfo.Count);

            //List<InfiniteWheel.InfiniteWheelItem> allButtons = new List<InfiniteWheel.InfiniteWheelItem>();

            try
            {
                Quaternion spawnrotation = Quaternion.identity;
                Vector3 playerTilePosition = PlayersContainer.transform.localPosition;

                /*
                for (int i = 0; i < currentYipliConfig.allPlayersInfo.Count; i++)
                {
                    GameObject PlayerButton = Instantiate(PlayerButtonPrefab, playerTilePosition, spawnrotation) as GameObject;
                    generatedObjects.Add(PlayerButton);
                    PlayerButton.name = currentYipliConfig.allPlayersInfo[i].playerName;
                    PlayerButton.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentYipliConfig.allPlayersInfo[i].playerName;
                    PlayerButton.transform.SetParent(PlayersContainer.transform, false); // old code
                    //PlayerButton.transform.SetParent(PlayersMenuObject.transform, false);
                    PlayerButton.transform.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(SelectPlayer);

                    //allButtons.Add(PlayerButton.GetComponent<InfiniteWheel.InfiniteWheelItem>());
                }
                */
            }
            catch (Exception exp)
            {
                Debug.Log("Exception in Adding player : " + exp.Message);
                //Application.Quit();
            }
            TurnOffAllPanels();

            // set all button list for wheel scrolling
            // PlayersMenuObject.GetComponent<InfiniteWheel.InfiniteWheelController>().SetitemsList(allButtons);

            playerSelectionPanel.SetActive(true);
            newMatInputController.DisplayMainMat();
            newMatInputController.SetMatPlayerSelectionPosition();
            newMatInputController.KeepLeftNadRightButtonColorToOriginal();
            newMatInputController.DisplayChevrons();
            newMatInputController.DisplayLegs();
            newMatInputController.HideTextButtons();
        }
        else
        {
            Debug.Log("No player found from firebase.");
            TurnOffAllPanels();
            //zeroPlayersPanel.SetActive(true);
            // TODO : if no players are found
        }
    }

    public void SelectPlayer()
    {
        playerSelectionPanel.SetActive(false);
        newMatInputController.HideMainMat();
        //FindObjectOfType<YipliAudioManager>().Play("ButtonClick");
        //PlayerName = EventSystem.current.currentSelectedGameObject != null ? EventSystem.current.currentSelectedGameObject.name : FindObjectOfType<MatInputController>().GetCurrentButton().name;

        PlayerName = matInputController.CurrentPlayerName;
        //Debug.LogError("switchPlayer matInputController.CurrentPlayerName :  " + matInputController.CurrentPlayerName);

        // first of all destroy all PlayerButton prefabs. This is required to remove stale prefabs.
        foreach (var obj1 in generatedObjects)
        {
            Destroy(obj1);
        }
        //Debug.LogError("switchPlayer Selected :  " + PlayerName);

        //Changing the currentSelected player in the Scriptable object
        //No Making this player persist in the device. This will be done on continue press.
        currentYipliConfig.playerInfo = GetPlayerInfoFromPlayerName(PlayerName);
        //Debug.LogError("switchPlayer configured : " + currentYipliConfig.playerInfo);

        //Save the player to device
        //UserDataPersistence.SavePlayerToDevice(currentYipliConfig.playerInfo);

        //Trigger the GetGameData for new player.
        DefaultPlayerChanged();
        //Debug.LogError("switchPlayer GameData is triggered");

        //StartCoroutine(ImageUploadAndPlayerUIInit());

        TurnOffAllPanels();
        //Debug.LogError("switchPlayer All panels are off : " + learnMatControlIsClicked + " " +  currentYipliConfig.playerInfo.isMatTutDone);

        /* if else conditions to start tutorial if player has not done
        if (learnMatControlIsClicked || currentYipliConfig.playerInfo.isMatTutDone == 0)
        {
            Debug.LogError("switchPlayer from 1st if : " + learnMatControlIsClicked + " " +  currentYipliConfig.playerInfo.isMatTutDone);
            if (learnMatControlIsClicked) {

                Debug.LogError("switchPlayer from 2nd if : " + learnMatControlIsClicked + " " +  currentYipliConfig.playerInfo.isMatTutDone);

                learnMatControlIsClicked = false;

                //learnMatControlText.text = "Learn MAT Controls";
                //learnMatControlText.fontSize = 14;

                playDeviceSpecificMatTutorial();
            }

            // write tut flag to backend here or start the tutorial
            if (isTutorialDoneWithoutPlayerInfo)
            {
                Debug.LogError("switchPlayer from 3rd if : " + learnMatControlIsClicked + " " +  currentYipliConfig.playerInfo.isMatTutDone);

                FirebaseDBHandler.UpdateTutStatusData(currentYipliConfig.userId, currentYipliConfig.playerInfo.playerId, 1);
                //UserDataPersistence.SavePropertyValue("player-tutDone", 1.ToString());
                matInputController.IsThisSwitchPlayerPanel = true;

                matInputController.UpdateSwitchPlayerPanelPlayerObject();

                //switchPlayerPanelText.text = "You have selected " + matInputController.CurrentPlayerName + " as player.";

                //switchPlayerPanel.SetActive(true);
                OnContinuePress();
            }
            else
            {
                Debug.LogError("switchPlayer from 3rd else : " + learnMatControlIsClicked + " " +  currentYipliConfig.playerInfo.isMatTutDone);
                if (currentYipliConfig.bIsChangePlayerCalled) {
                    currentYipliConfig.bIsChangePlayerCalled = false;
                }

                Debug.LogError("switchPlayer from 3rd else deviceSpecific tutorial will be triggered from next line : " + learnMatControlIsClicked + " " +  currentYipliConfig.playerInfo.isMatTutDone);
                playDeviceSpecificMatTutorial();
            }
        }
        else
        {
            Debug.LogError("switchPlayer from 1st else : " + learnMatControlIsClicked + " " +  currentYipliConfig.playerInfo.isMatTutDone);

            matInputController.IsThisSwitchPlayerPanel = true;

            matInputController.UpdateSwitchPlayerPanelPlayerObject();

            //switchPlayerPanelText.text = "You have selected " + matInputController.CurrentPlayerName + " as player.";

            newMatInputController.DisplayMainMat();
            newMatInputController.HideChevrons();
            newMatInputController.DisplayMatForSwitchPlayerPanel();

            Debug.LogError("switchPlayer from 1st else next line switch player panel active true : " + learnMatControlIsClicked + " " +  currentYipliConfig.playerInfo.isMatTutDone);
            //switchPlayerPanel.SetActive(true);

            // only until switch player panel gets desiged
            OnContinuePress();
        }
        */

        matInputController.IsThisSwitchPlayerPanel = true;

        matInputController.UpdateSwitchPlayerPanelPlayerObject();

        //switchPlayerPanelText.text = "You have selected " + matInputController.CurrentPlayerName + " as player.";

        newMatInputController.DisplayMainMat();
        newMatInputController.HideChevrons();
        newMatInputController.DisplayMatForSwitchPlayerPanel();

        if (currentYipliConfig.bIsChangePlayerCalled)
        {
            // currentYipliConfig.bIsChangePlayerCalled = false;
            psFlowForCPcalled = false;
        }

        //switchPlayerPanel.SetActive(true);

        // only until switch player panel gets desiged
        OnContinuePress();
    }

    private YipliPlayerInfo GetPlayerInfoFromPlayerName(string playerName)
    {
        if (currentYipliConfig.allPlayersInfo.Count > 0)
        {
            foreach (YipliPlayerInfo player in currentYipliConfig.allPlayersInfo)
            {
                Debug.Log("Found player : " + player.playerName);
                if (player.playerName == playerName)
                {
                    Debug.Log("Found player : " + player.playerName);
                    return player;
                }
            }
        }
        else
        {
            Debug.Log("No Players found.");
        }
        return null;
    }

    /* Player selection is done. 
     * This function takes the flow to Mat connection */
    public void OnContinuePress()
    {
        Debug.Log("Continue Pressed.");
        TurnOffAllPanels();
        //matSelectionScript.MatConnectionFlow();
        matSelectionScript.LoadMainGameSceneIfMatIsConnected();
    }

    public void OnJumpOnMat()
    {
        // activate mat tutorial here
        allowPhoneHolderAudioPlay = false;

        //TurnOffAllPanels();

        //TutorialPanel.SetActive(true);
        //TutorialPanel.GetComponent<TutorialManagerGL>().ActivateTutorial();
        //secondTutorialManager.StartMatTutorial();

        //Debug.LogError("Next line is scene manager to load scene");
        SceneManager.LoadScene("gameLibTutorial");

        /*
        currentYipliConfig.bIsMatIntroDone = true;
        phoneHolderInfo.SetActive(false);
        StopCoroutine(ChangeTextMessage());
        SwitchPlayerFlow();
        */
    }

    // This function is attached to Continue button on Tutorial panel. This will be the end of tutorial.
    public void OnTutorialContinuePress()
    {
        //if (currentYipliConfig.bIsRetakeTutorialFlagActivated)
        //{
        //    currentYipliConfig.bIsRetakeTutorialFlagActivated = false;
        //    //matSelectionScript.MatConnectionFlow();
        //    matSelectionScript.LoadMainGameSceneIfMatIsConnected();
        //    return;
        //}


        if (currentYipliConfig.playerInfo != null)
        {
            if (currentYipliConfig.playerInfo.isMatTutDone == 0)
            {
                FirebaseDBHandler.UpdateTutStatusData(currentYipliConfig.userId, currentYipliConfig.playerInfo.playerId, 1);
                //UserDataPersistence.SavePropertyValue("player-tutDone", 1.ToString());
            }
        }
        else
        {
            // set tut flag after player selection is done in backend
            isTutorialDoneWithoutPlayerInfo = true;
        }

        phoneHolderInfo.SetActive(false);

#if UNITY_ANDROID || UNITY_IOS
        //StopCoroutine(ChangeTextMessageAndoridPhone());
        if (currentYipliConfig.isDeviceAndroidTV)
        {
            PlayTVStickTutStartAnimation();
        }
        else
        {
            PlayPhoneTutStartAnimation();
        }
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        //StopCoroutine(ChangeTextMessageWindowsPC());
        PlayPCTutStartAnimation();
#endif

        SwitchPlayerFlow();
    }

    public void OnSwitchPlayerPress(bool isInternalCall = false /* If called internally that means no default player found.*/)
    {
        TurnOffAllPanels();

        if (!YipliHelper.checkInternetConnection())
        {
            //If default player also not available, then show no players panel.
            if (currentYipliConfig.playerInfo != null)
            {
                //noNetworkPanelText.text = "No active network connection. Cannot switch player. Press continue to play as " + defaultPlayer.playerName;
                //newUIManager.UpdateButtonDisplay(noNetworkPanel.tag);
                noNetworkPanel.SetActive(true);
            }
            /*
            else
            {
                //Default player is not there.
                //zeroPlayersPanel.SetActive(true);
            }
            */
        }
        else//Active Network connection is available
        {
            //In case of internall call
            //This is to handle if the default player isn't set
            if (isInternalCall)
            {
                //This means no default player is present.Show all the players for selection.
                //Whichever gets selected, will become the default player later.
                //First check if the players count under userId is more than 0?
                if (currentYipliConfig.allPlayersInfo != null && currentYipliConfig.allPlayersInfo.Count > 0)
                {
                    Debug.Log("Calling the PlayerSelectionFlow");
                    PlayerSelectionFlow();
                }
                /*
                else // If No then throw a new panel to tell the Gamer that there is no player found
                {
                    TurnOffAllPanels();
                    zeroPlayersPanel.SetActive(true);
                }
                */
            }
            else
            {
                //First check if the players count under userId is more than 1 ?
                //if (currentYipliConfig.allPlayersInfo != null && currentYipliConfig.allPlayersInfo.Count > 1)
                if (currentYipliConfig.allPlayersInfo != null && currentYipliConfig.allPlayersInfo.Count > 0)
                {
                    PlayerSelectionFlow();
                }
                /*
                else // If No then throw a new panel to tell the Gamer that there is only 1 player currently
                {
                    TurnOffAllPanels();
                    onlyOnePlayerPanel.SetActive(true);
                }
                */
            }
        }
    }

    public void OnGoToYipliPress()
    {
        YipliHelper.GoToYipli(ProductMessages.noPlayerAdded);
    }

    public void IAlreadyHaveMat()
    {
        YipliHelper.GoToYipli(ProductMessages.relaunchGame, currentYipliConfig.gameId);
    }

    public void OnBackButtonPress()
    {
        TurnOffAllPanels();
        switchPlayerPanel.SetActive(true);
    }

    //public void setBluetoothEnabled()
    //{
    //    using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
    //    {
    //        try
    //        {
    //            using (var BluetoothManager = activity.Call<AndroidJavaObject>("getSystemService", "bluetooth"))
    //            {
    //                using (var BluetoothAdapter = BluetoothManager.Call<AndroidJavaObject>("getAdapter"))
    //                {
    //                    BluetoothAdapter.Call("enable");
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Debug.Log(e);
    //            Debug.Log("could not enable the bluetooth automatically");
    //        }
    //    }
    //}

    async private void loadProfilePicAsync(Image gameObj, string profilePicUrl)
    {
        try
        {
            if (profilePicUrl == null || profilePicUrl == "" || gameObj == null)
            {
                Debug.Log("Something went wrong. Returning.");
                //Set the profile pic to a default one.
                gameObj.sprite = defaultProfilePicSprite;
            }
            else
            {
                // Create local filesystem URL
                string onDeviceProfilePicPath = Application.persistentDataPath + "/" + profilePicUrl;
                Sprite downloadedSprite = await FirebaseDBHandler.GetImageAsync(profilePicUrl, onDeviceProfilePicPath);
                if (downloadedSprite != null)
                {
                    gameObj.sprite = downloadedSprite;
                    bIsProfilePicLoaded = true;
                }
                else
                {
                    //Actual profile pic in the backend
                    gameObj.sprite = defaultProfilePicSprite;
                }
            }
            bIsProfilePicLoaded = false;
        }
        catch (Exception e)
        {
            Debug.LogError("Loading of profile pic failed : " + e.Message);
        }
    }

#if UNITY_STANDALONE_WIN
    private void ReadFromWindowsFile()
    {
        currentYipliConfig.userId = FileReadWrite.ReadFromFile();
        //currentYipliConfig.playerInfo = null;
    }
#endif

    // retake tutorial button function
    public void RetakeMatTutorialButton()
    {
        if (currentYipliConfig.playerInfo == null)
        {

            learnMatControlIsClicked = true;

            //learnMatControlText.text = "Select Player to Continue";
            //learnMatControlText.fontSize = 12;

            return;
        }

        //playDeviceSpecificMatTutorial();
    }

    // gamelib quit button
    public void QuitFromGameLibButton()
    {
        Application.Quit();
    }

    // no internet check coroutine
    private IEnumerator CheckNoInternetConnection()
    {
        //Todo : remove this and shift it to FirebaseHandler Listner function itself.
        //Set default valuye of currentYipliConfig.bIsInternetConnected to true, to avoid the Internet panel from coming for 1st fraction of second
        yield return new WaitForSecondsRealtime(8f);

        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);

            if (YipliHelper.checkInternetConnection())
            {
                //newUIManager.TurnOffMainCommonButton();
                noNetworkPanel.SetActive(false);

                //Time.timeScale = 0f; // pause everything
            }
            else
            {
                //newUIManager.UpdateButtonDisplay(noNetworkPanel.tag);
                noNetworkPanel.SetActive(true);

                //Time.timeScale = 1f; // resume everything
            }
            /*
            noNetworkPanel.SetActive(false);

            yield return new WaitForSecondsRealtime(1f);

            if (!currentYipliConfig.bIsInternetConnected)
            {
                yield return new WaitForSecondsRealtime(2f);

                if (currentYipliConfig.bIsInternetConnected) continue;

                //noNetworkPanelText.text = "No Internet connection.\nGame will resume when network is available."; text is already set or set that in inspector
                //newUIManager.UpdateButtonDisplay(noNetworkPanel.tag);
                noNetworkPanel.SetActive(true);
            }
            */
        }
    }

    private void UpdateGameAndDriverVersionText()
    {
        gameAndDriverVersionText.text = Application.version;
    }

    // redirect to yipli id userid is notfound after 10 seconds
    private IEnumerator RelaunchgameFromYipliApp()
    {
        //if (!currentYipliConfig.onlyMatPlayMode)
        //{
        //    yield break;
        //}

#if UNITY_EDITOR
        if (currentYipliConfig.userId != null)
        {
            yield break;
        }
#endif

        int totalTime = 0;

        while (totalTime < 10)
        {

            if (firebaseDBListenersAndHandlers.dynamicLinkIsReceived)
            {
                yield break;
            }

            yield return new WaitForSecondsRealtime(1f);
            totalTime++;
        }

        if (!firebaseDBListenersAndHandlers.dynamicLinkIsReceived)
        {
            if (YipliHelper.checkInternetConnection())
            {
                NoUserFoundInGameFlow();
            }
            else
            {
                //newUIManager.UpdateButtonDisplay(noNetworkPanel.tag);
                noNetworkPanel.SetActive(true);
            }
        }
    }

    // destroy all generated objects
    public void DestroyPlayerSelectionButtons()
    {
        foreach (var obj1 in generatedObjects)
        {
            Destroy(obj1);
        }
    }

    // Manage Retries Button On Different Panels
    private void ManageRetriesButtonOnDifferentPanel()
    {
        /*
        if (onlyOnePlayerPanel.activeSelf && currentYipliConfig.allPlayersInfo.Count > 1) {
            onlyOnePlayerPanelRetryButton.gameObject.SetActive(true);
        }
        else {
            onlyOnePlayerPanelRetryButton.gameObject.SetActive(false);
        }
        */
    }

    // tryAgain function for no internet panel for new UI
    public void TryAgainInternetConnection()
    {
        if (YipliHelper.checkInternetConnection())
        {
            noNetworkPanel.SetActive(false);
        }
        else
        {
            //newUIManager.UpdateButtonDisplay(noNetworkPanel.tag);
            noNetworkPanel.SetActive(true);
        }
    }
}