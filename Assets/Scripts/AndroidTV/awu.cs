/* This script is not in use for now. Do not delete this script. */
using UnityEngine;
public static class AndroidEnvInfo
{
    public static bool IsAndroidTV
    {
        get
        {
#if UNITY_EDITOR || !UNITY_ANDROID
            Debug.Log("Android TV : from #if and returning after next line");
            return false;
#else
            Debug.Log("Android TV : from #else");
            AndroidJavaClass connectionClass = new AndroidJavaClass("rixment.awu.Main");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            Debug.Log("Android TV : next line is return isAndrodTv flag.");
            return connectionClass.CallStatic<bool>("isAndroidTv", currentActivity);
#endif
        }
    }
//    public static bool IsConnectedViaCellular
//    {
//        get
//        {
//#if UNITY_EDITOR || !UNITY_ANDROID
//            return false;
//#else
//        AndroidJavaClass connectionClass = new AndroidJavaClass("rixment.awu.Main");
//        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
//        return connectionClass.CallStatic<bool>("isConnectedViaCellular", currentActivity);
//#endif
//        }
//    }
//    public static bool IsConnectedViaWifi
//    {
//        get
//        {
//#if UNITY_EDITOR || !UNITY_ANDROID
//            return false;
//#else
//        AndroidJavaClass connectionClass = new AndroidJavaClass("rixment.awu.Main");
//        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
//        return connectionClass.CallStatic<bool>("IsConnectedViaWifi", currentActivity);
//#endif
//        }
//    }
}