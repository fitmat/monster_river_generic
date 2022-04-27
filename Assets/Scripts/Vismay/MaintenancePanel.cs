using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class MaintenancePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI message;
    //[SerializeField] TextMeshProUGUI title;

    [SerializeField] GameObject maintenancePanel;

    [SerializeField] YipliConfig currentYipliConfig;

    [SerializeField] NewUIManager newUIManager = null;

    [SerializeField] NewMatInputController newMatInputController = null;

    [SerializeField] PlayerSelection playerSelection = null;

    [SerializeField] MatSelection matSelection = null;

    [Header("UI objects")]
    [SerializeField] GameObject skipButton = null;
    [SerializeField] GameObject updateButton = null;
    [SerializeField] Sprite errorSprite = null;

    [Header("Required Script objects")]
    [SerializeField] private Color yipliRed;
    [SerializeField] private Color yipliMarine;
    [SerializeField] private Color yipliBubble;

    private void Start()
    {
        maintenancePanel.SetActive(false);
        //newUIManager.TurnOffMainCommonButton();
        skipButton.SetActive(false);
    }

    private void Update()
    {
        // panels with buttons
        // noInternetPanel, noMatPanel(Guest User panel), maintanencePanel, noMatConnectionPanel, phoneHolderTutorialPanel, minimum2Player

        // Debug.LogError("versions : maintenancePanel.activeSelf : " + maintenancePanel.activeSelf);

        //if (maintenancePanel.activeSelf || playerSelection.noNetworkPanel.activeSelf || playerSelection.GuestUserPanel.activeSelf || playerSelection.phoneHolderInfo.activeSelf || playerSelection.Minimum2PlayersPanel.activeSelf || matSelection.NoMatPanel.activeSelf) {
        //    newUIManager.TurnOnMainCommonButton();
        //    newMatInputController.MakeSortLayerZero();
        //} else {
        //    newUIManager.TurnOffMainCommonButton();
        //    newMatInputController.MakeSortLayerTen();
        //}

        ManageManitanenceOrBlocking();
        //BlockIfTroubleShootingIsOn();
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    private void ManageManitanenceOrBlocking()
    {
        // Debug.LogError("versions : from manage maintanence or blocking : ");

        if (currentYipliConfig.gameInventoryInfo == null) return;

        /*
        // for testing turn isGameUnderMaintenance to 0, or it has to be 1. Do not change value in firebase, that will immediately block live customers gameplay
        if (currentYipliConfig.gameInventoryInfo.isGameUnderMaintenance == 1) // change to 1 for release
        {
            string[] allOS = currentYipliConfig.gameInventoryInfo.osListForMaintanence.Split(',');

            Debug.LogError("Executing allOS length : " + allOS.Length);

            if (allOS.Length > 0) {
                for (int i = 0; i < allOS.Length; i++) {
                    if (allOS[i] == "a" && Application.platform == RuntimePlatform.Android) {
                        //Debug.LogError("Executing a");
                        ManageMaintanenceMessages();
                        break;
                    } else if (allOS[i] == "atv" && Application.platform == RuntimePlatform.Android && currentYipliConfig.isDeviceAndroidTV) {
                        //Debug.LogError("Executing atv");
                        ManageMaintanenceMessages();
                        break;
                    } else if (allOS[i] == "i" && Application.platform == RuntimePlatform.IPhonePlayer) {
                        //Debug.LogError("Executing i");
                        ManageMaintanenceMessages();
                        break;
                    } else if (allOS[i] == "w" && Application.platform == RuntimePlatform.WindowsPlayer) {
                        // for testing in editor (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
                        //Debug.LogError("Executing w");
                        ManageMaintanenceMessages();
                        break;
                    }
                }
            }
            return;
        }
        */

        // Debug.LogError("versions : next is os processing for maintanence : ");

        string[] allOS = currentYipliConfig.gameInventoryInfo.osListForMaintanence.Split(',');

        // Debug.LogError("versions : Executing allOS string : " + currentYipliConfig.gameInventoryInfo.osListForMaintanence);

        if (currentYipliConfig.gameInventoryInfo.osListForMaintanence != ",") {
            for (int i = 0; i < allOS.Length; i++) {
                if (allOS[i] == "a" && Application.platform == RuntimePlatform.Android) {
                    //Debug.LogError("Executing a");
                    ManageMaintanenceMessages();
                    break;
                } else if (allOS[i] == "atv" && Application.platform == RuntimePlatform.Android && currentYipliConfig.isDeviceAndroidTV) {
                    //Debug.LogError("Executing atv");
                    ManageMaintanenceMessages();
                    break;
                } else if (allOS[i] == "i" && Application.platform == RuntimePlatform.IPhonePlayer) {
                    //Debug.LogError("Executing i");
                    ManageMaintanenceMessages();
                    break;
                } else if (allOS[i] == "w" && Application.platform == RuntimePlatform.WindowsPlayer) {
                    // for testing in editor (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
                    //Debug.LogError("Executing w");
                    ManageMaintanenceMessages();
                    break;
                }
            }

            return;
        }

        // Debug.LogError("versions : next is to check which block version to call : ");

#if UNITY_ANDROID
        if (currentYipliConfig.isDeviceAndroidTV) {
            BlockVersionCheck(currentYipliConfig.gameInventoryInfo.androidTVMinVersion, currentYipliConfig.gameInventoryInfo.gameVersion);
        } else {
            BlockVersionCheck(currentYipliConfig.gameInventoryInfo.androidMinVersion, currentYipliConfig.gameInventoryInfo.gameVersion);
        }
#elif UNITY_IOS
        BlockVersionCheck(currentYipliConfig.gameInventoryInfo.iosMinVersion, currentYipliConfig.gameInventoryInfo.iosCurrentVersion);
#elif UNITY_STANDALONE_WIN
        BlockVersionCheck(currentYipliConfig.gameInventoryInfo.winMinVersion, currentYipliConfig.gameInventoryInfo.winCurrentVersion);
#endif
    }

    private void ManageMaintanenceMessages() {
        //message.text = char.ToUpper(currentYipliConfig.gameId[0]) + currentYipliConfig.gameId.Substring(1) + " is under maintanance." + "\n\nStay tuned.";
        message.text = currentYipliConfig.gameInventoryInfo.maintenanceMessage;
        //title.text = "Maintenance Notice";

        //newUIManager.UpdateButtonDisplay(maintenancePanel.tag, true);
        //newMatInputController.MakeSortLayerZero();
        updateButton.GetComponent<Image>().sprite = errorSprite;
        updateButton.transform.GetChild(0).GetComponent<Image>().sprite = errorSprite;
        updateButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = yipliMarine;
        updateButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Quit";
        maintenancePanel.SetActive(true);

        currentYipliConfig.allowMainGameSceneToLoad = false;
    }

    private void BlockVersionCheck(string notAllowedVersionString, string currentStoreVersion)
    {
        // Debug.LogError("versions : from block version check : ");

        if (notAllowedVersionString.Equals(",", System.StringComparison.OrdinalIgnoreCase)) return;

        int gameVersionCode = YipliHelper.convertGameVersionToBundleVersionCode(Application.version);
        int notAllowedVersionCode = YipliHelper.convertGameVersionToBundleVersionCode(notAllowedVersionString);
        int currentStoreCode = YipliHelper.convertGameVersionToBundleVersionCode(currentStoreVersion);

        // Debug.LogError("versions : gameVersionCode : " + gameVersionCode);
        // Debug.LogError("versions : notAllowedVersionCode : " + notAllowedVersionCode);
        // Debug.LogError("versions : currentStoreCode : " + currentStoreCode);

        if (notAllowedVersionCode > gameVersionCode)
        {
            message.text = currentYipliConfig.gameInventoryInfo.versionUpdateMessage;

            maintenancePanel.SetActive(true);
            newMatInputController.MakeSortLayerZero();

            //newUIManager.UpdateButtonDisplay(maintenancePanel.tag);

            currentYipliConfig.allowMainGameSceneToLoad = false;
        }
        else if (notAllowedVersionCode < gameVersionCode && currentStoreCode > gameVersionCode) {
            // Debug.LogError("versions : TimeDifferenceManager() : " + TimeDifferenceManager());

            // update below 6 with firebase entries
            if (currentYipliConfig.skipNormalUpdateClicked || TimeDifferenceManager() < 6) return;
            
            message.text = "A new version of " + currentYipliConfig.gameInventoryInfo.displayName + " is available.\nUpdate recommended";

            maintenancePanel.SetActive(true);
            newMatInputController.MakeSortLayerZero();
            skipButton.SetActive(true);

            //newUIManager.UpdateButtonDisplay(maintenancePanel.tag);

            currentYipliConfig.allowMainGameSceneToLoad = false;
        }
        else
        {
            skipButton.SetActive(false);
            maintenancePanel.SetActive(false);
        }
    }

    private void BlockIfTroubleShootingIsOn()
    {
        if (currentYipliConfig.thisUserTicketInfo.ticketStatus == 0) return;

#if UNITY_ANDROID || UNITY_IOS
        TroubleShootBlockCheck(currentYipliConfig.thisUserTicketInfo.bleTest);
#elif UNITY_STANDALONE_WIN
        TroubleShootBlockCheck(currentYipliConfig.thisUserTicketInfo.usbTest);
#endif
    }

    private void TroubleShootBlockCheck(string testStatusToCheck)
    {
        if (testStatusToCheck.Equals("done", System.StringComparison.OrdinalIgnoreCase))
        {
            message.text = "Your environment is currently under troubleshooting. The game is not playable here at this time.";
            //title.text = "Troubleshoot Notice";

            maintenancePanel.SetActive(true);

            currentYipliConfig.allowMainGameSceneToLoad = false;
        }
        else
        {
            skipButton.SetActive(false);
            maintenancePanel.SetActive(false);
            //newUIManager.TurnOffMainCommonButton();
        }
    }

    // button functions
    public void SkipButtonFunction() {
        currentYipliConfig.skipNormalUpdateClicked = true;

        PlayerPrefs.SetString("skippedDate", DateTime.Today.ToString());

        skipButton.SetActive(false);
        maintenancePanel.SetActive(false);

        newMatInputController.MakeSortLayerTen();
        currentYipliConfig.allowMainGameSceneToLoad = true;
    }

    // skip version management
    private int TimeDifferenceManager() {
        if (PlayerPrefs.GetString("skippedDate") == null || PlayerPrefs.GetString("skippedDate") == "" || PlayerPrefs.GetString("skippedDate") == string.Empty) return 100;

        DateTime todaysDate = DateTime.Today;
        DateTime skippedDate = DateTime.Parse(PlayerPrefs.GetString("skippedDate"));

        Debug.LogError("difference : " + (int)(todaysDate - skippedDate).TotalDays);

        return (int)(todaysDate - skippedDate).TotalDays;
    }

    public void UpdateButtonFunction()
    {
        if (updateButton.transform.GetChild(0).GetComponent<Image>().sprite == errorSprite)
        {
            // quit application.
            Application.Quit();
        }
        else
        {
            // update game
            playerSelection.OnUpdateGameClick();
        }
    }
}