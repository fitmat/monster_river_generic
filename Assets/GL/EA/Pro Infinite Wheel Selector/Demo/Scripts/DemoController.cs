using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class DemoController : MonoBehaviour
{
    public Button back;
    public Canvas canvas_mask;
    public Canvas canvas_background;

    bool FirtScene = true;

    private void Start()
    {
        FirtScene = true;
        back.gameObject.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!FirtScene)
            {
                Back();
            }
            else
            {
                Application.Quit();
            }
        }
    }

    public void LoadScene(string name_scene)
    {
        back.gameObject.SetActive(true);
        SceneManager.LoadScene(name_scene, LoadSceneMode.Single);

        StartCoroutine(FindNewCamera());
        FirtScene = false;
    }


    IEnumerator FindNewCamera()
    {
        Camera new_camera = null;
        while (true)
        {
            yield return null;//waiting for the next frame

            new_camera = FindObjectOfType<Camera>();
            if (new_camera) { break; }
        }
        canvas_mask.worldCamera = new_camera;
        canvas_background.worldCamera = new_camera;
    }

    public void Back()
    {
        SceneManager.LoadScene("demo", LoadSceneMode.Single);
        Destroy(gameObject);
    }
}
