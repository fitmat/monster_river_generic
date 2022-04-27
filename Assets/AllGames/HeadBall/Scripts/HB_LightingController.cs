using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HB_LightingController : MonoBehaviour
{

    [SerializeField] private Light mainLight;
    [SerializeField] private GameObject streetLights;
    public bool isStreetlightOn;
    public float maxIntensity, minIntensity, duration, currentTime;

    // Start is called before the first frame update
    void Start()
    {
        isStreetlightOn = false;
    }

    // Update is called once per frame
    public IEnumerator DayCycle()
    {
        currentTime = 0f;

        //if (HB_GameController.instance.isGameRunning)
        //{
        while (currentTime < duration)
        {
            mainLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, currentTime / duration);
            yield return null;
            currentTime += Time.deltaTime;
            if (mainLight.intensity < 0.1f && !isStreetlightOn)
            {
                streetLights.SetActive(true);
                isStreetlightOn = true;
            }
        }
    }

}

