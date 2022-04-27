using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToW_GamePlayObjectManager : MonoBehaviour
{
    public static ToW_GamePlayObjectManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [SerializeField] private GameObject gamePlayObject, gamePlayPrefab, gamePlayPosition;

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 10);
    }

    public void SetGameObject()
    {
        if (gamePlayObject != null)
        {
            Destroy(gamePlayObject);
        }

        gamePlayObject = Instantiate(gamePlayPrefab, gamePlayPosition.transform);
    }
}
