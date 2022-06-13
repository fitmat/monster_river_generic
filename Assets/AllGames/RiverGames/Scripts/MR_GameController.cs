using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MR_GameController : MonoBehaviour
{
    public static MR_GameController instance;

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

        Screen.SetResolution(1280, 720, true);
        Time.timeScale = 0;
        Application.targetFrameRate = 30;
    }

    public int gameTime;
    public bool isGameRunning;
    public bool isSinglePlayer;
    public bool isGameWon;
    public bool isRiverOver;

    public float gameTimeScale;
    public int gameLength;

    public GameObject gameplayObject;
    public float startX, endX, currentX;
    public int playerOneKills, playerTwoKills;
    [SerializeField] private Slider progressBar;

    // Start is called before the first frame update
   
    
    void Start()
    {
        isSinglePlayer = false;
        isRiverOver = false;

        startX = 0;
        endX = gameLength * (MR_TerrainGenerator.instance.terrainLength + 2f);

        //isSinglePlayer = PlayerSession.Instance.currentYipliConfig.MP_GameStateManager.isSinglePlayer;
        Debug.Log("Gamemode = " + InitBLE.getGameMode());
    }

    private void OnDisable()
    {
        MR_AudioManager.instance.StopAllAudio();
    }

    private void Update()
    {
        currentX = gameplayObject.transform.localPosition.x;
        //progressBar.value = (float)(currentX / endX);
    }

    public void StartGame()
    {
        gameTimeScale = 1;

        playerOneKills = 0;
        playerTwoKills = 0;

        gameTime = 0;

        Time.timeScale = gameTimeScale;
        isGameRunning = true;
        StartCoroutine(GameTimer());
    }

    public void SpeedUpGame()
    {
        gameTimeScale += 0.15f;
        Time.timeScale = gameTimeScale;
    }

    private IEnumerator GameTimer()
    {
        if (isGameRunning)
        {
            gameTime++;
            yield return new WaitForSecondsRealtime(1f);
            StartCoroutine(GameTimer());
        }
    }
}
