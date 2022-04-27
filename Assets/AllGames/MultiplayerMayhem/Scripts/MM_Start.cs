using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MM_Start : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
