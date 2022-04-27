using UnityEngine;

[CreateAssetMenu(menuName = "TroubleShoot/SystemManager")]
public class TroubleShootManagerS : ScriptableObject
{
    public int currentAlgorithmID = -1;

    // process flags
    [Header("game question flags")]
    public bool osUpdateCheck = false;
    public bool playerFetchingCheckDone = false;
    public bool noMatPanelCheckDone = false;
    public bool internetConnectionTest = false;
    public bool matUsbConnectionTest = false;
    public bool phoneBleTest = false;
    public bool matInYipliAccountCheckDone = false;
    public bool backgroundAppsRunningCheckDone = false;
    public bool gamesAndAppUpdateCheckDone = false;
    public bool sameBehaviourGamesAsked = false;
    public bool sameBehaviourPlatformAsked = false;
    public bool behaviourRondomOrPersistentAsked = false;

    [Header("game answer flags")]
    public bool sameBehaviourGamessolutionProvided = false;
    public bool sameBehaviourPlatformsolutionProvided = false;
    public bool behaviourRondomOrPersistentProvided = false;
    public bool osUpdateSolutionProvided = false;

    [Header("mat question flags")]
    public bool matOnCheck = false;
    public bool colorOfLED = false;
    public bool charginglightVisibility = false;
    public bool bleListHasYipliCheckDone = false;
    public bool siliconDriverInstallCheck = false;
    public bool siliconPortAvailability = false;
    public bool matConnectionToOtherDeviceCheckDone = false;
    public bool sameMatFromYipliCheckDone = false;
    public bool isMatConnectedToOtherDeviceCheckDone = false;

    [Header("mat answer flags")]
    public bool isMatOnSolutionProvided = false;
    public bool redLedSolutionProvided = false;
    public bool sameMatFromYipliSolutionProvided = false;
    public bool isMatConnectedToOtherDeviceSolutionProvided = false;
    public bool chargingLightVisibilitySolutionProvided = false;

    [Header("Extra information")]
    public string bleScannedMacAddress = string.Empty;

    public int CurrentAlgorithmID { get => currentAlgorithmID; set => currentAlgorithmID = value; }
    public bool OsUpdateCheck { get => osUpdateCheck; set => osUpdateCheck = value; }
    public bool PlayerFetchingCheckDone { get => playerFetchingCheckDone; set => playerFetchingCheckDone = value; }
    public bool NoMatPanelCheckDone { get => noMatPanelCheckDone; set => noMatPanelCheckDone = value; }
    public bool InternetConnectionTest { get => internetConnectionTest; set => internetConnectionTest = value; }
    public bool MatUsbConnectionTest { get => matUsbConnectionTest; set => matUsbConnectionTest = value; }
    public bool PhoneBleTest { get => phoneBleTest; set => phoneBleTest = value; }
    public bool MatInYipliAccountCheckDone { get => matInYipliAccountCheckDone; set => matInYipliAccountCheckDone = value; }
    public bool BackgroundAppsRunningCheckDone { get => backgroundAppsRunningCheckDone; set => backgroundAppsRunningCheckDone = value; }
    public bool GamesAndAppUpdateCheckDone { get => gamesAndAppUpdateCheckDone; set => gamesAndAppUpdateCheckDone = value; }
    public bool SameBehaviourGamesAsked { get => sameBehaviourGamesAsked; set => sameBehaviourGamesAsked = value; }
    public bool SameBehaviourPlatformAsked { get => sameBehaviourPlatformAsked; set => sameBehaviourPlatformAsked = value; }
    public bool BehaviourRondomOrPersistentAsked { get => behaviourRondomOrPersistentAsked; set => behaviourRondomOrPersistentAsked = value; }
    public bool MatOnCheck { get => matOnCheck; set => matOnCheck = value; }
    public bool ColorOfLED { get => colorOfLED; set => colorOfLED = value; }
    public bool CharginglightVisibility { get => charginglightVisibility; set => charginglightVisibility = value; }
    public bool BleListHasYipliCheckDone { get => bleListHasYipliCheckDone; set => bleListHasYipliCheckDone = value; }
    public bool SiliconDriverInstallCheck { get => siliconDriverInstallCheck; set => siliconDriverInstallCheck = value; }
    public bool SiliconPortAvailability { get => siliconPortAvailability; set => siliconPortAvailability = value; }
    public bool MatConnectionToOtherDeviceCheckDone { get => matConnectionToOtherDeviceCheckDone; set => matConnectionToOtherDeviceCheckDone = value; }
    public bool SameMatFromYipliCheckDone { get => sameMatFromYipliCheckDone; set => sameMatFromYipliCheckDone = value; }
    public bool SameBehaviourGamessolutionProvided { get => sameBehaviourGamessolutionProvided; set => sameBehaviourGamessolutionProvided = value; }
    public bool SameBehaviourPlatformsolutionProvided { get => sameBehaviourPlatformsolutionProvided; set => sameBehaviourPlatformsolutionProvided = value; }
    public bool BehaviourRondomOrPersistentProvided { get => behaviourRondomOrPersistentProvided; set => behaviourRondomOrPersistentProvided = value; }
    public string BleScannedMacAddress { get => bleScannedMacAddress; set => bleScannedMacAddress = value; }
    public bool IsMatOnSolutionProvided { get => isMatOnSolutionProvided; set => isMatOnSolutionProvided = value; }
    public bool RedLedSolutionProvided { get => redLedSolutionProvided; set => redLedSolutionProvided = value; }
    public bool SameMatFromYipliSolutionProvided { get => sameMatFromYipliSolutionProvided; set => sameMatFromYipliSolutionProvided = value; }
    public bool IsMatConnectedToOtherDeviceCheckDone { get => isMatConnectedToOtherDeviceCheckDone; set => isMatConnectedToOtherDeviceCheckDone = value; }
    public bool IsMatConnectedToOtherDeviceSolutionProvided { get => isMatConnectedToOtherDeviceSolutionProvided; set => isMatConnectedToOtherDeviceSolutionProvided = value; }
    public bool OsUpdateSolutionProvided { get => osUpdateSolutionProvided; set => osUpdateSolutionProvided = value; }
    public bool ChargingLightVisibilitySolutionProvided { get => chargingLightVisibilitySolutionProvided; set => chargingLightVisibilitySolutionProvided = value; }

    public void ResetTroubleShootChecks()
    {
        CurrentAlgorithmID = -1;

        // game question flags
        OsUpdateCheck = false;
        PlayerFetchingCheckDone = false;
        NoMatPanelCheckDone = false;
        InternetConnectionTest = false;
        MatUsbConnectionTest = false;
        PhoneBleTest = false;
        MatInYipliAccountCheckDone = false;
        BackgroundAppsRunningCheckDone = false;
        GamesAndAppUpdateCheckDone = false;
        SameBehaviourGamesAsked = false;
        SameBehaviourPlatformAsked = false;
        BehaviourRondomOrPersistentAsked = false;

        // game answer flags
        SameBehaviourGamessolutionProvided = false;
        SameBehaviourPlatformsolutionProvided = false;
        BehaviourRondomOrPersistentProvided = false;

        // mat question flags
        MatOnCheck = false;
        ColorOfLED = false;
        CharginglightVisibility = false;
        BleListHasYipliCheckDone = false;
        SiliconDriverInstallCheck = false;
        SiliconPortAvailability = false;
        MatConnectionToOtherDeviceCheckDone = false;
        SameMatFromYipliCheckDone = false;
        IsMatConnectedToOtherDeviceCheckDone = false;

        // mat answer flags
        IsMatOnSolutionProvided = false;
        RedLedSolutionProvided = false;
        SameMatFromYipliSolutionProvided = false;
        IsMatConnectedToOtherDeviceSolutionProvided = false;

        // extra informations
        BleScannedMacAddress = string.Empty;
    }

    public string GetTroubleShootScriptableJson()
    {
        return JsonUtility.ToJson(this);
    }
}
