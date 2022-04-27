using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_STANDALONE_WIN
using yipli.Windows;
#endif


public class MatSelection : MonoBehaviour
{
    private const int MaxBleCheckCount = 20;

    //public TextMeshProUGUI noMatText;
    //public TextMeshProUGUI bleSuccessMsg;
    //public TextMeshProUGUI passwordErrorText;

    //public InputField inputPassword;
    public GameObject loadingPanel;
    //public GameObject BluetoothSuccessPanel;

    public GameObject NoMatPanel;
    //public GameObject SkipMatButton;
    //public GameObject secretEntryPanel;
    public YipliConfig currentYipliConfig;
    private string connectionState;
    private int checkMatStatusCount;

    //public GameObject tick;

    //public GameObject troubleshootButton;
    //public GameObject yipliHomeButton;
    //public GameObject retryButton;
    //public GameObject installDriverButton;

    [Header("Required script objects")]
    [SerializeField] private NewUIManager newUIManager = null;
    [SerializeField] private NewMatInputController newMatInputController = null;

    int retriesDone = 0;
    const int totalMatConnectionRetriesOnRecheck = 2;

    private bool autoSkipMatConnection;

    private bool bIsGameMainSceneLoading = false;

    private bool bIsRetryConnectionCalled = false;

    private bool bIsMatFlowInitialized = false;
    private void Start()
    {
        //Initialize
        bIsGameMainSceneLoading = false;

        //if (currentYipliConfig.onlyMatPlayMode == false)
        //{
        //    // Make the Skip button visible
        //    //SkipMatButton.SetActive(true);
        //}
        //else
        //{
        //    //SkipMatButton.SetActive(false);
        //}

        // make troubleshoot button disable by default
        DisableTroubleshootButton();

        if (currentYipliConfig.bIsChangePlayerCalled)
        {
            StartCoroutine(MatConnectionCheck());
        }
    }

    // This function is to be called before Mat tutorial.
    // Mat tutorial requires the mat connection to be established.
    public void EstablishMatConnection()
    {
        Debug.Log("Starting Mat connection flow");
        NoMatPanel.SetActive(false);
        newMatInputController.MakeSortLayerTen();
        //newUIManager.TurnOffMainCommonButton();

#if UNITY_ANDROID
        /*
        if (currentYipliConfig.matInfo == null && !currentYipliConfig.isDeviceAndroidTV)
        {
            Debug.Log("Filling te current mat Info from Device saved MAT");
            currentYipliConfig.matInfo = UserDataPersistence.GetSavedMat();
        }
        */

        if (currentYipliConfig.matInfo != null || currentYipliConfig.isDeviceAndroidTV)
        {
            //Debug.Log("Mac Address : " + currentYipliConfig.matInfo.macAddress);
            //Load Game scene if the mat is already connected.
            if (!InitBLE.getMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase))
            {
                StartCoroutine(ConnectMat());
            }
        }
        else //Current Mat not found in Db.
        {
            loadingPanel.SetActive(false);
            Debug.Log("No Mat found in cache.");
            //noMatText.text = ProductMessages.Err_mat_connection_android_phone_register;
            //newUIManager.UpdateButtonDisplay(NoMatPanel.tag);
            newMatInputController.MakeSortLayerZero();
            NoMatPanel.SetActive(true);
            FindObjectOfType<YipliAudioManager>().Play("BLE_failure");
        }
#elif UNITY_STANDALONE_WIN
        if (!InitBLE.getMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase))
        {
            StartCoroutine(ConnectMat());
        }
#elif UNITY_IOS
        if (!InitBLE.getMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase))
        {
            StartCoroutine(ConnectMat());
        }
#endif

        StartCoroutine(MatConnectionCheck());
    }

    // during gamelib scene processes keep checking for mat ble connection in android devices.
    private IEnumerator MatConnectionCheck()
    {
        if (!currentYipliConfig.onlyMatPlayMode)
        {
            yield break;
        }

#if UNITY_IOS
            yield return new WaitForSecondsRealtime(15f);
#else
        yield return new WaitForSecondsRealtime(5f);
#endif

        while (true)
        {
#if UNITY_IOS
                yield return new WaitForSecondsRealtime(1f);
#else
            yield return new WaitForSecondsRealtime(0.5f);
#endif

            if (!YipliHelper.GetMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase))
            {
                //newUIManager.UpdateButtonDisplay(NoMatPanel.tag);
                newMatInputController.MakeSortLayerZero();
                NoMatPanel.SetActive(true);

                //Time.timeScale = 0f; // pause everything
            }
            else
            {
                //Time.timeScale = 1f; // resume everything
            }
        }
    }

    public void MatConnectionFlow()
    {
        Debug.Log("Starting Mat connection flow");
        bIsMatFlowInitialized = true;
        bIsGameMainSceneLoading = false;
        NoMatPanel.SetActive(false);
        newMatInputController.MakeSortLayerTen();
        //newUIManager.TurnOffMainCommonButton();

#if UNITY_EDITOR
        if (!bIsGameMainSceneLoading)
            StartCoroutine(LoadMainGameScene());
#elif UNITY_ANDROID
        /*
        if (currentYipliConfig.matInfo == null && !currentYipliConfig.isDeviceAndroidTV)
        {
            Debug.Log("Filling te current mat Info from Device saved MAT");
            currentYipliConfig.matInfo = UserDataPersistence.GetSavedMat();
        }
        */

        if (currentYipliConfig.matInfo != null || currentYipliConfig.isDeviceAndroidTV)
        {
            if (currentYipliConfig.matInfo != null) {
                Debug.Log("Mac Address : " + currentYipliConfig.matInfo.macAddress);
            }
            //Load Game scene if the mat is already connected.
            if (!InitBLE.getMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase))
            {
                StartCoroutine(ConnectMat());
            }
        }
        else //Current Mat not found in Db.
        {
            loadingPanel.SetActive(false);
            Debug.Log("No Mat found in cache.");
            //noMatText.text = ProductMessages.Err_mat_connection_android_phone_register;
            //newUIManager.UpdateButtonDisplay(NoMatPanel.tag);
            newMatInputController.MakeSortLayerZero();
            NoMatPanel.SetActive(true);
            //newUIManager.TurnOffMainCommonButton();
            FindObjectOfType<YipliAudioManager>().Play("BLE_failure");
        }
#elif UNITY_STANDALONE_WIN
        if (!InitBLE.getMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase))
        {
            StartCoroutine(ConnectMat(true));
        }
#elif UNITY_IOS
        if (!InitBLE.getMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase))
        {
            StartCoroutine(ConnectMat(true));
        }
#endif
    }

    public void LoadMainGameSceneIfMatIsConnected()
    {
        if (!YipliHelper.GetMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase) || currentYipliConfig.onlyMatPlayMode)
        {
            StartCoroutine(LoadMainGameScene());
        }
        else
        {
            MatConnectionFlow();
        }
    }

    public void LoadMainGameSceneDirectly()
    {
        StartCoroutine(LoadMainGameScene());
    }

    public void ReCheckMatConnection()
    {
        //newUIManager.TurnOffMainCommonButton();

        Debug.Log("ReCheckMatConnection() called");
        if (bIsMatFlowInitialized)
            MatConnectionFlow();
        else
            EstablishMatConnection();

        retriesDone++;

        if (retriesDone > totalMatConnectionRetriesOnRecheck)
        {
            //EnableTroubleshootButton();// ask users if they wants to start it.
        }
        /*
        #if UNITY_STANDALONE_WIN
                retriesDone++;

                if (retriesDone > totalMatConnectionRetriesOnRecheck)
                {
                    //EnableTroubleshootButton();// ask users if they wants to start it.
                    TroubleshootButton();
                }
        #endif
        */
    }


    public void Update()
    {
        if (bIsMatFlowInitialized)
        {
            //LoadGameScene if mat connection is established
            if (InitBLE.getMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase))
            {
                if (true != bIsGameMainSceneLoading)
                    StartCoroutine(LoadMainGameScene());
            }
        }

        // panels with buttons
        // noInternetPanel, noMatPanel(Guest User panel), maintanencePanel, noMatConnectionPanel, phoneHolderTutorialPanel, minimum2Player
        // if (NoMatPanel.activeSelf) {
        //     newUIManager.TurnOnMainCommonButton();
        // } else {
        //     newUIManager.TurnOffMainCommonButton();
        // }
    }


    private IEnumerator ConnectMat(bool bIsReconnectMatNeeded = false)
    {
        int iTryCount = 0;

        //Initiate the connection with the mat.  
        try
        {
            // if (bIsReconnectMatNeeded)
            // {
            //     RetryMatConnectionOnPC();
            // }
            // else
            // {
            //     InitiateMatConnection();
            // }

            InitiateMatConnection();
        }
        catch (Exception e)
        {
            Debug.LogError("mat connection failed : " + e.Message);

            loadingPanel.SetActive(false);
            //newUIManager.UpdateButtonDisplay(NoMatPanel.tag);
            newMatInputController.MakeSortLayerZero();
            NoMatPanel.SetActive(true);
            yield break;
        }

        yield return new WaitForSecondsRealtime(0.1f);

        //Turn on the Mat Find Panel, and animate
        //loadingPanel.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Finding your mat..";
        loadingPanel.SetActive(true);//Show msg till mat connection is confirmed.

        while (!InitBLE.getMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase)
            && iTryCount < MaxBleCheckCount)
        {
#if UNITY_IOS
            yield return new WaitForSecondsRealtime(1f);
#else
            yield return new WaitForSecondsRealtime(0.25f);
#endif
            iTryCount++;
        }

        //Turn off the Mat Find Panel
        loadingPanel.SetActive(false);
        //loadingPanel.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Fetching player details...";

        if (!InitBLE.getMatConnectionStatus().Equals("connected", StringComparison.OrdinalIgnoreCase))
        {
            FindObjectOfType<YipliAudioManager>().Play("BLE_failure");
            Debug.Log("Mat not reachable.");

            //#if UNITY_ANDROID || UNITY_IOS
            //                //noMatText.text = ProductMessages.Err_mat_connection_mat_off;
            //#elif UNITY_STANDALONE_WIN && UNITY_EDITOR
            //            if (PortTestings.CheckAvailableComPorts() == 0)
            //            {
            //                noMatText.text = ProductMessages.Err_mat_connection_no_ports;
            //            }
            //            else
            //            {
            //                noMatText.text = ProductMessages.Err_mat_connection_mat_off;
            //            }

            //#endif
            //newUIManager.UpdateButtonDisplay(NoMatPanel.tag);
            newMatInputController.MakeSortLayerZero();
            NoMatPanel.SetActive(true);
        }
    }

    public void SkipMat()
    {
        NoMatPanel.SetActive(false);
        newMatInputController.MakeSortLayerTen();
        //newUIManager.TurnOffMainCommonButton();

        //passwordErrorText.text = "";
        //inputPassword.text = "";
        //secretEntryPanel.SetActive(true);
    }

    public void OnPlayPress()
    {
        //if (inputPassword.text == "123456")
        //{
        //    //load last Scene
        //    if (!bIsGameMainSceneLoading)
        //        StartCoroutine(LoadMainGameScene());
        //}
        //else
        //{
        //    FindObjectOfType<YipliAudioManager>().Play("BLE_failure"); inputPassword.text = "";
        //    //passwordErrorText.text = "Invalid pasword";
        //    Debug.Log("incorrect password");
        //}
    }

    IEnumerator LoadMainGameScene()
    {
        /*
        bIsGameMainSceneLoading = true;
        loadingPanel.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "launching game..";
        loadingPanel.SetActive(false);
        NoMatPanel.SetActive(false);
        newMatInputController.MakeSortLayerTen();
        newUIManager.TurnOffMainCommonButton();
        bleSuccessMsg.text = "Your YIPLI MAT is connected.";

        BluetoothSuccessPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);

        FindObjectOfType<YipliAudioManager>().Play("BLE_success");
        tick.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        */
        bIsGameMainSceneLoading = true;
        //loadingPanel.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "launching game..";
        loadingPanel.SetActive(true);

        if (currentYipliConfig.gameType != GameType.MULTIPLAYER_GAMING)
        {
            while (firebaseDBListenersAndHandlers.GetGameDataForCurrenPlayerQueryStatus() != QueryStatus.Completed)
            {
                Debug.Log("waiting to finish new player's game data");
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }


        //yield return null;
        StartCoroutine(LoadSceneAfterDisplayingDriverAndGameVersion());
    }

    IEnumerator LoadSceneAfterDisplayingDriverAndGameVersion()
    {
        //TODO : Comment following lines for production build and uncomment for display game and driver version
        //bleSuccessMsg.text = "FmDriver Version : " + YipliHelper.GetFMDriverVersion() + "\n Game Version : " + Application.version;
        //yield return new WaitForSeconds(1f);

        yield return null;

        if (currentYipliConfig.bIsChangePlayerCalled)
        {
            // this check has to be false for every game scene load.
            currentYipliConfig.bIsChangePlayerCalled = false;
            currentYipliConfig.allowMainGameSceneToLoad = true;
        }
        currentYipliConfig.allowMainGameSceneToLoad = true; //comment while not for testing purposes or demo puposes.
        //load last Scene
        Debug.LogError("Time to launch the scene : " + currentYipliConfig.allowMainGameSceneToLoad);
        if (currentYipliConfig.allowMainGameSceneToLoad)
        {

            SceneManager.LoadScene(currentYipliConfig.callbackLevel);
        }
    }

    public void OnBackPress()
    {
        //secretEntryPanel.SetActive(false);
        //newUIManager.UpdateButtonDisplay(NoMatPanel.tag);
        newMatInputController.MakeSortLayerZero();
        NoMatPanel.SetActive(true);
    }

    private void InitiateMatConnection()
    {
        //Initiate the connection with the mat.
#if UNITY_IOS
        // connection part for ios
        InitBLE.InitBLEFramework(currentYipliConfig.matInfo?.macAddress ?? "", 0, currentYipliConfig.matInfo?.matAdvertisingName ?? LibConsts.MatTempAdvertisingNameOnlyForNonIOS);
#elif UNITY_ANDROID
        InitBLE.InitBLEFramework(currentYipliConfig.matInfo?.macAddress ?? "", 0, currentYipliConfig.matInfo?.matAdvertisingName ?? LibConsts.MatTempAdvertisingNameOnlyForNonIOS, currentYipliConfig.isDeviceAndroidTV);
#else
        InitBLE.InitBLEFramework(currentYipliConfig.matInfo?.macAddress ?? "", 0);
#endif
    }

    private void RetryMatConnectionOnPC()
    {
        //Initiate the connection with the mat.
#if UNITY_IOS
        // connection part for ios
        InitBLE.InitBLEFramework(currentYipliConfig.matInfo?.macAddress ?? "", 0, currentYipliConfig.matInfo?.matAdvertisingName ?? LibConsts.MatTempAdvertisingNameOnlyForNonIOS);
#elif UNITY_ANDROID
        InitBLE.InitBLEFramework(currentYipliConfig.matInfo?.macAddress ?? "", 0, currentYipliConfig.matInfo?.matAdvertisingName ?? LibConsts.MatTempAdvertisingNameOnlyForNonIOS, currentYipliConfig.isDeviceAndroidTV);
#else
        InitBLE.InitBLEFramework(currentYipliConfig.matInfo?.macAddress ?? "", 0);
#endif
        //InitBLE.reconnectMat();
    }

    public void OnGoToYipliPress()
    {
        YipliHelper.GoToYipli(ProductMessages.noMatCase);
    }

    private void DisableTroubleshootButton()
    {
        //troubleshootButton.transform.GetChild(1).gameObject.SetActive(true);
        //troubleshootButton.SetActive(false);
        //installDriverButton.SetActive(false);
    }

    private void EnableTroubleshootButton()
    {
        //troubleshootButton.SetActive(true);
        //troubleshootButton.transform.GetChild(1).gameObject.SetActive(false);
    }

    /*
    #region driver troubleshoot
#if UNITY_STANDALONE_WIN
    // troubole mat drivers and connection
    public void TroubleshootButton()
    {
        EnableTroubleshootingPanel();
    }

    public void TroubleshootSystem()
    {
        FileReadWrite.WriteToFileForDriverSetup(currentYipliConfig.gameId);
        FileReadWrite.ReadFromFile();

        StartCoroutine(StartValidationUI());

        FileReadWrite.CheckIfMatDriverIsInstalled(currentYipliConfig.gameId);
    }

    private IEnumerator StartValidationUI()
    {
        while (!FileReadWrite.DriverInstalledFinished) 
        {
            troubleshootButton.transform.GetChild(1).gameObject.SetActive(true);

            noMatText.text = string.Empty;
            yield return new WaitForSecondsRealtime(1f);

            FileReadWrite.ReadFromFile();
        }

        // after validation is done
        //noMatText.text = ProductMessages.Err_mat_connection_retry;
        troubleshootButton.transform.GetChild(1).gameObject.SetActive(false);

        yipliHomeButton.SetActive(true);
        retryButton.SetActive(true);
        troubleshootButton.SetActive(true);
        installDriverButton.SetActive(false);

        ReCheckMatConnection();
    }

    private void EnableTroubleshootingPanel()
    {
        noMatText.text = ProductMessages.Err_mat_connection_no_driver;

        yipliHomeButton.SetActive(false);
        retryButton.SetActive(false);
        troubleshootButton.SetActive(false);
        installDriverButton.SetActive(true);
    }
#endif
    
    #endregion
    */

    // TroubleShoot System
    public void TroubleShootSystemFromMS()
    {
        SceneManager.LoadScene("Troubleshooting");
    }
}

//Register the YIPLI fitness mat from Yipli Hub to continue playing.