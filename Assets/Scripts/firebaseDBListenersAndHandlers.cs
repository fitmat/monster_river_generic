using Firebase.Database;
//using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.DynamicLinks;
using GameLib.UI.PanelManagers;

public enum QueryStatus
{
    NotStarted,
    InProgress,
    Completed
};

public class firebaseDBListenersAndHandlers : MonoBehaviour
{
    public YipliConfig currentYipliConfig;

    //Track if the query exection is completed or not
    private static QueryStatus getAllPlayersQureyStatus = global::QueryStatus.NotStarted;

    //Track if the query exection is completed or not
    private static QueryStatus getDefaultMatQueryStatus = global::QueryStatus.NotStarted;

    //Track if the query exection is completed or not
    private static QueryStatus getGameInfoQueryStatus = global::QueryStatus.NotStarted;

    //Track if the query exection is completed or not
    private static QueryStatus getThisUserTicketInfoQueryStatus = global::QueryStatus.NotStarted;

    //Track if the query exection is completed or not
    private static QueryStatus getGameDataForCurrentPlayerQueryStatus = global::QueryStatus.NotStarted;

    //Track if the query exection is completed or not
    private static QueryStatus getGameBlockDataForCurrentPlayerQueryStatus = global::QueryStatus.NotStarted;

    //Track if the query exection is completed or not
    private static QueryStatus getAllMatsQureyStatus = global::QueryStatus.NotStarted;

    public static QueryStatus GetGameBlockDataForCurrentPlayerQueryStatus { get => getGameBlockDataForCurrentPlayerQueryStatus; set => getGameBlockDataForCurrentPlayerQueryStatus = value; }
    public static QueryStatus GetThisUserTicketInfoQueryStatus { get => getThisUserTicketInfoQueryStatus; set => getThisUserTicketInfoQueryStatus = value; }
    public static QueryStatus GetAllMatsQureyStatus { get => getAllMatsQureyStatus; set => getAllMatsQureyStatus = value; }

    public static bool dynamicLinkIsReceived = false;

    // delegates and events
    public delegate void OnUrlsFound();
    public static event OnUrlsFound allUrlsFound;

    public static QueryStatus GetGameDataForCurrenPlayerQueryStatus()
    {
        return getGameDataForCurrentPlayerQueryStatus;
    }

    public static void SetGameDataForCurrenPlayerQueryStatus(QueryStatus queryStatus)
    {
        getGameDataForCurrentPlayerQueryStatus = queryStatus;
    }

    public static QueryStatus GetPlayersQueryStatus()
    {
        return getAllPlayersQureyStatus;
    }

    public static QueryStatus GetMatQueryStatus()
    {
        return getDefaultMatQueryStatus;
    }

    public static QueryStatus GetGameInfoQueryStatus()
    {
        return getGameInfoQueryStatus;
    }

    private void Start() {
        allUrlsFound();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        allUrlsFound += addGetAllURLListener;

        // Add listener for Dynamic links
        #if UNITY_ANDROID || UNITY_IOS
        DynamicLinks.DynamicLinkReceived += ExtractUserDetailsFromLink;
        #endif

        //Add listeners to the Firebase backend for all the DB Calls
        Debug.Log("Add listeners to the Firebase backend for all the DB Calls");
        PlayerSelection.NewUserFound += addGetPlayersListener;
        PlayerSelection.GetAllMats += addGetAllMatsListener;

#if UNITY_ANDROID || UNITY_IOS
        PlayerSelection.NewUserFound += addDefaultMatIdListener;
        PlayerSession.NewMatFound += addDefaultMatIdListener;
#endif
        PlayerSelection.DefaultPlayerChanged += addGameDataListener;
        PlayerSelection.GetGameInfo += addListnerForGameInfo;

        PlayerSelection.TicketData += addListnerForThisUserTicketDataInfo;

        StartCoroutine(TrackNetworkConnectivity());
    }

    private  async void addListnerForGameInfo()
    {
        Debug.Log("addGetPlayersListener invoked");
        await anonAuthenticate();
        FirebaseDatabase.DefaultInstance
        .GetReference("inventory/games/" + currentYipliConfig.gameId)
        .ValueChanged += HandleGameInfoValueChanged;
    }

    private void HandleGameInfoValueChanged(object sender, ValueChangedEventArgs e)
    {
        getGameInfoQueryStatus = global::QueryStatus.InProgress;
        if(e.Snapshot.Value != null)
        {
            currentYipliConfig.gameInventoryInfo = new YipliInventoryGameInfo(e.Snapshot);
        }
        else
        {
            Debug.Log("onlyMatPlayMode : Invalid Game. Nothing found at specified path.");
        }
        getGameInfoQueryStatus = global::QueryStatus.Completed;
    }

    private IEnumerator TrackNetworkConnectivity()
    {
        //yield return new WaitForSecondsRealtime(8f);

        yield return anonAuthenticate();
        FirebaseDatabase.DefaultInstance.GetReference(".info/connected").ValueChanged += HandleConnectedChanged;

/*
#if UNITY_ANDROID || UNITY_IOS
        FirebaseDatabase.DefaultInstance.GetReference(".info/connected").ValueChanged += HandleConnectedChanged;
#else
        StartCoroutine(CheckPingResult());
#endif
*/
    }

    private void HandleConnectedChanged(object sender, ValueChangedEventArgs e)
    {
        Debug.Log("Network : " + e.Snapshot.Value);
        currentYipliConfig.bIsInternetConnected = e.Snapshot.Value.Equals(true);

        //FindObjectOfType<NoInternetPanelManager>().ManageNoInternetPanel();
    }

/*
    private IEnumerator CheckPingResult() {
        while(true) {
            yield return new WaitForSecondsRealtime(1f);
            
            if (Application.internetReachability == NetworkReachability.NotReachable) {
                Debug.Log("Network from if : " + Application.internetReachability);
                currentYipliConfig.bIsInternetConnected =false;
            } else {
                Debug.Log("Network from else : " + Application.internetReachability);
                currentYipliConfig.bIsInternetConnected = true;
            }
        }
    }
*/

    void OnDisable()
    {
        allUrlsFound -= addGetAllURLListener;

        //Remove the events to avoid memory leaks
        PlayerSelection.NewUserFound -= addGetPlayersListener;
        PlayerSelection.GetAllMats -= addGetAllMatsListener;
        PlayerSelection.DefaultPlayerChanged -= addGameDataListener;
        PlayerSelection.GetGameInfo -= addListnerForGameInfo;

#if UNITY_ANDROID
        PlayerSelection.NewUserFound -= addDefaultMatIdListener;
        PlayerSession.NewMatFound -= addDefaultMatIdListener;
#endif
        PlayerSelection.TicketData -= addListnerForThisUserTicketDataInfo;
    }

    private async void addDefaultMatIdListener()
    {
        Debug.Log("addDefaultMatIdListener invoked");
        await anonAuthenticate();
        FirebaseDatabase.DefaultInstance
        .GetReference("profiles/users/" + currentYipliConfig.userId + "/current-mat-id")
        .ValueChanged += HandleCurrentMatIdValueChanged;
    }

    private async void addDefaultMatInfoListener(string matId)
    {
        Debug.Log("addDefaultMatInfoListener invoked");
        await anonAuthenticate();
        FirebaseDatabase.DefaultInstance
        .GetReference("profiles/users/" + currentYipliConfig.userId + "/mats/" + matId)
        .ValueChanged += HandleCurrentMatInfoValueChanged;
    }

    private void HandleCurrentMatInfoValueChanged(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("HandleCurrentMatInfoValueChanged invoked");
        if(args.Snapshot.Value != null)
            currentYipliConfig.matInfo = new YipliMatInfo(args.Snapshot, args.Snapshot.Key);
        getDefaultMatQueryStatus = global::QueryStatus.Completed;
    }

    private void HandleCurrentMatIdValueChanged(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("HandleCurrentMatIdValueChanged invoked");
        getDefaultMatQueryStatus = global::QueryStatus.InProgress;
        //args.Snapshot has mat-Id for default mat.
        string matId = args.Snapshot.ToString();
        addDefaultMatInfoListener(matId);
    }

    private async Task anonAuthenticate()
    {
        Debug.Log("Syncing data from the Firebase backend");
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        //Firebase.Auth.FirebaseUser newUser = await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password);
        Firebase.Auth.FirebaseUser newUser = await auth.SignInAnonymouslyAsync();
        Debug.LogFormat("Dummy user signed in successfully: {0} ({1})",
        newUser.DisplayName, newUser.UserId);

        //Firebase.FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private async void addGetPlayersListener()
    {
        Debug.Log("addGetPlayersListener invoked");
        await anonAuthenticate();
        FirebaseDatabase.DefaultInstance
        .GetReference("profiles/users/" + currentYipliConfig.userId + "/players")
        .ValueChanged += HandleAllPlayersDataValueChanged;
    }

    void HandleAllPlayersDataValueChanged(object sender, ValueChangedEventArgs args)
    {
        getAllPlayersQureyStatus = global::QueryStatus.InProgress;
        Debug.Log("HandleAllPlayersDataValueChanged invoked");
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        //bool isDefaultPlayerPresent = false;
        //bool isSavedPlayerInfoAvailabe = currentYipliConfig.playerInfo == null ? false : true;

        currentYipliConfig.allPlayersInfo = new List<YipliPlayerInfo>();

        foreach (var childSnapshot in args.Snapshot.Children)
        {
            YipliPlayerInfo playerInstance = new YipliPlayerInfo(childSnapshot, childSnapshot.Key);
            if (playerInstance.playerId != null)
            {
                currentYipliConfig.allPlayersInfo.Add(playerInstance);

                // if (isSavedPlayerInfoAvailabe && playerInstance.playerId.Equals(currentYipliConfig.playerInfo.playerId))
                // {
                //     isDefaultPlayerPresent = true;
                // }
            }
            else
            {
                Debug.Log("Skipping this instance of player, backend seems corrupted.");
            }
        }

        /*
        if (currentYipliConfig.gameType != GameType.MULTIPLAYER_GAMING && (!isDefaultPlayerPresent || !isSavedPlayerInfoAvailabe))
        {
            Debug.Log("Removing saved player as it don't exist.");
            UserDataPersistence.ClearDefaultPlayer(currentYipliConfig);

            if (PlayerSession.Instance != null)
            {
                PlayerSession.Instance.ChangePlayer();
            }
        }
        */

        Debug.Log("All players data got successfully.");
        getAllPlayersQureyStatus = global::QueryStatus.Completed;
    }

    private async void addGameDataListener()
    {
        //Debug.Log("addGameDataListener invoked");
        await anonAuthenticate();

        //Debug.LogError("player id : " + currentYipliConfig.playerInfo.playerId);

        if (!currentYipliConfig.gameId.Equals("default") || currentYipliConfig.gameId.Length > 1)
            FirebaseDatabase.DefaultInstance
            .GetReference("fgd/" + currentYipliConfig.userId + "/" + currentYipliConfig.playerInfo.playerId + "/" + currentYipliConfig.gameId)
            .ValueChanged += HandleGameDataValueChanged;
    }

    void HandleGameDataValueChanged(object sender, ValueChangedEventArgs args)
    {
        getGameDataForCurrentPlayerQueryStatus = global::QueryStatus.InProgress;
        Debug.Log("HandleGameDataValueChanged invoked");
        currentYipliConfig.gameDataForCurrentPlayer = args.Snapshot;
        getGameDataForCurrentPlayerQueryStatus = global::QueryStatus.Completed;
    }

    // ticket data info
    private async void addListnerForThisUserTicketDataInfo()
    {
        Debug.Log("addListnerForThisUserTicketDataInfo invoked");
        await anonAuthenticate();
        //FirebaseDatabase.DefaultInstance.GetReference("customer-tickets/" + currentYipliConfig.userId + "/").ValueChanged += HandleThisUserTicketDataInfoValueChanged;
        FirebaseDatabase.DefaultInstance.GetReference("customer-tickets/")
            .Child(currentYipliConfig.userId).Child("open/current_tkt").ValueChanged += HandleThisUserTicketDataInfoValueChanged;
    }

    private void HandleThisUserTicketDataInfoValueChanged(object sender, ValueChangedEventArgs e)
    {
        GetThisUserTicketInfoQueryStatus = global::QueryStatus.InProgress;

        currentYipliConfig.thisUserTicketInfo = new YipliThisUserTicketInfo(e.Snapshot);

        GetThisUserTicketInfoQueryStatus = global::QueryStatus.Completed;
    }

    // get all the mats od the received user ID
    private async void addGetAllMatsListener()
    {
        Debug.Log("addGetAllMatsListener invoked");
        await anonAuthenticate();
        FirebaseDatabase.DefaultInstance
        .GetReference("profiles/users/" + currentYipliConfig.userId + "/mats")
        .ValueChanged += HandleAllMatsDataValueChanged;
    }

    void HandleAllMatsDataValueChanged(object sender, ValueChangedEventArgs args)
    {
        GetAllMatsQureyStatus = global::QueryStatus.InProgress;
        Debug.Log("HandleAllMatsDataValueChanged invoked");
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        currentYipliConfig.allMatsInfo = new List<YipliMatInfo>();

        foreach (var childSnapshot in args.Snapshot.Children)
        {
            YipliMatInfo matInstance = new YipliMatInfo(childSnapshot, childSnapshot.Key);
            if (matInstance.matId != null)
            {
                currentYipliConfig.allMatsInfo.Add(matInstance);                
            }
            else
            {
                Debug.Log("Skipping this instance of mat, backend seems corrupted.");
            }
        }

        Debug.Log("All mats data got successfully.");
        GetAllMatsQureyStatus = global::QueryStatus.Completed;
    }

    // dyanamic link function on received
    private void ExtractUserDetailsFromLink(object sender, EventArgs args)
    {
        if (!dynamicLinkIsReceived) {
            dynamicLinkIsReceived = true;

            var dynamicLinkEventArgs = args as ReceivedDynamicLinkEventArgs;
            string dynamicLinkOrig = dynamicLinkEventArgs.ReceivedDynamicLink.Url.OriginalString;
            Debug.Log("Deep link : Received dynamic link : " + dynamicLinkOrig);

            dynamicLinkOrig.Remove(0, dynamicLinkOrig.IndexOf("?"));

            int questionMarkIndex = dynamicLinkOrig.IndexOf("?");

            string stringToParse = dynamicLinkOrig.Substring(questionMarkIndex + 1, dynamicLinkOrig.Length - 1 - questionMarkIndex);

            string[] dataSets = stringToParse.Split('&');

            for (int i = 0; i < dataSets.Length; i++)
            {
                string[] tempSplits = dataSets[i].Split('=');

                switch (tempSplits[0])
                {
                    case "uId":
                        currentYipliConfig.userId = tempSplits[1];
                        break;

                    case "pId":
                        currentYipliConfig.pId = tempSplits[1];
                        break;

                    case "tv":
                        if (YipliHelper.StringToIntConvert(tempSplits[1]) == 1) {
                            currentYipliConfig.isDeviceAndroidTV = true;
                        } else {
                            currentYipliConfig.isDeviceAndroidTV = false;
                        }
                        break;

                    default:
                        Debug.LogError("Wrong data set field : " + tempSplits[0]);
                        break;
                }
            }

            FindObjectOfType<PlayerSelection>().SetLinkData();
        }
    }

    /*
    // user details link managemenhet
    private void ExtractUserDetailsFromLink(object sender, EventArgs args)
    {
        Debug.Log("Deep link is received : " + dataSetsAreFilled);

        if (!dataSetsAreFilled)
        {
            var dynamicLinkEventArgs = args as ReceivedDynamicLinkEventArgs;
            string dynamicLinkOrig = dynamicLinkEventArgs.ReceivedDynamicLink.Url.OriginalString;
            Debug.Log("Deep link : Received dynamic link : " + dynamicLinkOrig);

            dynamicLinkOrig.Remove(0, dynamicLinkOrig.IndexOf("?"));

            int questionMarkIndex = dynamicLinkOrig.IndexOf("?");

            string stringToParse = dynamicLinkOrig.Substring(questionMarkIndex + 1, dynamicLinkOrig.Length - 1 - questionMarkIndex);

            string[] dataSets = stringToParse.Split('&');

            for (int i = 0; i < dataSets.Length; i++)
            {
                string[] tempSplits = dataSets[i].Split('=');

                switch (tempSplits[0])
                {
                    case "uId":
                        uId = tempSplits[1];
                        currentYipliConfig.userId = uId;
                        break;

                    case "pId":
                        currentYipliConfig.pId = tempSplits[1];
                        break;

                    case "pName":
                        pName = tempSplits[1];
                        break;

                    case "pDOB":
                        pDOB = tempSplits[1];
                        break;

                    case "pWt":
                        pWt = tempSplits[1];
                        break;

                    case "pHt":
                        pHt = tempSplits[1];
                        break;

                    case "pPicUrl":
                        pPicUrl = tempSplits[1];
                        break;

                    case "mId":
                        mId = tempSplits[1];
                        break;

                    case "mMac":
                        mMac = tempSplits[1];
                        break;

                    case "mName":
                        mName = tempSplits[1];
                        break;

                    case "pTutDone":
                        pTutDone = tempSplits[1];
                        break;

                    default:
                        Debug.LogError("Wrong data set field : " + tempSplits[0]);
                        break;
                }
            }

            dataSetsAreFilled = true;
            Debug.Log("Deep link : Data sets are filled : " + dataSetsAreFilled);

            //SetLinkData();
        }

        //Debug.Log("Received dynamic link Argumanets : " + stringToParse);

        // old project api key : AIzaSyAceBtIqNZdErggHnZDuU12DfYrbBhe-T4
        // new project api key : AIzaSyAKKmLL2iDQRSNMRfToAxDJio7yPjx2NPE
    }
    */

    // achieve all required urls
    private async void addGetAllURLListener()
    {
        Debug.Log("addGetAllMatsListener invoked");
        await anonAuthenticate();
        FirebaseDatabase.DefaultInstance
        .GetReference("inventory/yipli-app/urls")
        .ValueChanged += HandleAllURLDataValueChanged;
    }

    void HandleAllURLDataValueChanged(object sender, ValueChangedEventArgs args)
    {
        Debug.Log("HandleAllURLDataValueChanged invoked");
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        currentYipliConfig.getMatUrlIn = args.Snapshot.Child("get-mat-in").Value.ToString();
        currentYipliConfig.getMatUrlUS = args.Snapshot.Child("get-mat-us").Value.ToString();

        Debug.Log("All url data got successfully.");
    }
}