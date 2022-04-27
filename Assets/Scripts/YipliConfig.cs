using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;


public enum GameType
{
    FITNESS_GAMING,
    MULTIPLAYER_GAMING,
    ADV_GAMING
}


[CreateAssetMenu]
public class YipliConfig : ScriptableObject
{
    //[HideInInspector]
    public string callbackLevel;

    [HideInInspector]
    public YipliPlayerInfo playerInfo;

    [HideInInspector]
    public YipliMatInfo matInfo;

    //[HideInInspector]
    public string userId = "";
    
    public string gameId;

    //[HideInInspector]
    //public bool bIsMatIntroDoneForCurrentPlayer = false;

    [HideInInspector]
    public MP_GameStateManager MP_GameStateManager;

    [HideInInspector]
    public List<YipliPlayerInfo> allPlayersInfo;

    [HideInInspector]
    public List<YipliMatInfo> allMatsInfo;

    [HideInInspector]
    public DataSnapshot gameDataForCurrentPlayer;

    public bool bIsChangePlayerCalled = false;

    public bool onlyMatPlayMode = true;

    [HideInInspector]
    public bool bIsRetakeTutorialFlagActivated = false;

    //[HideInInspector]
    public bool bIsInternetConnected = true;

    [HideInInspector]
    public YipliInventoryGameInfo gameInventoryInfo;

    [HideInInspector]
    public YipliThisUserTicketInfo thisUserTicketInfo;

    [HideInInspector]
    public int oldFMResponseCount;

    [HideInInspector]
    public GameType gameType = GameType.FITNESS_GAMING;

    public bool troubleshootingPOSTDone = false;

    [HideInInspector]
    public DataSnapshot currentMatDetails;

    public string currentMatID = string.Empty;
    public string pId = string.Empty;

    public bool isDeviceAndroidTV = false;

    public bool skipNormalUpdateClicked = false;

    public string getMatUrlIn = string.Empty;

    public string getMatUrlUS = string.Empty;

    public bool allowMainGameSceneToLoad = false;
}
