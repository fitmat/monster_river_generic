#if UNITY_STANDALONE_WIN
using com.fitmat.fitmatdriver.Producer.Connection;
//using FMInterface_Windows;
#endif

using BLEFramework.Unity;
using System;
using UnityEngine;
using System.Runtime.InteropServices;

public class InitBLE
{
    static AndroidJavaClass _pluginClass;
    static AndroidJavaObject _pluginInstance;
    //const string driverPathName = "com.fitmat.fitmatdriver.Producer.Connection.DeviceControlActivity"; // old reference
    const string driverPathName = "com.fitmat.fmjavainterface.DeviceControlActivity";// This is used only  for android. No dependency on other platforms
    public static string BLEStatus = "";
    public static bool isInitActive = false;
    public static string MAC_ADDRESS = "";
    public static string MAT_NAME = "";
    public static string MAT_UUID = "";

    //required variables
    static string peripheralJsonList = null;
    
    public static string PeripheralJsonList { get => peripheralJsonList; set => peripheralJsonList = value; }
    
    //STEP 3 - Create Unity Callback class
#if UNITY_IOS
		[DllImport("__Internal")]
    private static extern void _InitBLEFramework();

    // For the most part, your imports match the function defined in the iOS code, except char* is replaced with string here so you get a C# string.    
    [DllImport("__Internal")]
    private static extern void _ScanForPeripherals();

    [DllImport("__Internal")]
    private static extern bool _IsDeviceConnected();

    [DllImport("__Internal")]
    private static extern string _GetListOfDevices();

    [DllImport("__Internal")]
    private static extern bool _ConnectPeripheralAtIndex(int peripheralIndex);

    [DllImport("__Internal")]
    private static extern bool _ConnectPeripheral(string peripheralID);
    
    [DllImport("__Internal")]
    private static extern void _Disconnect();
    
    [DllImport("__Internal")]
    private static extern void _SendData(byte[] buffer, int length);
    
    [DllImport("__Internal")]
    private static extern int _GetData(byte[] data, int size);
    
    [DllImport("__Internal")]
    private static extern string _getFMResponse();
    
    [DllImport("__Internal")]
    private static extern void _setGameMode(int _gameMode);
    
    [DllImport("__Internal")]
    private static extern int _getGameMode();
    
    [DllImport("__Internal")]
    private static extern void _setGameID(int _clusterID);
    
    [DllImport("__Internal")]
    private static extern int _getGameID();
    
    [DllImport("__Internal")]
    private static extern void _setGameID_Multiplayer(int _P1_gameID, int _P2_gameID);
    
    [DllImport("__Internal")]
    private static extern int _getGameID_Multiplayer(int _playerID);
    
    [DllImport("__Internal")]
    private static extern string _getDriverVersion();
    
#elif UNITY_ANDROID
    class UnityCallback : AndroidJavaProxy
    {
        private Action<string> initializeHandler;
        public UnityCallback(Action<string> initializeHandlerIn) : base(driverPathName + "$UnityCallback")
        {
            initializeHandler = initializeHandlerIn;
        }
        public void sendMessage(string message)
        {
            Debug.Log("sendMessage: " + message);
            if (message == "connected")
            {
                BLEStatus = "CONNECTED";
            }
            if (message == "disconnected")
            {
                BLEStatus = "DISCONNECTED";
            }
            if (message == "lost")
            {
                BLEStatus = "CONNECTION LOST";
            }
            if (message.Contains("error"))
            {
                BLEStatus = "ERROR";
            }
            initializeHandler?.Invoke(message);
        }
    }
#endif
    
    //STEP 4 - Init Android Class & Objects
    public static AndroidJavaClass PluginClass
    {
        get
        {
            if (_pluginClass == null)
            {
                _pluginClass = new AndroidJavaClass(driverPathName);
            }
            return _pluginClass;
        }
    }
    public static AndroidJavaObject PluginInstance
    {
        get
        {
            if (_pluginInstance == null)
            {
                AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
                _pluginInstance = PluginClass.CallStatic<AndroidJavaObject>("getInstance", activity);
            }
            return _pluginInstance;
        }
    }
    
    public static string GetFMResponse()
    {
        try
        {
#if UNITY_IOS
            string ver = _getFMResponse();
            return ver;
#elif UNITY_ANDROID
            return PluginInstance.Call<string>("_getFMResponse");
#elif UNITY_STANDALONE_WIN
            string defaultResponse = DeviceControlActivity._getFMResponse();
            // this is to check if "null" response is coming from driver as this will come until 1st action is made
            if (defaultResponse.Equals("null", StringComparison.OrdinalIgnoreCase)) {
                
                //defaultResponse = "{\"count\":1,\"timestamp\":1597237057689,\"playerdata\":[{\"id\":1,\"fmresponse\":{\"action_id\":\"NOID\",\"action_name\":\"Jump\",\"properties\":\"null\"}}]}";
                defaultResponse = "Make action";
                return defaultResponse;
            }
            //return DeviceControlActivity._getFMResponse(); // old code
            return defaultResponse;
#endif
        }
        catch (Exception e)
        {
            Debug.Log("Exception in getMatConnectionStatus() : " + e.Message);
            return "error";
        }
        //return "error";
    }
    
    public static string getMatConnectionStatus()
    {
        try
        {
#if UNITY_ANDROID
            return BLEStatus;
#elif UNITY_IOS
            return BLEStatus;
#elif UNITY_STANDALONE_WIN
            return DeviceControlActivity._IsDeviceConnected() == 1 ? "CONNECTED" : "DISCONNECTED";
#endif
        }
        catch (Exception e)
        {
            Debug.Log("Exception in getMatConnectionStatus() : " + e.Message);
            return "disconnected";
        }
        //return "error";
    }
    
    public static void setMatConnectionStatus(string status)
    {
        BLEStatus = status;
    }
    
    /*
    public static void reconnectMat()
    {
        try
        {
#if UNITY_ANDROID
            System.Action<string> callback = ((string message) =>
            {
                BLEFramework.Unity.BLEControllerEventHandler.OnBleDidInitialize(message);
            });
            PluginInstance.Call("_InitBLEFramework", new object[] { new UnityCallback(callback) });
#elif UNITY_STANDALONE_WIN
            //DeviceControlActivity._reconnectDevice();
#elif UNITY_IOS
            // IOS part
#endif
        }
        catch (Exception e)
        {
            Debug.Log("Exception in reconnectMat() : " + e.Message);
        }
    }
    */

    //Android TV Part
    public static void setConnectionType(string type)
    {
        /*
            - Strickly should be used for Android TV
            - Optional for Android
            - Not required for PC
            @Params type : USB or BLE
        */
        PluginInstance.Call("_setConnectionType", type);
    }
    
    //STEP 5 - Init Android Class & Objects
    public static void InitBLEFramework(string macaddress, int gameID, string matAdvertisingName = "YIPLI", bool isThisAndroidTV = false)
    {
        Debug.Log("init_ble: setting macaddress & gameID - " + macaddress + " " + gameID);
        isInitActive = true;
        MAC_ADDRESS = macaddress;
        MAT_NAME = matAdvertisingName;
#if UNITY_IOS
        // Now we check that it's actually an iOS device/simulator, not the Unity Player. You only get plugins on the actual device or iOS Simulator.
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _InitBLEFramework();
        }
#elif UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            System.Action<string> callback = ((string message) =>
            {
                BLEFramework.Unity.BLEControllerEventHandler.OnBleDidInitialize(message);
            });

            if (isThisAndroidTV)
            {
                Debug.Log("Setting up Connection Type : USB");
                setConnectionType("USB");
                Debug.Log("Connection Type set to : USB");
            }
            else
            {
                PluginInstance.Call("_setMACAddress", macaddress);
            }
            
            //PluginInstance.Call("_setMACAddress", macaddress);

            setGameClusterID(gameID);
            PluginInstance.Call("_InitBLEFramework", new object[] { new UnityCallback(callback) });
            /*
            if(!setGameMode(0)){
                Debug.Log("Failed to set Game Mode. Probable reason is your game doesnt support MultiPlayer functionality yet. ");
            }
            */
        }
#elif UNITY_STANDALONE_WIN
            Debug.Log("Calling DeviceControlActivity.InitPCFramework()");
            DeviceControlActivity.InitPCFramework(gameID);
#endif
    }
    
    public static void setGameMode(int gameMode)
    {
        try
        {
#if UNITY_IOS
            _setGameMode(gameMode);
#elif UNITY_ANDROID
            PluginInstance.Call("_setGameMode", gameMode);
#elif UNITY_STANDALONE_WIN
                DeviceControlActivity._setGameMode(gameMode);
#endif
        }
        catch (Exception e)
        {
            Debug.Log("Exception in _setGameMode() : " + e.Message);
        }
    }
    
    public static int getGameMode()
    {
        try
        {
#if UNITY_IOS
            return _getGameMode();
#elif UNITY_ANDROID
            return PluginInstance.Call<int>("_getGameMode");
#elif UNITY_STANDALONE_WIN
                return DeviceControlActivity._getGameMode();
#endif
        }
        catch (Exception e)
        {
            Debug.Log("Exception in _getGameMode() : " + e.Message);
            return 1000;//1000 will be flagged as an invalid GameId on game side.
        }
        //return 1000;
    }
    
    public static void setGameClusterID(int gameID)
    {
        try
        {
#if UNITY_IOS
            _setGameID(gameID);
#elif UNITY_ANDROID
            PluginInstance.Call("_setGameID", gameID);
#elif UNITY_STANDALONE_WIN
                //Debug.Log("Setting cluter ID : " + gameID);
                DeviceControlActivity._setGameID(gameID);
#endif
        }
        catch (Exception e)
        {
            Debug.Log("Exception in setGameClusterID() : " + e.Message);
        }
    }
    
    public static int getGameClusterID()
    {
        try
        {
#if UNITY_IOS
            return _getGameID();
#elif UNITY_ANDROID
            return PluginInstance.Call<int>("_getGameID");
#elif UNITY_STANDALONE_WIN
                return DeviceControlActivity._getGameID();
#endif
        }
        catch (Exception e)
        {
            Debug.Log("Exception in getGameClusterID() : " + e.Message);
            return 1000;//1000 will be flagged as an invalid GameId on game side.
        }
        //return 1000;
    }
    
    public static string getFMDriverVersion()
    {
        try
        {
#if UNITY_IOS
            string ver = _getDriverVersion();
            Debug.Log("Driver Version Received: " + ver);
            return ver;
#elif UNITY_ANDROID
            return PluginInstance.Call<string>("_getDriverVersion");
#elif UNITY_STANDALONE_WIN
            return DeviceControlActivity._getDriverVersion();
#endif
        }
        catch (Exception exp)
        {
            Debug.Log("Exception in Driver Version" + exp.Message);
            return null;
        }
        //return "error";
    }
    
    public static void setGameClusterID(int P1_gameID, int P2_gameID)
    {
        try
        {
#if UNITY_IOS
            _setGameID_Multiplayer(P1_gameID, P2_gameID);
#elif UNITY_ANDROID
            PluginInstance.Call("_setGameID", P1_gameID, P2_gameID);
#elif UNITY_STANDALONE_WIN
            
                DeviceControlActivity._setGameID(P1_gameID, P2_gameID);
#endif
        }
        catch (Exception e)
        {
            Debug.Log("Exception in setGameClusterID() : " + e.Message);
        }
    }
    
    public static int getGameClusterID(int playerID)
    {
        try
        {
#if UNITY_IOS
                return _getGameID_Multiplayer(playerID);
#elif UNITY_ANDROID
            return PluginInstance.Call<int>("_getGameID", playerID);
#elif UNITY_STANDALONE_WIN
                return DeviceControlActivity._getGameID(playerID);
#endif
        }
        catch (Exception e)
        {
            Debug.Log("Exception in getGameClusterID() : " + e.Message);
            return 1000;//1000 will be flagged as an invalid GameId on game side.
        }
        //return 1000;
    }
    
    public static void ScanForPeripherals()
    {
        // We check for UNITY_IPHONE again so we don't try this if it isn't iOS platform.
#if UNITY_IOS
        // Now we check that it's actually an iOS device/simulator, not the Unity Player. You only get plugins on the actual device or iOS Simulator.
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _ScanForPeripherals();
        }
#elif UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            System.Action<string> callback = ((string message) =>
            {
                BLEControllerEventHandler.OnBleDidCompletePeripheralScan(message);
            });
            PluginInstance.Call("_scanForPeripherals", new object[] { new UnityCallback(callback) });
        }
#endif
    }
    
    public static string GetListOfDevices()
    {
        string listOfDevices = "";
        // We check for UNITY_IPHONE again so we don't try this if it isn't iOS platform.
#if UNITY_IOS
        // Now we check that it's actually an iOS device/simulator, not the Unity Player. You only get plugins on the actual device or iOS Simulator.
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            listOfDevices = _GetListOfDevices();
        }
#elif UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            listOfDevices = PluginInstance.Call<string>("_getListOfDevices");
        }
#endif
        return listOfDevices;
    }
    
    public static bool ConnectPeripheral(string peripheralID)
    {
        bool result = false;
        Debug.Log("Connecting at peripheral : " + peripheralID);
        // We check for UNITY_IPHONE again so we don't try this if it isn't iOS platform.
#if UNITY_IOS
        // Now we check that it's actually an iOS device/simulator, not the Unity Player. You only get plugins on the actual device or iOS Simulator.
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            result = _ConnectPeripheral(peripheralID);
            Debug.Log("Connection result : " + result);
            
            if (result)
            {
                BLEStatus = "CONNECTED";
            }
            else
            {
                BLEStatus = "DISCONNECTED";
            }
        }
#elif UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            result = PluginInstance.Call<bool>("_ConnectPeripheral", peripheralID);
        }
#endif
        return result;
    }

    public static void DisconnectMat()
    {
#if UNITY_IOS
        _Disconnect();
#endif
    }
}