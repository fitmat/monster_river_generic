using System;
using UnityEngine;
#if UNITY_STANDALONE_WIN || UNITY_WDITOR
using yipli.Windows;
#endif

public static class YipliHelper
{
    private static string yipliAppBundleId = "com.yipli.app"; //todo: Change this later

    public static string userName = "bhansali.saurabh20@gmail.com";
    public static string password = "abcdefg123456789";

    public static int GetGameClusterId()
    {
        return InitBLE.getGameClusterID();
    }

    // based on player
    public static int GetGameClusterId(int playerID)
    {
        return InitBLE.getGameClusterID(playerID);
    }

    public static string GetFMDriverVersion()
    {
        return InitBLE.getFMDriverVersion();
    }

    public static void SetGameClusterId(int gameClusterId)
    {
        InitBLE.setGameClusterID(gameClusterId);
    }

    // for 2 players
    public static void SetGameClusterId(int p1gameClusterId, int p2gameClusterId)
    {
        InitBLE.setGameClusterID(p1gameClusterId, p2gameClusterId);
    }

    public static void SetGameMode(int gameMode)
    {
        Debug.Log("GameMode: " + gameMode);
        InitBLE.setGameMode(gameMode);
    }

    public static bool checkInternetConnection()
    {
        try
        {
            return PlayerSession.Instance.currentYipliConfig.bIsInternetConnected;
        }
        catch(Exception e )
        {
            Debug.LogError("Exception in check Internet : " + e.Message);
            return false;
        }
    }

    public static string GetMatConnectionStatus()
    {
        if(!PlayerSession.Instance.currentYipliConfig.onlyMatPlayMode)
            return "Connected";
        Debug.Log("GetBleConnectionStatus returning : " + InitBLE.getMatConnectionStatus());
        return InitBLE.getMatConnectionStatus();
    }


    public static void GoToPlaystoreUpdate(string gamePackageId)
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + gamePackageId);
#else
        Debug.Log("Unsupported os");
#endif
    }

    public static void GoToYipli(string direction = "NoDir", string gameID = "NoID")
    {
        // add ios part also
#if UNITY_ANDROID || UNITY_IOS

        switch(direction)
        {
            case ProductMessages.noMatCase:
                Debug.LogError("case : " + ProductMessages.noMatCase);
                Application.OpenURL(ProductMessages.AddMatAppPageUrl);
                break;

            case ProductMessages.noUserFound:
                Debug.LogError("case : " + ProductMessages.noUserFound);
                Application.OpenURL(ProductMessages.UserFoundAppPageUrl);
                break;

            case ProductMessages.noPlayerAdded:
                Debug.LogError("case : " + ProductMessages.noPlayerAdded);
                Application.OpenURL(ProductMessages.AddPlayerAppPageUrl);
                break;

            case ProductMessages.relaunchGame:
                Debug.LogError("case : " + ProductMessages.relaunchGame);
                // Application.OpenURL(ProductMessages.RelaunchGameUrl + Application.identifier); // provide full package id
                Application.OpenURL(ProductMessages.RelaunchGameUrl + gameID);
                break;

            case ProductMessages.openYipliApp:
                Debug.LogError("case : " + ProductMessages.openYipliApp);
                Application.OpenURL(ProductMessages.OpenYipliAppUrl);
                break;

            default:
                Debug.LogError("case : default");
                Application.OpenURL(ProductMessages.OpenYipliAppUrl);
                /*
                try
                {
                    AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

                    AndroidJavaObject launchIntent = null;
                    launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", yipliAppBundleId);
                    ca.Call("startActivity", launchIntent);
                }
                catch (AndroidJavaException e)
                {
                    Debug.Log(e);
                    Application.OpenURL("market://details?id=" + yipliAppBundleId);
                }
                */
                break;
        }
#elif UNITY_STANDALONE_WIN
        FileReadWrite.OpenYipliApp();
#else
        Debug.Log("Unsupported os");
#endif
    }


    //Returns true if YipliApp is installed, else returns false
    public static bool IsYipliAppInstalled()
    {
#if UNITY_EDITOR
        return true;
#elif UNITY_ANDROID
        AndroidJavaObject launchIntent = null;
        try
        {
            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
            Debug.Log(" Quering if Yipli App is installed.");
            
            //if the app is installed, no errors. Else, doesn't get past next line

            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", yipliAppBundleId);
        }
        catch (Exception ex)
        {
            Debug.Log("exception" + ex.Message);
        }
        if (launchIntent == null)
        {
            Debug.Log("Yipli app is not installed.");
            return false;
        }

        Debug.Log("Yipli App is Installed. Returning true.");
        return true;

#elif UNITY_STANDALONE_WIN || UNITY_EDITOR // TODO : Handle Windows flow
        Debug.Log("Yipli App validation for windows isnt required. Returning true");
        return FileReadWrite.IsYipliPcIsInstalled();
#else
        Debug.Log("OS not supported. Returnin false.");
        return false;
#endif
    }

    public static int convertGameVersionToBundleVersionCode(string gameVersion)
    {
        int versionCode;

        string[] strVersionCode = gameVersion.Split('.');

        string finalVersion = "";
        foreach (var word in strVersionCode)
        {
            finalVersion += word;
        }

        versionCode = int.Parse(finalVersion);

        //Debug.Log("Returning version Code : " + versionCode);

        return versionCode;
    }

    public static int StringToIntConvert(string text)
    {
        try
        {
            int convertedVal = int.Parse(text);
            return convertedVal;
        }
        catch (System.Exception e)
        {
            Debug.Log("String to int conversion error : " + e.Message);
            return 0;
        }
    }
}

