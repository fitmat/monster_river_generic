using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject loading;

    public void OnPlayButton()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        loading.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
