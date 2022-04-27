using UnityEngine;
using Firebase.Database;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Firebase;
//using Firebase.Unity.Editor;
using Firebase.Storage;

public static class FirebaseDBHandler
{
    // Get a reference to the storage service, using the default Firebase App
    static Firebase.Storage.FirebaseStorage yipliStorage = Firebase.Storage.FirebaseStorage.DefaultInstance;
    public static Firebase.Storage.StorageReference profilepic_storage_ref = yipliStorage.GetReferenceFromUrl("gs://yipli-project.appspot.com/profile-pics/");

    private static string profilePicRootUrl = "gs://yipli-project.appspot.com/profile-pics/";

    static Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    private const string projectId = "yipli-project"; //Taken from Firebase project settings
    //private static readonly string databaseURL = "https://yipli-project.firebaseio.com/"; // Taken from Firebase project settings

    private static readonly string storagePath = "gs://yipli-project.appspot.com/game-setups/";

    public static string StoragePath => storagePath;

    public delegate void PostUserCallback();

    /* The function call to be allowed only if network is available */
    //public static async Task<YipliInventoryGameInfo> GetGameInfo(string gameId)
    //{
    //    YipliInventoryGameInfo gameInfo = new YipliInventoryGameInfo();
    //    DataSnapshot snapshot = null;
    //    try
    //    {
    //        Firebase.Auth.FirebaseUser newUser = await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password);
    //        Debug.LogFormat("Dummy User signed in successfully: {0} ({1})",
    //        newUser.DisplayName, newUser.UserId);

    //        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
    //        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    //        snapshot = await reference.Child("inventory/games").Child(gameId).GetValueAsync();
    //        gameInfo = new YipliInventoryGameInfo(snapshot);
    //    }
    //    catch (Exception exp)
    //    {
    //        Debug.Log("Failed to GetAllPlayerdetails : " + exp.Message);
    //    }

    //    return gameInfo ;
    //}

    ///* The function call to be allowed only if network is available */
    public static async Task<string> GetUserIdFromCode(string code)
    {
        string userId = "";
        DataSnapshot snapshot = null;
        try
        {
            //Firebase.Auth.FirebaseUser newUser = await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password);
            Firebase.Auth.FirebaseUser newUser = await auth.SignInAnonymouslyAsync();
            Debug.LogFormat("User signed in successfully: {0} ({1})",
            newUser.DisplayName, newUser.UserId);

            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            snapshot = await reference.Child("remoteCodes").Child(code).GetValueAsync();
            userId = snapshot.Child("user-id").Value.ToString();
        }
        catch (Exception exp)
        {
            Debug.Log("Failed to GetAllPlayerdetails : " + exp.Message);
        }

        return userId;
    }

    // Adds a PlayerSession to the Firebase Database
    public static void PostPlayerSession(PlayerSession session, PostUserCallback callback)
    {
        //auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password).ContinueWith(task => {
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

            Debug.Log("Pushing data to backend: " + JsonConvert.SerializeObject(session.GetPlayerSessionDataJsonDic()));

            string key = reference.Child("stage-bucket/player-sessions").Push().Key;
            reference.Child("stage-bucket/player-sessions").Child(key).SetRawJsonValueAsync(JsonConvert.SerializeObject(session.GetPlayerSessionDataJsonDic(), Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        });
    }

    // Adds a PlayerSession to the Firebase Database
    public static void PostMultiPlayerSession(PlayerSession session,PlayerDetails playerDetails,string mpSessionUUID, PostUserCallback callback)
    {
        //auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password).ContinueWith(task => {
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

            Debug.Log("Pushing data to backend: " + JsonConvert.SerializeObject(session.GetMultiPlayerSessionDataJsonDic(playerDetails, mpSessionUUID)));

            string key = reference.Child("stage-bucket/player-sessions").Push().Key;
            reference.Child("stage-bucket/player-sessions").Child(key).SetRawJsonValueAsync(JsonConvert.SerializeObject(session.GetMultiPlayerSessionDataJsonDic(playerDetails, mpSessionUUID), Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        });
    }

    //public static void ChangeCurrentPlayerInBackend(string strUserId, string strPlayerId, PostUserCallback callback)
    //{
    //    auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password).ContinueWith(task =>
    //    {
    //        if (task.IsCanceled)
    //        {
    //            Debug.LogError("SignInAnonymouslyAsync was canceled.");
    //            return;
    //        }
    //        if (task.IsFaulted)
    //        {
    //            Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
    //            return;
    //        }

    //        Firebase.Auth.FirebaseUser newUser = task.Result;
    //        Debug.LogFormat("User signed in successfully: {0} ({1})",
    //            newUser.DisplayName, newUser.UserId);

    //        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
    //        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

    //        reference.Child("profiles/users").Child(strUserId).Child("current-player-id").SetValueAsync(strPlayerId);
    //    });
    //}

    /* The function call to be allowed only if network is available */
    //public static async Task<DataSnapshot> GetGameData(string userId, string playerId, string gameId, PostUserCallback callback)
    //{
    //    DataSnapshot snapshot = null;
    //    if (userId.Equals(null) || playerId.Equals(null) || gameId.Equals(null))
    //    {
    //        Debug.Log("User ID not found");
    //    }
    //    else
    //    {
    //        try
    //        {
    //            Firebase.Auth.FirebaseUser newUser = await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password);
    //            Debug.LogFormat("User signed in successfully: {0} ({1})",
    //            newUser.DisplayName, newUser.UserId);

    //            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
    //            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    //            snapshot = await reference.Child("profiles/users/" + userId).Child("players").Child(playerId).Child("activity-statistics/games-statistics").Child(gameId).Child("game-data").GetValueAsync();
    //        }
    //        catch(Exception exp)
    //        {
    //            Debug.Log("Failed to GetGameData : " + exp.Message);
    //        }
    //    }
    //    return snapshot;
    //}

    /* The function call to be allowed only if network is available */
    //public static async Task<List<YipliPlayerInfo>> GetAllPlayerdetails(string userId, PostUserCallback callback)
    //{
    //    List<YipliPlayerInfo> players = new List<YipliPlayerInfo>();
    //    DataSnapshot snapshot = null;
    //    if (userId.Equals(null))
    //    {
    //        Debug.Log("User ID not found");
    //    }
    //    else
    //    {
    //        try
    //        {
    //            Firebase.Auth.FirebaseUser newUser = await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password);
    //            Debug.LogFormat("User signed in successfully: {0} ({1})",
    //            newUser.DisplayName, newUser.UserId);

    //            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
    //            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    //            snapshot = await reference.Child("profiles/users/" + userId).Child("players").GetValueAsync();

    //            foreach (var childSnapshot in snapshot.Children)
    //            {
    //                YipliPlayerInfo playerInstance = new YipliPlayerInfo(childSnapshot, childSnapshot.Key);
    //                if(playerInstance.playerId != null)
    //                {
    //                    players.Add(playerInstance);
    //                }
    //                else
    //                {
    //                    Debug.Log("Skipping this instance of player, backend seems corrupted.");
    //                }
    //            }
    //        }
    //        catch(Exception exp)
    //        {
    //            Debug.Log("Failed to GetAllPlayerdetails : " + exp.Message);
    //            return null;
    //        }
    //    }
    //    return players;
    //}

    ///* The function call to be allowed only if network is available */
    //public static async Task<YipliPlayerInfo> GetCurrentPlayerdetails(string userId, PostUserCallback callback)
    //{
    //    Debug.Log("Getting the Default player from backend");

    //    DataSnapshot snapshot = null;
    //    YipliPlayerInfo defaultPlayer = new YipliPlayerInfo();//Cant return null defaultPlayer. Initialze the default player.

    //    if (userId.Equals(null) || userId.Equals(""))
    //    {
    //        Debug.Log("User ID not found");
    //    }
    //    else
    //    {
    //        try
    //        {
    //            Firebase.Auth.FirebaseUser newUser = await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password);
    //            Debug.LogFormat("User signed in successfully: {0} ({1})",
    //            newUser.DisplayName, newUser.UserId);

    //            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
    //            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

    //            //First get the current player id from user Id
    //            snapshot = await reference.Child("profiles/users").Child(userId).GetValueAsync();
    //            string playerId = snapshot.Child("current-player-id").Value?.ToString() ?? "";

    //            //Now get the complete player details from Player Id
    //            DataSnapshot defaultPlayerSnapshot = await reference.Child("profiles/users/" + userId + "/players/" + playerId).GetValueAsync();

    //            defaultPlayer = new YipliPlayerInfo(defaultPlayerSnapshot, defaultPlayerSnapshot.Key);

    //            if (defaultPlayer.playerId != null)
    //            {
    //                //Do Nothing
    //                Debug.Log("Found Default player : " + defaultPlayer.playerId);
    //            }
    //            else
    //            {
    //                //Case to handle if the default player object doesnt exist in backend/or is corrupted
    //                Debug.Log("Default Player Not found. Returning null.");
    //                return null;
    //            }
    //        }
    //        catch(Exception exp)
    //        {
    //            //If couldnt get defualt player details from the backend, return null.
    //            Debug.Log("Failed to GetAllPlayerdetails: " + exp.Message);
    //            return null;
    //        }
    //    }

    //    return defaultPlayer;
    //}


    // Mat related queries

    /* The function call to be allowed only if network is available */
    //public static async Task<YipliMatInfo> GetCurrentMatDetails(string userId, PostUserCallback callback)
    //{
    //    Debug.Log("Getting the Default mat from backend");
    //    DataSnapshot snapshot = null;
    //    YipliMatInfo defaultMat = new YipliMatInfo();

    //    if (userId.Equals(null) || userId.Equals(""))
    //    {
    //        Debug.Log("User ID not found");
    //    }
    //    else
    //    {
    //        try
    //        {
    //            Firebase.Auth.FirebaseUser newUser = await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password);
    //            Debug.LogFormat("User signed in successfully: {0} ({1})",
    //            newUser.DisplayName, newUser.UserId);

    //            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
    //            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

    //            //First get the current mat id from user Id
    //            snapshot = await reference.Child("profiles/users").Child(userId).GetValueAsync();

    //            string matId = snapshot.Child("current-mat-id").Value?.ToString() ?? "";
    //            //Now get the complete player details from Player Id
    //            DataSnapshot defaultMatSnapshot = await reference.Child("profiles/users/" + userId + "/mats/" + matId).GetValueAsync();
    //            defaultMat = new YipliMatInfo(defaultMatSnapshot, defaultMatSnapshot.Key);

    //            if (defaultMat.matId != null)
    //            {
    //                //Do Nothing
    //                Debug.Log("Found Default mat : " + defaultMat.matId);
    //            }
    //            else
    //            {
    //                //Case to handle if the default mat object doesnt exist in backend/or is corrupted
    //                return null;
    //            }
    //        }
    //        catch(Exception exp)
    //        {
    //            Debug.Log("Failed to GetAllMatdetails : " + exp.Message);
    //            return null;
    //        }
    //    }
    //    return defaultMat;
    //}

    /* The function call to be allowed only if network is available */
    //public static async Task<List<YipliMatInfo>> GetAllMatDetails(string userId, PostUserCallback callback)
    //{
    //    List<YipliMatInfo> mats = new List<YipliMatInfo>();
    //    DataSnapshot snapshot = null;
    //    if (userId.Equals(null) || userId.Equals(""))
    //    {
    //        Debug.Log("User ID not found");
    //    }
    //    else
    //    {
    //        try
    //        {
    //            Firebase.Auth.FirebaseUser newUser = await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password);
    //            Debug.LogFormat("User signed in successfully: {0} ({1})",
    //            newUser.DisplayName, newUser.UserId);

    //            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
    //            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    //            snapshot = await reference.Child("profiles/users/" + userId + "/mats").GetValueAsync();

    //            foreach (var childSnapshot in snapshot.Children)
    //            {
    //                YipliMatInfo matInstance = new YipliMatInfo(childSnapshot, childSnapshot.Key);
    //                if(matInstance.matId != null)
    //                {
    //                    mats.Add(matInstance);
    //                }
    //                else
    //                {
    //                    Debug.Log("Skipping this instance of mat, backend seems corrupted.");
    //                }
    //            }
    //        }
    //        catch(Exception exp)
    //        {
    //            Debug.Log("Failed to GetAllPlayerdetails : " + exp.Message);
    //        }
    //    }
    //    return mats;
    //}


    /*
     * profilePicUrl : Player profile pic property stored already 
     * onDeviceProfilePicPath : Path to store the image locally
     */
    public static async Task<Sprite> GetImageAsync(string profilePicUrl, string onDeviceProfilePicPath)
    {
        Debug.Log("Local path : " + onDeviceProfilePicPath);

        // Get a reference to the storage service, using the default Firebase App
        Firebase.Storage.StorageReference storage_ref = yipliStorage.GetReferenceFromUrl(profilePicRootUrl + profilePicUrl);

        Debug.Log("File download started.");

        try
        {
            // Start downloading a file and store it at local_url path
            await storage_ref.GetFileAsync(onDeviceProfilePicPath);
            byte[] bytes = System.IO.File.ReadAllBytes(onDeviceProfilePicPath);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            
            Debug.Log("Profile image downloaded.");
            return sprite;
        }
        catch (Exception exp)
        {
            Debug.Log("Failed to download Profile image : " + exp.Message);
            return null;
        }
    }

    /* The function to store game data to backend without gamePlay. 
    * This is to be called by your games shop manager module.*/
    public static async void UpdateStoreData(string strUserId, string strPlayerId, string strGameId, Dictionary<string, object> dStoreData, PostUserCallback callback)
    {
        //await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password).ContinueWith(async task =>
        await auth.SignInAnonymouslyAsync().ContinueWith(async task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            await reference.Child("fgd/" + strUserId).Child(strPlayerId).Child(strGameId).UpdateChildrenAsync(dStoreData);
        });
    }

    /* The function to store mat tutorial status to backend. */
    public static async Task<int> GetMatTutorialStatus(string strUserId, string strPlayerId)
    {
        int tutStatus = -1;
        DataSnapshot snapshot = null;
        try
        {
            //Firebase.Auth.FirebaseUser newUser = await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password);
            Firebase.Auth.FirebaseUser newUser = await auth.SignInAnonymouslyAsync();
            Debug.LogFormat("User signed in successfully: {0} ({1})",
            newUser.DisplayName, newUser.UserId);

            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            snapshot = await reference.Child("/profiles/users/").Child(strUserId).Child("players").Child(strPlayerId).Child("mat-tut-done").GetValueAsync();
            tutStatus = snapshot.Value == null ? 0 : (int)snapshot.Value;
        }
        catch (Exception exp)
        {
            Debug.LogError("Failed to tutstatus : " + exp.Message);
        }

        return tutStatus;
    }

    /* The function to store mat tutorial status to backend. */
    public static async void UpdateTutStatusData(string strUserId, string strPlayerId, int tutStatus)
    {
        //await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password).ContinueWith(async task =>
        await auth.SignInAnonymouslyAsync().ContinueWith(async task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            await reference.Child("/profiles/users/").Child(strUserId).Child("players").Child(strPlayerId).Child("mat-tut-done").SetValueAsync(tutStatus);
        });
    }

    /* The function call to be allowed only if network is available 
       Get yipli pc app url from backend */
    public static async Task<string> GetYipliWinAppUpdateUrl()
    {
        Uri appUpdateUrl = null;
        try
        {
            StorageReference reference = yipliStorage.GetReferenceFromUrl(StoragePath + "Yipli_Setup.exe");
            appUpdateUrl = await reference.GetDownloadUrlAsync();
        }
        catch (Exception exp)
        {
            Debug.Log("Failed to Get app win version : " + exp.Message);
        }

        return appUpdateUrl.ToString();
    }

    /* The function call to be allowed only if network is available 
       Get yipli pc app url from backend */
    public static async Task<string> UploadLogsFileToDB(string userID, List<string> fileNames, List<string> filePaths)
    {
        StorageReference storageRef = yipliStorage.RootReference;

        string storageChildRef = "customer-tickets/" + userID + "/" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + "/" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "/";

        for (int i = 0; i < fileNames.Count; i++)
        {
            StorageReference fmResponseLogRef = storageRef.Child(storageChildRef + fileNames[i]);

            await fmResponseLogRef.PutFileAsync(filePaths[i]).ContinueWith((Task<StorageMetadata> task) => {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log(task.Exception.ToString());
                    // Uh-oh, an error occurred!
                }
                else
                {
                    // Metadata contains file metadata such as size, content-type, and download URL.
                    StorageMetadata metadata = task.Result;
                    string md5Hash = metadata.Md5Hash;
                    Debug.Log("Finished uploading...");
                    Debug.Log("md5 hash = " + md5Hash);
                }
            });
        }

        return storageChildRef;
    }

    // only foir IOS get mac address from fb
    public static async Task<string> GetMacAddressFromMatIDAsync(string MatID)
    {
        string macAddress = string.Empty;
        DataSnapshot snapshot = null;
        try
        {
            //Firebase.Auth.FirebaseUser newUser = await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password);
            Firebase.Auth.FirebaseUser newUser = await auth.SignInAnonymouslyAsync();
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            snapshot = await reference.Child("/inventory/mats/").Child(MatID).Child("mac-address").GetValueAsync();
            macAddress = snapshot.Value.ToString();
        }
        catch (Exception exp)
        {
            Debug.LogError("Failed to tutstatus : " + exp.Message);
        }

        //return macAddress;

        return "A4:DA:32:4F:C2:54";
    }

    /* The function to store mat tutorial status to backend. */
    public static async void SetTicketData(string strUserId, string ticketID, Dictionary<string, object> ticketData)
    {
        //await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password).ContinueWith(async task =>
        await auth.SignInAnonymouslyAsync().ContinueWith(async task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            await reference.Child("/customer-tickets/").Child(strUserId).Child(ticketID).SetValueAsync(ticketData);
        });
    }

    /* The function to store ticket data to backend for this user. 
    * This is to be called by your games shop manager module.*/
    public static async void UpdateCurrentTicketData(string strUserId, Dictionary<string, object> ticketData, PostUserCallback callback)
    {
        //await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password).ContinueWith(async task =>
        await auth.SignInAnonymouslyAsync().ContinueWith(async task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            await reference.Child("customer-tickets/" + strUserId).Child("open/current_tkt/").UpdateChildrenAsync(ticketData);
        });
    }

    //Get Email from user id
    /* The function call to be allowed only if network is available */
    public static async Task<string> GetEmailFromUserID(string receivedUserID)
    {
        string email = "";
        DataSnapshot snapshot = null;
        try
        {
            Firebase.Auth.FirebaseUser newUser = await auth.SignInAnonymouslyAsync();
            Debug.LogFormat("User signed in successfully: {0} ({1})",
            newUser.DisplayName, newUser.UserId);

            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            snapshot = await reference.Child("profiles/users").Child(receivedUserID).Child("email").GetValueAsync();

            //Debug.LogError("Player's email : " + snapshot.Value);

            email = snapshot.Value.ToString();
        }
        catch (Exception exp)
        {
            Debug.LogError("Failed to GetPlayer's email : " + exp.Message);
        }

        return email;
    }

    // get user's current mat details
    public static async Task<DataSnapshot> GetMatDetailsOfUserId(string userID, string currentMatID)
    {
        DataSnapshot snapshot = null;
        try
        {
            //Firebase.Auth.FirebaseUser newUser = await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password);
            Firebase.Auth.FirebaseUser newUser = await auth.SignInAnonymouslyAsync();
            Debug.LogFormat("User signed in successfully: {0} ({1})",
            newUser.DisplayName, newUser.UserId);

            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

            snapshot = await reference.Child("profiles/users").Child(userID).Child("mats").Child(currentMatID).GetValueAsync();
        }
        catch (Exception exp)
        {
            Debug.Log("Failed to GetMatDetailsOfUserId : " + exp.Message);
        }

        return snapshot;
    }

    public static async Task<string> GetCurrentMatIdOfUserId(string userID)
    {
        string currentMatID = null;
        DataSnapshot snapshot = null;
        try
        {
            //Firebase.Auth.FirebaseUser newUser = await auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password);
            Firebase.Auth.FirebaseUser newUser = await auth.SignInAnonymouslyAsync();
            Debug.LogFormat("User signed in successfully: {0} ({1})",
            newUser.DisplayName, newUser.UserId);

            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
            snapshot = await reference.Child("profiles/users").Child(userID).Child("current-mat-id").GetValueAsync();
            currentMatID = snapshot.Value.ToString();
        }
        catch (Exception exp)
        {
            Debug.Log("Failed to GetCurrentMatIdOfUserId : " + exp.Message);
        }

        return currentMatID;
    }

    //************************ Test Harness code. Do Not modify (- Saurabh) ***************************
    // Adds a PlayerSession to the Firebase Database
    public static void _T_PostDummyPlayerSession(Dictionary<string, dynamic> tempData)
    {
        //auth.SignInWithEmailAndPasswordAsync(YipliHelper.userName, YipliHelper.password).ContinueWith(task => {
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://yipli-project.firebaseio.com/");
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

            Debug.Log("Pushing data to backend: " + JsonConvert.SerializeObject(tempData));

            string key = reference.Child("stage-bucket/player-sessions").Push().Key;
            reference.Child("stage-bucket/player-sessions").Child(key).SetRawJsonValueAsync(JsonConvert.SerializeObject(tempData, Formatting.None, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));    
        });
    }
}
