using UnityEngine;
using TMPro;
using UnityEngine.UI;
#if UNITY_STANDALONE_WIN
using yipli.Windows;
#endif
using System.Collections;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using YipliFMDriverCommunication;
using BLEFramework.MiniJSON;

public class TroubleShootSystem : MonoBehaviour
{
    // required objects
    // Questions
    [Header("Question Lists")]
    //[SerializeField] Questions gameQuestions;
    //[SerializeField] Questions matQuestions;
    [SerializeField] TroubleShootManagerS troubleshootManager;

    private AllQuestions aq;

    // yipli config
    [Header("Yipli config")]
    [SerializeField] YipliConfig currentYipliConfig;

    // Display Objects
    [Header("UI Elements")]
    // text elements
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI solutionText;
    [SerializeField] TextMeshProUGUI sampleText;
    [SerializeField] TextMeshProUGUI messageBoxText;
    [SerializeField] TextMeshProUGUI notesText;

    // buttons
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;
    [SerializeField] Button notSureButton;
    [SerializeField] Button continueButton;

    [Header("UI Panels")]
    // gameobjects
    [SerializeField] GameObject animationParent;
    [SerializeField] GameObject entryPanel;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject questionAnswerPage;
    [SerializeField] GameObject practicalTaskPanel;
    [SerializeField] GameObject messageBoxPanel;
    [SerializeField] GameObject notesPanel;

    private PracticalTask practicalTaskManager;

    // blob data objects
    private List<BlobData> newBlobData = null;
    private BlobData blobData;

    //const string siliconLabsDesc = "Silicon Labs CP210x USB to UART Bridge";
    //const string siliconLabsManufacturer = "Silicon Labs";
    private bool checkMatActions = false;

    public bool yesClicked = false;
    public bool notSureClicked = false;
    public bool noClicked = false;

    // questions flags
    private bool questionAsked = false;
    private bool solutionProvided = false;

    // fm response variables
    private string lastPixelSituations = null;

    // file data variables
    private string flowInfo = "Start->";

    public bool QuestionAsked { get => questionAsked; set => questionAsked = value; }
    public bool SolutionProvided { get => solutionProvided; set => solutionProvided = value; }
    public string LastPixelSituations { get => lastPixelSituations; set => lastPixelSituations = value; }
    public bool YesClicked { get => yesClicked; set => yesClicked = value; }
    public bool NotSureClicked { get => notSureClicked; set => notSureClicked = value; }
    public bool NoClicked { get => noClicked; set => noClicked = value; }
    public bool CheckMatActions { get => checkMatActions; set => checkMatActions = value; }
    public string FlowInfo { get => flowInfo; set => flowInfo = value; }
    public TroubleShootManagerS TroubleshootManager { get => troubleshootManager; set => troubleshootManager = value; }
    public YipliConfig CurrentYipliConfig { get => currentYipliConfig; set => currentYipliConfig = value; }

    private void Awake()
    {
        aq = FindObjectOfType<AllQuestions>();
        practicalTaskManager = FindObjectOfType<PracticalTask>();
    }

    private void Start()
    {
        TroubleshootManager.ResetTroubleShootChecks();
        ResetTroubleShooter();
    }

    public void ResetTroubleShooter()
    {
        TurnOffAllPanels();
        TurnOnNotesPanel(ProductMessages.StartNote);
    }

    private void Update()
    {
        if (CheckMatActions)
        {
            TroubleShootFmResponseManager();
        }
    }

    #region gameQuestions
    // game specific questions
    // Game question 2
    public void IsOsUpdatedToLates()
    {
        SetGameQuestionText(2);

        FlowInfo += "G2->";

        //troubleshootManager.OsUpdateCheck = true;
    }

    // Game question 3
    public void IsStuckOnPlayerFetchingDetails()
    {
        if (CurrentYipliConfig.playerInfo == null)
        {
            if (IsInternetAvailable())
            {
                // TODO : check for backend curruption;
            }
            else
            {
                SetGameSolutionText(5, 0); // ask user to provide internet
            }
        }

        FlowInfo += "G3->";
        //troubleshootManager.PlayerFetchingCheckDone = true;
    }

    // Game question 4
    public void IsStuckOnNoMatPanel()
    {
        if (YipliHelper.GetMatConnectionStatus() != "Connected")
        {
#if UNITY_ANDROID
            // check phone ble
            /*
            if (!IsBLEListHasYipli())
            {
                SetGameQuestionText(4);
            }*/
#elif UNITY_STANDALONE_WIN
            if (!IsMatConnectedToUSB())
            {
                SetGameSolutionText(6, 0); // ask user to connect via usb
            }

            IsBackgroundAppsRunning();
#endif
        }

        FlowInfo += "G4->";
        //troubleshootManager.NoMatPanelCheckDone = true;
    }

    // Game question 5
    public bool IsInternetAvailable()
    {
        FlowInfo += "G5->";
        //troubleshootManager.InternetConnectionTest = true;
        return YipliHelper.checkInternetConnection();
    }

    // Game question 6
    public bool IsMatConnectedToUSB()
    {
        FlowInfo += "G6->";
        //troubleshootManager.MatUsbConnectionTest = true;
        // check if driver can check it
        return true;
    }

    // Game question 7
    public bool IsPhoneBleOn()
    {
        FlowInfo += "G7->";
        //troubleshootManager.PhoneBleTest = true;
        // check if driver can check it
        return true;
    }

    // Game question 8
    public void IsMatAddedInYipliAccount()
    {
        if (YipliHelper.GetMatConnectionStatus() != "Connected")
        {
            // ask driver if the mac address is same that is being trying to connect
            // if not same aske user to add it
            SetGameQuestionText(8); // provide question id from the flowchart
            // else provide solution to make it active and reconnect
            SetGameSolutionText(8, 2);
        }

        FlowInfo += "G8->";
        //troubleshootManager.MatInYipliAccountCheckDone = true;
    }

    // Game question 9
    public void IsBackgroundAppsRunning()
    {
#if UNITY_STANDALONE_WIN
        // check if driver can check it
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (FileReadWrite.CheckIfOtherProcessesAreRunning())
            {
                SetGameSolutionText(9, 0);
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            // try package manager for android
        }

        troubleshootManager.BackgroundAppsRunningCheckDone = true;
#endif
        FlowInfo += "G9->";
    }

    // Game question 10
    public void AreGamesAndAppUpdated()
    {
        // compare version here
        int gameVersionCode = YipliHelper.convertGameVersionToBundleVersionCode(Application.version);
        int inventoryVersionCode = YipliHelper.convertGameVersionToBundleVersionCode(CurrentYipliConfig.gameInventoryInfo.gameVersion);


        if (inventoryVersionCode > gameVersionCode)
        {
            //Ask user to Update Game version option
            SetGameSolutionText(10, 0);
        }

        FlowInfo += "G10->";
        //troubleshootManager.GamesAndAppUpdateCheckDone = true;
    }

    #endregion

    #region behaviour questions
    // Game question 11
    public void IsBehaviourSameGames()
    {
        if (TroubleshootManager.SameBehaviourGamesAsked && TroubleshootManager.SameBehaviourGamessolutionProvided) return;

        if (!TroubleshootManager.SameBehaviourGamesAsked)
        {
            TroubleshootManager.SameBehaviourGamesAsked = true;
            SetGameQuestionText(11); // provide question id from the flowchart
        }
        else if (NotSureClicked && !TroubleshootManager.SameBehaviourGamessolutionProvided)
        {
            SetGameSolutionText(11, 0);
            TroubleshootManager.SameBehaviourGamessolutionProvided = true;
        }

        FlowInfo += "G11->";
    }

    // Game question 12
    public void IsBehaviourSamePlatform()
    {
        if (TroubleshootManager.SameBehaviourPlatformAsked && TroubleshootManager.SameBehaviourPlatformsolutionProvided) return;

        if (!TroubleshootManager.SameBehaviourPlatformAsked)
        {
            TroubleshootManager.SameBehaviourPlatformAsked = true;
            SetGameQuestionText(12); // provide question id from the flowchart
        }
        else if (NotSureClicked && !TroubleshootManager.SameBehaviourPlatformsolutionProvided)
        {
            SetGameSolutionText(12, 0);
            TroubleshootManager.SameBehaviourPlatformsolutionProvided = true;
        }

        FlowInfo += "G12->";
    }

    // Game question 13
    public void IsBehaviourRandomOrPersistent()
    {
        if (TroubleshootManager.BehaviourRondomOrPersistentAsked && TroubleshootManager.BehaviourRondomOrPersistentProvided) return;

        if (!TroubleshootManager.BehaviourRondomOrPersistentAsked)
        {
            TroubleshootManager.BehaviourRondomOrPersistentAsked = true;
            SetGameQuestionText(13); // provide question id from the flowchart
        }
        else if (NotSureClicked && !TroubleshootManager.BehaviourRondomOrPersistentProvided)
        {
            // set something for persistent or random behaviour
            TroubleshootManager.BehaviourRondomOrPersistentProvided = true;
        }
        FlowInfo += "G13->";
    }

    #endregion

    #region Mat questions
    // Mat flow questions
    /*
    // Mat question 1
    public void IsMatOn()
    {
        if (TroubleshootManager.MatOnCheck && TroubleshootManager.IsMatOnSolutionProvided) return;

        if (!TroubleshootManager.MatOnCheck)
        {
            SetMatQuestionText(1);
            TroubleshootManager.MatOnCheck = true;
        }

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

        if (NoClicked && !TroubleshootManager.IsMatOnSolutionProvided)
        {
            SetMatSolutionText(1, 0);
            TroubleshootManager.IsMatOnSolutionProvided = true;
        }
        else
        {
            TroubleshootManager.IsMatOnSolutionProvided = true;
        }

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

        FlowInfo += "M1->";
    }

    // Mat question 2
    public void WhatIsTheColorOfLED()
    {
        if (TroubleshootManager.ColorOfLED && TroubleshootManager.RedLedSolutionProvided) return;

        if (!TroubleshootManager.ColorOfLED)
        {
            // if (battery level < 15)
            SetMatQuestionText(2);

            TroubleshootManager.ColorOfLED = true;
        }

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

        if (NoClicked && !TroubleshootManager.RedLedSolutionProvided)
        {
            SetMatSolutionText(2, 0);
            TroubleshootManager.RedLedSolutionProvided = true;
        }
        else
        {
            TroubleshootManager.RedLedSolutionProvided = true;
        }

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

        FlowInfo += "M2->";
    }
    */

    // Mat question 3
    public void IsChargingLightVisible()
    {
        if (TroubleshootManager.CharginglightVisibility && TroubleshootManager.ChargingLightVisibilitySolutionProvided) return;

        if (!TroubleshootManager.CharginglightVisibility)
        {
            SetMatQuestionText(3);
            TroubleshootManager.CharginglightVisibility = true;

        }

        if ((NoClicked || NotSureClicked) && !TroubleshootManager.ChargingLightVisibilitySolutionProvided)
        {
            SetMatSolutionText(3, 0);
            TroubleshootManager.ChargingLightVisibilitySolutionProvided = true;
        }
        else
        {
            TroubleshootManager.ChargingLightVisibilitySolutionProvided = true;
        }

        FlowInfo += "M3->";
    }

    // Mat question 4
    public IEnumerator IsBLEListHasYipliAndroid()
    {
        // ask driver to confirm

        Debug.LogError("MatIsNotGettingConnected : from ble list couroutine.");

        TurnOnLoadingPanel();
        InitBLE.ScanForPeripherals();

        yield return new WaitForSecondsRealtime(12f);

        // string peripheralJsonList = "F4:BF:80:63:E3:7A|honor Band 4-37A,F9:4B:4A:BF:66:C1|Amazfit Bip U,F5:FB:4A:55:76:22|Mi Smart Band 4,F5:FV:4A:55:80:22|Yipli";

        string peripheralJsonList = InitBLE.PeripheralJsonList;

        string[] allBleDevices = peripheralJsonList.Split(',');

        for (int i = 0; i < allBleDevices.Length; i++)
        {
            string[] tempSplits = allBleDevices[i].Split('|');

            if (tempSplits[1].Equals(LibConsts.MatTempAdvertisingNameOnlyForNonIOS, StringComparison.OrdinalIgnoreCase))
            {
                TroubleshootManager.BleScannedMacAddress = tempSplits[0];
                break;
            }
        }
        
        TurnOffLoadingPanel();

        if (TroubleshootManager.BleScannedMacAddress == null || TroubleshootManager.BleScannedMacAddress == "" || TroubleshootManager.BleScannedMacAddress == string.Empty)
        {
            AreThereAnyOtherDeviceConectedToMat();

            // if question is asked or solution is provided that return;
            if (StopFurtherProcesses()) yield break;

            // ble is not shown
            SetMatSolutionText(4, 0);

            // generate ticket here for ble module
            string desc = "Bluetooth module is not working";
            FmResponseFile.GenerateFilesAndUpload(null, FlowInfo, TroubleshootManager.CurrentAlgorithmID, CurrentYipliConfig, desc, TroubleshootManager.GetTroubleShootScriptableJson());

            // if question is asked or solution is provided that return;
            if (StopFurtherProcesses()) yield break;
        }
        else
        {
            Debug.LogError("scan has ble. mac address is : " + TroubleshootManager.BleScannedMacAddress);
            sampleText.text = "all mac addresses : " + InitBLE.PeripheralJsonList + "\nscan has ble. mac address is : " + TroubleshootManager.BleScannedMacAddress;

            IsThisSameMatAddedInYipliAccountAndroid(TroubleshootManager.BleScannedMacAddress);

            // if question is asked or solution is provided that return;
            if (StopFurtherProcesses()) yield break;

            // start suraj driver test
            StartPracticalTask();

            TurnOffLoadingPanel();
        }

        FlowInfo += "M4->";
    }

#if UNITY_STANDALONE_WIN
    // Mat question 5
    public void IsSiliconDrivreInstalled()
    {
        troubleshootManager.SiliconDriverInstallCheck = true;

        if (!troubleshootManager.SiliconDriverInstallCheck)
        {
            TroubleshootSystem();
        }

        FlowInfo += "M5->";
    }

    public void TroubleshootSystem()
    {
        FileReadWrite.WriteToFileForDriverSetup(currentYipliConfig.gameId);
        FileReadWrite.ReadFromFile();

        StartCoroutine(WindowsCMDCheck());

        FileReadWrite.CheckIfMatDriverIsInstalled(currentYipliConfig.gameId);
    }

    private IEnumerator WindowsCMDCheck()
    {
        TurnOnLoadingPanel();

        while (!FileReadWrite.DriverInstalledFinished)
        {
            yield return new WaitForSecondsRealtime(1f);

            FileReadWrite.ReadFromFile();
        }

        // after validation is done
        troubleshootManager.SiliconDriverInstallCheck = true;

        AreThereAnyOtherDeviceConectedToMat();

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) yield break;

        // start suraj driver test
        StartPracticalTask();

        TurnOffLoadingPanel();
    }
#endif

    // Mat question 6
    public bool IsSiliconPortAvailableInDeviceManager()
    {
#if UNITY_STANDALONE_WIN
        troubleshootManager.SiliconPortAvailability = true;

        FlowInfo += "M6->";
        return FileReadWrite.DriverInstalledFinished;
#else
        FlowInfo += "M6->";
        return true;
#endif
    }

    // Mat question 7
    public void AreThereAnyOtherDeviceConectedToMat()
    {
        if (TroubleshootManager.IsMatConnectedToOtherDeviceCheckDone && TroubleshootManager.IsMatConnectedToOtherDeviceSolutionProvided) return;

        if (!TroubleshootManager.IsMatConnectedToOtherDeviceCheckDone)
        {
            // ask driver to confirm
            SetMatQuestionText(7);
            TroubleshootManager.IsMatConnectedToOtherDeviceCheckDone = true;
        }

        if (YesClicked && !TroubleshootManager.IsMatConnectedToOtherDeviceSolutionProvided)
        {
            // on Yes click
            SetMatSolutionText(7, 0);
        }
        else
        {
            TroubleshootManager.IsMatConnectedToOtherDeviceSolutionProvided = true;
        }

        FlowInfo += "M7->";
    }

    // Mat question 8
    public void IsThisSameMatAddedInYipliAccountAndroid(string scannedMacAddress)
    {
        if (!TroubleshootManager.SameMatFromYipliCheckDone && !CurrentYipliConfig.matInfo.macAddress.Equals(scannedMacAddress, StringComparison.OrdinalIgnoreCase))
        {
            SetMatQuestionText(8); // provide question id from the flowchart
            TroubleshootManager.SameMatFromYipliCheckDone = true;
        }

        if ((NoClicked || NotSureClicked) && !TroubleshootManager.SameMatFromYipliSolutionProvided)
        {
            // else provide solution to make it active and reconnect
            SetMatSolutionText(8, 0);
            TroubleshootManager.SameMatFromYipliSolutionProvided = true;
        }

        FlowInfo += "M8->";
    }

    #endregion

    #region UI Management
    // UI management
    private void SetGameQuestionText(int qid)
    {
        solutionText.text = "";
        questionText.text = "";

        for (int i = 0; i < aq.GameQuestions.Count; i++)
        {
            if (aq.GameQuestions[i].id == qid)
            {
                questionText.text = aq.GameQuestions[i].question;

                EnableChoices(i);
                break;
            }
        }

        QuestionAsked = true;
    }

    private void SetGameSolutionText(int qid, int solutionID)
    {
        DisableChoices();
        continueButton.gameObject.SetActive(true);

        solutionText.text = "";
        questionText.text = "";

        for (int i = 0; i < aq.GameQuestions.Count; i++)
        {
            if (aq.GameQuestions[i].id == qid)
            {
                solutionText.text = aq.GameQuestions[i].solutions[solutionID];
            }
        }

        solutionProvided = true;
    }

    private void SetMatQuestionText(int qid)
    {
        solutionText.text = "";
        questionText.text = "";

        for (int i = 0; i < aq.MatQuestions.Count; i++)
        {
            if (aq.MatQuestions[i].id == qid)
            {
                questionText.text = aq.MatQuestions[i].question;

                EnableChoices(i, "m");
                break;
            }
        }

        QuestionAsked = true;
    }

    private void SetMatSolutionText(int qid, int solutionID)
    {
        DisableChoices();
        continueButton.gameObject.SetActive(true);

        solutionText.text = "";
        questionText.text = "";

        for (int i = 0; i < aq.MatQuestions.Count; i++)
        {
            if (aq.MatQuestions[i].id == qid)
            {
                solutionText.text = aq.MatQuestions[i].solutions[solutionID];
            }
        }

        solutionProvided = true;
    }

    private void EnableChoices(int qid, string matOrGame = "g")
    {
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
        notSureButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);

        if (matOrGame == "m")
        {
            yesButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = aq.MatQuestions[qid].choices[0];
            noButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = aq.MatQuestions[qid].choices[1];
            notSureButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = aq.MatQuestions[qid].choices[2];
        }
        else
        {
            yesButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = aq.GameQuestions[qid].choices[0];
            noButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = aq.GameQuestions[qid].choices[1];
            notSureButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = aq.GameQuestions[qid].choices[2];
        }
    }

    private void DisableChoices()
    {
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        notSureButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }

    private bool StopFurtherProcesses()
    {
        TurnOffLoadingPanel();

        if (questionAsked) return true;
        if (solutionProvided) return true;

        return false;
    }

    private void TurnOffAllPanels()
    {
        entryPanel.SetActive(false);
        loadingPanel.SetActive(false);
        questionAnswerPage.SetActive(false);
        practicalTaskPanel.SetActive(false);
        messageBoxPanel.SetActive(false);
        notesPanel.SetActive(false);
    }

    private void TurnOnLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }

    private void TurnOffLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }

    public void TurnOnEntryPanel()
    {
        TurnOffAllPanels();
        entryPanel.SetActive(true);
    }

    private void TurnOffEntryPanel()
    {
        entryPanel.SetActive(false);
    }

    private void StartUserInteraction()
    {
        TurnOffEntryPanel();

        loadingPanel.SetActive(true);
        questionAnswerPage.SetActive(true);
    }

    private void TurnOnPracticalPanel()
    {
        practicalTaskPanel.SetActive(true);
    }

    private void TurnOffPracticalPanel()
    {
        practicalTaskPanel.SetActive(false);
    }

    public void TurnOnMessageBoxPanel(string message)
    {
        messageBoxText.text = message;
        messageBoxPanel.SetActive(true);
    }

    private void TurnOffMessageBoxPanel()
    {
        messageBoxText.text = "";
        messageBoxPanel.SetActive(false);
    }

    private void TurnOnNotesPanel(string msg)
    {
        notesText.text = msg;
        notesPanel.SetActive(true);
    }

    private void TurnOffNotesPanel()
    {
        notesText.text = "";
        notesPanel.SetActive(false);
    }

    #endregion

    #region Flow Algorithms
    // flow algorithms
    // Set behaviour questions
    public void BehaviourQuestionsFlow()
    {
        // ask behavioural questions
        //troubleshootManager.CurrentAlgorithmID = 9;

        IsBehaviourSameGames();

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

        IsBehaviourSamePlatform();

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

        IsBehaviourRandomOrPersistent();

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;
    }

    // Gameplay actions are not working
    public void GameplayActionsAreNotWorking()
    {
        StartUserInteraction();

        // assuming game is not crashing as applications is working
        TroubleshootManager.CurrentAlgorithmID = 1;

        // Game questions flow
        IsStuckOnPlayerFetchingDetails();

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

        IsStuckOnNoMatPanel();

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

        AreGamesAndAppUpdated();

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

        // behaviout questions
        BehaviourQuestionsFlow();

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

        // mat questions flow
        /*
        IsMatOn();

        WhatIsTheColorOfLED();
        */

        StartPracticalTask();

        TurnOffLoadingPanel();

        //Debug.LogError("No Problem is found");
       //// ResetTroubleShooter();
      ///  TurnOnMessageBoxPanel("No Problem is found");
    }

    // game crashing
    public void GameCrashingIssue()
    {
        StartUserInteraction();

        // games are already crashing
        TroubleshootManager.CurrentAlgorithmID = 2;

        if (!TroubleshootManager.OsUpdateCheck)
        {
            SetGameQuestionText(2);
            TroubleshootManager.OsUpdateCheck = true;
        }

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

        if ((NoClicked || NotSureClicked) && !TroubleshootManager.OsUpdateSolutionProvided)
        {
            SetGameSolutionText(2, 1);
            TroubleshootManager.OsUpdateSolutionProvided = true;
        }
        else if (YesClicked)
        {
            SetGameSolutionText(2, 0);
            TroubleshootManager.OsUpdateSolutionProvided = true;
        }
        else
        {
            TroubleshootManager.OsUpdateSolutionProvided = true;
        }

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

        // ask user for the behaviour
        BehaviourQuestionsFlow();

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

        SetGameSolutionText(10, 0); // ask user to update app and games

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

        TurnOffLoadingPanel();
    }

    // actions are not getting detected
    public void MatActionsAreNotGettingDetected()
    {
        StartUserInteraction();

        TroubleshootManager.CurrentAlgorithmID = 3;

        // mat questions flow
        /*
        IsMatOn();

        WhatIsTheColorOfLED();
        */

        StartPracticalTask();

        TurnOffLoadingPanel();
    }

    // USB cable  is not working
    public void USBCableNotWorking()
    {
        if ((Application.platform != RuntimePlatform.WindowsEditor) || (Application.platform != RuntimePlatform.WindowsPlayer))
        {
            Debug.LogError("Non USB Platform is detected");
            ResetTroubleShooter();
            TurnOnMessageBoxPanel("This platform is not working with USB cable. Try on Windows machine");
            return;
        }

        StartUserInteraction();

        TroubleshootManager.CurrentAlgorithmID = 4;

        // mat questions flow
        /*
        IsMatOn();

        WhatIsTheColorOfLED();
        */

        // ask for charging light
        IsChargingLightVisible();

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

#if UNITY_STANDALONE_WIN
        IsSiliconDrivreInstalled();
#endif
    }

    // Mat is not getting connected
    public void MatIsNotGettingConnected()
    {
        StartUserInteraction();

        TroubleshootManager.CurrentAlgorithmID = 5;

        // mat questions flow
        /*
        IsMatOn();

        WhatIsTheColorOfLED();
        */
        Debug.LogError("MatIsNotGettingConnected : color of led is done.");

#if UNITY_STANDALONE_WIN
        IsChargingLightVisible();

        // if question is asked or solution is provided that return;
        if (StopFurtherProcesses()) return;

        IsSiliconDrivreInstalled();
#elif UNITY_ANDROID
        Debug.LogError("MatIsNotGettingConnected : next line is ble list couroutine.");

        StartCoroutine(IsBLEListHasYipliAndroid());

        // if question is asked or solution is provided that return;
        //if (StopFurtherProcesses()) return;
#endif
    }

    // Game is not getting launched
    public void GameisNotGetingLaunched()
    {
        StartUserInteraction();

        TroubleshootManager.CurrentAlgorithmID = 6;
        // start yipli app trouble shooting flow

        TurnOffLoadingPanel();
    }

    // Mat is not starting
    public void MatIsNotStarting()
    {
        StartUserInteraction();

        TroubleshootManager.CurrentAlgorithmID = 7;
        // start ticket system flow
        string desc = "Mat is not getting started";
        FmResponseFile.GenerateFilesAndUpload(null, FlowInfo, TroubleshootManager.CurrentAlgorithmID, currentYipliConfig, desc, TroubleshootManager.GetTroubleShootScriptableJson());

        TurnOffLoadingPanel();
    }

    // full troubleshoot
    public void FullTroubleShoot()
    {
        StartUserInteraction();

        TroubleshootManager.CurrentAlgorithmID = 8;
        // start ticket system flow
        string desc = "Full trouble shoot is requested";
        FmResponseFile.GenerateFilesAndUpload(null, FlowInfo, TroubleshootManager.CurrentAlgorithmID, currentYipliConfig, desc, TroubleshootManager.GetTroubleShootScriptableJson());

        TurnOffLoadingPanel();
    }

#endregion

    #region Button Functions
    // button functions
    public void YesButtonFunction()
    {
        YesClicked = true;

        TurnOnLoadingPanel();

        questionAsked = false;
        solutionProvided = false;

        ManageCurrentAlgorithm();
    }

    public void NoButtonFunction()
    {
        NoClicked = true;

        TurnOnLoadingPanel();

        questionAsked = false;
        solutionProvided = false;

        ManageCurrentAlgorithm();
    }

    public void NotSureButtonFunction()
    {
        NotSureClicked = true;

        TurnOnLoadingPanel();

        questionAsked = false;
        solutionProvided = false;

        ManageCurrentAlgorithm();
    }

    public void OkayButton()
    {
        questionAsked = false;
        solutionProvided = false;

        TurnOffMessageBoxPanel();
    }

    public void ContinueButton()
    {
        questionAsked = false;
        solutionProvided = false;

        ResetButtonFlags();
        ResetTroubleShooter();
    }

    private void ResetButtonFlags()
    {
        YesClicked = false;
        NotSureClicked = false;
        NoClicked = false;
    }

    public void QuitTroubleShooting() {
        Application.Quit();
    }

    // current algorithm manager
    public void ManageCurrentAlgorithm()
    {
        switch (TroubleshootManager.CurrentAlgorithmID)
        {
            case 1:
                GameplayActionsAreNotWorking();
                break;

            case 2:
                GameCrashingIssue();
                break;

            case 3:
                MatActionsAreNotGettingDetected();
                break;

            case 4:
                USBCableNotWorking();
                break;

            case 5:
                MatIsNotGettingConnected();
                break;

            case 6:
                GameisNotGetingLaunched();
                break;

            case 7:
                MatIsNotStarting();
                break;

            case 8:
                FullTroubleShoot();
                break;

            default:
                FullTroubleShoot();
                break;
        }
    }

#endregion

    #region PracticalTaskManager

    // specific practical task
    public void StartPracticalTask()
    {
        if (YipliHelper.GetMatConnectionStatus().Equals("Connected", StringComparison.OrdinalIgnoreCase))
        {
            TurnOffAllPanels();
            SetTroubleShootClusterID();

            TurnOffEntryPanel();
            TurnOnPracticalPanel();

            practicalTaskManager.ManagePracticalTaskStepOne();
        }
        else
        {
            // handle case for no mat connection
            // practical should not start
        }
    }

#endregion

    #region DriverResponseMnaagers

    public async Task<object> GetPracticalTaskDriverResponse()
    {
        // update await part with driver response
        await Task.Delay(TimeSpan.FromSeconds(1));
        return newBlobData;
    }

    public void SetTroubleShootClusterID()
    {
        // trouble shooting new cluster id is 999
        YipliHelper.SetGameClusterId(999);
    }

    private void TroubleShootFmResponseManager()
    {
        // test only
        //practicalTaskManager.ManagePracticaltask("");
        //return;

        // actual code
        string fmActionData = InitBLE.GetFMResponse();
        Debug.Log("Json Data from Fmdriver : " + fmActionData);

        FmDriverResponseInfo singlePlayerResponse = JsonUtility.FromJson<FmDriverResponseInfo>(fmActionData);

        if (singlePlayerResponse == null) return;

        if (PlayerSession.Instance.currentYipliConfig.oldFMResponseCount != singlePlayerResponse.count)
        {
            Debug.LogError("FMResponse " + fmActionData);
            PlayerSession.Instance.currentYipliConfig.oldFMResponseCount = singlePlayerResponse.count;

            YipliUtils.PlayerActions providedAction = ActionAndGameInfoManager.GetActionEnumFromActionID(singlePlayerResponse.playerdata[0].fmresponse.action_id);

            switch (providedAction)
            {
                case YipliUtils.PlayerActions.TROUBLESHOOTING:

                    if (singlePlayerResponse.playerdata[0].fmresponse.properties.ToString() != "null")
                    {
                        string[] tokens = singlePlayerResponse.playerdata[0].fmresponse.properties.Split(':');

                        if (tokens.Length > 0)
                        {
                            if (tokens[0].Equals("array"))
                            {
                                if (LastPixelSituations != tokens[1])
                                {
                                    LastPixelSituations = tokens[1];

                                    Debug.LogError("array - response : " + tokens[1]);
                                    practicalTaskManager.ManagePracticaltask(tokens[1], singlePlayerResponse.playerdata[0].fmresponse.ToString());
                                }
                            }
                        }
                    }

                    //practicalTaskManager.TransformLegs("");
                    break;

                case YipliUtils.PlayerActions.JUMPING_JACK:

                    if (singlePlayerResponse.playerdata[0].fmresponse.properties.ToString() != "null")
                    {
                        string[] tokens = singlePlayerResponse.playerdata[0].fmresponse.properties.Split(':');

                        if (tokens.Length > 0)
                        {
                            if (tokens[0].Equals("array"))
                            {
                                practicalTaskManager.ManagePracticaltask(tokens[1], singlePlayerResponse.playerdata[0].fmresponse.ToString(), "JJ");

                                /*
                                if (LastPixelSituations != tokens[1])
                                {
                                    LastPixelSituations = tokens[1];

                                    Debug.LogError("array - response : " + tokens[1]);
                                    practicalTaskManager.ManagePracticaltask(tokens[1], "JJ");
                                }
                                */
                            }
                        }
                    }
                    break;

                default:
                    Debug.LogError("Wrong Actions detected : " + providedAction.ToString());
                    break;
            }
        }
    }

    #endregion

    #region Test functions

    public void TestYipliList()
    {
        // test code
        /*
        Debug.LogError("Systeminfo batterylevel : " + SystemInfo.batteryLevel);
        Debug.LogError("Systeminfo batteryStatus : " + SystemInfo.batteryStatus);
        Debug.LogError("Systeminfo deviceModel : " + SystemInfo.deviceModel);
        Debug.LogError("Systeminfo deviceName : " + SystemInfo.deviceName);
        Debug.LogError("Systeminfo deviceType : " + SystemInfo.deviceType);
        Debug.LogError("Systeminfo deviceUniqueIdentifier : " + SystemInfo.deviceUniqueIdentifier);
        Debug.LogError("Systeminfo graphicsDeviceID : " + SystemInfo.graphicsDeviceID);
        Debug.LogError("Systeminfo graphicsDeviceName : " + SystemInfo.graphicsDeviceName);
        Debug.LogError("Systeminfo graphicsDeviceType : " + SystemInfo.graphicsDeviceType);
        Debug.LogError("Systeminfo graphicsDeviceVendor : " + SystemInfo.graphicsDeviceVendor);
        Debug.LogError("Systeminfo graphicsDeviceVendorID : " + SystemInfo.graphicsDeviceVendorID);
        Debug.LogError("Systeminfo graphicsDeviceVersion : " + SystemInfo.graphicsDeviceVersion);
        Debug.LogError("Systeminfo graphicsMemorySize : " + SystemInfo.graphicsMemorySize);
        Debug.LogError("Systeminfo graphicsMultiThreaded : " + SystemInfo.graphicsMultiThreaded);
        Debug.LogError("Systeminfo graphicsShaderLevel : " + SystemInfo.graphicsShaderLevel);
        Debug.LogError("Systeminfo hasDynamicUniformArrayIndexingInFragmentShaders : " + SystemInfo.hasDynamicUniformArrayIndexingInFragmentShaders);
        Debug.LogError("Systeminfo hasHiddenSurfaceRemovalOnGPU : " + SystemInfo.hasHiddenSurfaceRemovalOnGPU);
        Debug.LogError("Systeminfo hasMipMaxLevel : " + SystemInfo.hasMipMaxLevel);
        Debug.LogError("Systeminfo operatingSystem : " + SystemInfo.operatingSystem);
        Debug.LogError("Systeminfo operatingSystemFamily : " + SystemInfo.operatingSystemFamily);
        Debug.LogError("Systeminfo processorCount : " + SystemInfo.processorCount);
        Debug.LogError("Systeminfo processorFrequency : " + SystemInfo.processorFrequency);
        Debug.LogError("Systeminfo processorType : " + SystemInfo.processorType);
        */

        Debug.LogError("ts json : " + TroubleshootManager.GetTroubleShootScriptableJson());
    }

    #endregion
}