/* This script is not in use for now. Do not delete this script. */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DetectAndroidTV : MonoBehaviour {

    [SerializeField] YipliConfig currentYipliConfig;

    void Start() {
        DetectDevice();
    }

    private void DetectDevice () {
#if UNITY_ANDROID

        Debug.Log("Android TV : From detect device function started");

        /**
         Detect UI_MODE_TYPE_TELEVISION using Android API getSystemService.
        =====================================================================
         Unity Forum poster tarwitz posted similar code on May 20,2015 at: http://forum.unity3d.com/threads/android-tv-detection.295965/

         1. Using Unitiy representation of a generic instance of a Java class - (see: http://docs.unity3d.com/ScriptReference/AndroidJavaClass.html) 
            we ask for the UnityPlayer Android Java object "com.unity.player.UnityPlayer" (see: http://docs.unity3d.com/Manual/PluginsForAndroid.html)
            store this class as an AndroidJavaClass object variable which represents the UnityPlayer class on Android.
         **/
        AndroidJavaClass unityPlayerJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        /** 
         2. Using the unityPlayerJavaClass variable we can aquire the current activity Object instance (see:  http://docs.unity3d.com/ScriptReference/AndroidJavaRunnable.html)
        **/
        AndroidJavaObject androidActivity = unityPlayerJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
        /**
         3. Next we need to grab the current Android Context class
         **/
        AndroidJavaClass contextJavaClass = new AndroidJavaClass("android.content.Context");
        /**
         4. Then we need the object representing the UI_MODE_SERVICE
         **/
        AndroidJavaObject modeServiceConst = contextJavaClass.GetStatic<AndroidJavaObject>("UI_MODE_SERVICE");
        /**
         5. Since we will call a method on the UiModeManager we need to get the system service object from the current activity (see: http://developer.android.com/reference/android/app/UiModeManager.html)
         **/
        AndroidJavaObject uiModeManager = androidActivity.Call<AndroidJavaObject>("getSystemService", modeServiceConst);
        /**
         6. Ask the getCurrentModeType which will return the current running mode type (which comes back as an integer representing the mode). The possible integer values returned represented by:
            Configuration.UI_MODE_TYPE_NORMAL, Configuration.UI_MODE_TYPE_DESK, Configuration.UI_MODE_TYPE_CAR, Configuration.UI_MODE_TYPE_TELEVISION, Configuration.UI_MODE_TYPE_APPLIANCE, or Configuration.UI_MODE_TYPE_WATCH.
            (see: http://developer.android.com/reference/android/content/res/Configuration.html)
         **/
        int currentModeType = uiModeManager.Call<int>("getCurrentModeType");
        /**
         7. Get a reference to the Android Configuratuion class which will allow us to compare the returned mode type integer with a known value - UI_MODE_TYPE_TELEVISION
         **/
        AndroidJavaClass configurationAndroidClass = new AndroidJavaClass("android.content.res.Configuration");
        int modeTypeTelevisionConst = configurationAndroidClass.GetStatic<int>("UI_MODE_TYPE_TELEVISION");
        /**
         8. Perform the integer comparison
         **/
        if (modeTypeTelevisionConst == currentModeType)
        {
            Debug.Log("############ This is an AndroidTV device");

            currentYipliConfig.isDeviceAndroidTV = true;
        }
        else
        {
            Debug.Log("@@@@@@@@@@@@ This is NOT an AndroidTV device");

            currentYipliConfig.isDeviceAndroidTV = false;
        }

        Debug.Log("Android TV : From detect device function ended");
#endif
    }
}