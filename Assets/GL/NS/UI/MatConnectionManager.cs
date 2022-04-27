using System.Collections;
using UnityEngine;

namespace GameLib.UI
{
    public class MatConnectionManager : MonoBehaviour
    {
        [SerializeField] private YipliConfig currentYipliConfig = null;
        [SerializeField] private GameObject noMatConnectionPanel = null;
        [SerializeField] private GameObject retryBleConnectionButton = null;

        // Update is called once per frame
        void Update()
        {
            if (YipliHelper.GetMatConnectionStatus().Equals("Connected", System.StringComparison.OrdinalIgnoreCase))
            {
                noMatConnectionPanel.SetActive(true);
            }
            else
            {
                noMatConnectionPanel.SetActive(false);
            }
        }

        public void RecheckButtonFunction()
        {
            StartCoroutine(ReconnectBleFromGame());
        }

        private IEnumerator ReconnectBleFromGame()
        {
            retryBleConnectionButton.SetActive(false);
            Debug.Log("In ReconnectBleFromGame.");
            try
            {
                //Initiate mat connection with last set GameCluterId
                Debug.Log("ReconnectBle with Game clster ID : " + YipliHelper.GetGameClusterId());
#if UNITY_ANDROID
            InitBLE.InitBLEFramework(currentYipliConfig.matInfo?.macAddress ?? "", YipliHelper.GetGameClusterId() != 1000 ? YipliHelper.GetGameClusterId() : 0, currentYipliConfig.matInfo?.matAdvertisingName ?? LibConsts.MatTempAdvertisingNameOnlyForNonIOS, currentYipliConfig.isDeviceAndroidTV);
#elif UNITY_IOS
            InitBLE.InitBLEFramework(currentYipliConfig.matInfo?.macAddress ?? "", 0, currentYipliConfig.matInfo?.matAdvertisingName ?? LibConsts.MatTempAdvertisingNameOnlyForNonIOS);
#else
                InitBLE.InitBLEFramework(currentYipliConfig.matInfo?.macAddress ?? "", 0);
                //InitBLE.reconnectMat();
#endif
            }
            catch (System.Exception exp)
            {
                Debug.Log("Exception in InitBLEFramework from ReconnectBleFromGame" + exp.Message);
            }

            //Block this function for next 5 seconds by disabling the retry Button.
            //Dont allow user to initiate Bluetooth connection for atleast 5 secs, as 1 connecteion initiation is enough.
            yield return new WaitForSecondsRealtime(5f);
            retryBleConnectionButton.SetActive(true);
        }
    }
}
