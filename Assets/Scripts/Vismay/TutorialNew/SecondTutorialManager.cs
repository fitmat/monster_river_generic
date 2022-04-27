using UnityEngine;
using UnityEngine.UI;
using YipliFMDriverCommunication;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class SecondTutorialManager : MonoBehaviour
{
    //required variables

    [Header("UI Elements")]
    [SerializeField] private GameObject tutorialPanel = null;
    [SerializeField] private GameObject confetiParent = null;
    [SerializeField] private Image runningManMat = null;
    [SerializeField] private GameObject threeBoxesParent = null;
    [SerializeField] private GameObject tryItBoxParent = null;
    [SerializeField] private GameObject finalTutParent = null;

    [Header("Script objects")]
    [SerializeField] private YipliConfig currentYipliConfig = null;
    [SerializeField] private NewMatInputController newMatInputController = null;
    //[SerializeField] private MatInputController matInputController = null;
    [SerializeField] private ThreeDModelManager threeDModelManager = null;

    [Header("All Text messages")]
    [SerializeField] private GameObject letsLearnHowToUseMatMSG = null;
    [SerializeField] private GameObject highlightGameMenu = null;
    [SerializeField] private GameObject toSelectJumpOnMat = null;
    [SerializeField] private GameObject navigatingWithYourMatMSG = null;
    [SerializeField] private GameObject letsLearnToUseYourMatMSG = null;
    [SerializeField] private GameObject messageStepOnMatAndCenter = null;
    [SerializeField] private GameObject tapLeft3Times = null;
    [SerializeField] private GameObject tapRight3Times = null;
    [SerializeField] private GameObject jump3Times = null;
    [SerializeField] private GameObject awesomeJob = null;
    [SerializeField] private GameObject youGotThatRight = null;
    [SerializeField] private GameObject pauseTitle = null;
    [SerializeField] private GameObject pauseStatement = null;
    [SerializeField] private GameObject resumeTitle = null;
    [SerializeField] private GameObject resumeStatement = null;
    [SerializeField] private GameObject trainingDoneTitle = null;
    [SerializeField] private GameObject goodHeadStart = null;
    [SerializeField] private GameObject letsStartPlaying = null;
    [SerializeField] private TextMeshProUGUI ftStatement = null;

    [Header("All Required Colors")]
    [SerializeField] private Sprite switchPlayerS = null;
    [SerializeField] private Sprite playFromMM_S = null;
    [SerializeField] private Sprite yipliHubS = null;

    [Header("All Required Colors")]
    [SerializeField] private Color yipliRed;
    [SerializeField] private Color yipliRedNoS;
    [SerializeField] private Color yipliGreen;
    [SerializeField] private Color yipliMarine;
    [SerializeField] private Color originalButtonColor;

    [Header("All Required Music")]
    [SerializeField] private AudioSource tutorialAudioSource;
    [SerializeField] private AudioClip checkMarkSound;
    [SerializeField] private AudioClip errorSound;

    [Header("Skip Button")]
    [SerializeField] private GameObject skipButton = null;

    [Header("All instruction audios")]
    [SerializeField] private AudioSource tutorialPanelAudioSource = null;
    [SerializeField] private AudioClip congratsChamp = null;
    [SerializeField] private AudioClip doJumps = null;
    [SerializeField] private AudioClip doLefttaps = null;
    [SerializeField] private AudioClip doRightTaps = null;
    [SerializeField] private AudioClip letsPractise = null;
    [SerializeField] private AudioClip pauseGame = null;
    [SerializeField] private AudioClip thisIsAGoodHeadStart = null;
    [SerializeField] private AudioClip toResumeStepOnTheMatAndJump = null;
    [SerializeField] private AudioClip tryTheExtensive = null;
    [SerializeField] private AudioClip finalClaps = null;

    // private variables
    YipliUtils.PlayerActions detectedAction;

    [Header("Test Area")]
    // bool values
    private bool leftTapsDone = false;
    private bool rightTapsDone = false;
    private bool jumpsDone = false;
    private bool runningIntroDone = false;
    private bool startIntroDone = false;
    private bool calculateTime = false;
    private bool tutorialStarted = false;
    private bool userInteractionStarted = false;
    private bool pauseFlowStarted = false;
    private bool resumeFlowStarted = false;
    private bool tapsAndJumpInfoFlowStarted = false;
    private bool simpleTutorialDone = false;
    private bool finalTutStarted = false;
    private bool finalMessageDisplayStarted = false;
    private bool waitForNextPart = false;
    private bool listenToMatActions = false;
    private bool turn180triggered = false;
    private bool headStartAudioPlayed = false;

    // int values
    private int totalLeftTaps = 0;
    private int totalRightTaps = 0;
    private int totalJumps = 0;
    private int currentCircleChildActive = 0;
    public int requiredCHildElement = -1;

    // float time variables
    private float currentCalculatedTime = 0;

    // getters and setters
    public YipliUtils.PlayerActions DetectedAction { get => detectedAction; set => detectedAction = value; }

    void Start()
    {
        runningManMat.gameObject.SetActive(false);
        tutorialPanel.SetActive(false);
        skipButton.SetActive(false);

        //newMatInputController.HideLegs();
        SetTutorialClusterID(6);

        //TurnOffAllPHAnimationOBJs();
        //TurnOnPHAnimationOBJs();
    }

    public void TurnOffEverything()
    {
        letsLearnHowToUseMatMSG.gameObject.SetActive(false);
        highlightGameMenu.gameObject.SetActive(false);
        toSelectJumpOnMat.gameObject.SetActive(false);
        navigatingWithYourMatMSG.gameObject.SetActive(false);
        letsLearnToUseYourMatMSG.gameObject.SetActive(false);
        messageStepOnMatAndCenter.gameObject.SetActive(false);
        tapLeft3Times.gameObject.SetActive(false);
        tapRight3Times.gameObject.SetActive(false);
        jump3Times.gameObject.SetActive(false);
        awesomeJob.gameObject.SetActive(false);
        youGotThatRight.gameObject.SetActive(false);
        pauseTitle.gameObject.SetActive(false);
        pauseStatement.gameObject.SetActive(false);
        resumeTitle.gameObject.SetActive(false);
        resumeStatement.gameObject.SetActive(false);
        trainingDoneTitle.gameObject.SetActive(false);
        goodHeadStart.gameObject.SetActive(false);
        letsStartPlaying.gameObject.SetActive(false);

        HideThreeBoxes();
        HideTryItBox();
        HideFinalTutElements();
    }

    void Update()
    {

        //if (!tutorialStarted) return;

        CalculateTime();

        //if (matInputController.IsTutorialRunning && listenToMatActions) {
        //    GetMatTutorialKeyboardInputs();
        //    ManageMatActionsForTutorial();
        //}

        GetMatTutorialKeyboardInputs();
        ManageMatActionsForTutorial();

        ManageMatTutorial();
    }

    public void SetTutorialClusterID(int clusterID)
    {
        try
        {
            //Debug.LogError("provided clusterID : " + clusterID);
            YipliHelper.SetGameClusterId(clusterID);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Something went wrong with setting the cluster id : " + e.Message);
        }
    }

    public void StartMatTutorial()
    {

        if (tutorialStarted) return;

        //matInputController.IsTutorialRunning = true;
        tutorialStarted = true;

        SetTutorialClusterID(6);

        TurnOffEverything();

        tutorialPanel.SetActive(true);

        // newMatInputController.DisplayMainMat();
        // newMatInputController.HideChevrons();
        // newMatInputController.UpdateCenterButtonColor();
        // newMatInputController.EnableMatParentButtonAnimator();

        // new running mat intro
        threeDModelManager.Display3DModel();
        threeDModelManager.Display3DMat();
        threeDModelManager.EnableModelManagerAnimator();
        threeDModelManager.ApplyWalkingOverride();

        skipButton.SetActive(true);
        // new animation code over

        letsLearnHowToUseMatMSG.gameObject.SetActive(true);
        messageStepOnMatAndCenter.gameObject.SetActive(true);
        newMatInputController.DisplayTextButtons();
        newMatInputController.KeepLeftNadRightButtonColorToOriginal();

        EnableMatActionListener();
    }

    public void EndMatTutorial()
    {
        calculateTime = false;
        currentCalculatedTime = 0f;

        confetiParent.SetActive(false);

        threeDModelManager.ApplyMainIdleOverride();
        threeDModelManager.Hide3DModel();
        threeDModelManager.Hide3DMat();

        newMatInputController.HideTextButtons();

        tutorialPanel.gameObject.SetActive(false);

        newMatInputController.UpdateCenterButtonWithOriginalColor();

        SetTutorialClusterID(0);

        //matInputController.IsTutorialRunning = false;

        //ResetTutorial();

        newMatInputController.DisableMatParentButtonAnimator();

        // load main game menu scene
        // FindObjectOfType<PlayerSelection>().OnTutorialContinuePress();

        // update player' status
        if (YipliHelper.checkInternetConnection())
        {
            if (currentYipliConfig.playerInfo != null)
            {
                if (currentYipliConfig.playerInfo.isMatTutDone == 0)
                {
                    FirebaseDBHandler.UpdateTutStatusData(currentYipliConfig.userId, currentYipliConfig.playerInfo.playerId, 1);
                    //UserDataPersistence.SavePropertyValue("player-tutDone", 1.ToString());
                }
            }
        }

        SceneManager.LoadScene(currentYipliConfig.callbackLevel);
    }

    public void ManageMatTutorial()
    {
        if (!startIntroDone)
        {
            StartMatTutorial();
            return;
        }

        // old code
        // if (!runningIntroDone) {
        //     StartRunningIntro();

        //     if (currentCalculatedTime > 10f) {
        //         runningIntroDone = true;
        //         calculateTime = false;
        //     }
        // }

        // if (!runningIntroDone) return;
        // old code over

        if (!runningIntroDone)
        {

            if (!turn180triggered)
            {
                turn180triggered = true;

                threeDModelManager.ApplyMainIdleOverride();

                threeDModelManager.TurnModelManager180();
                TurnOffEverything();
            }

            letsLearnToUseYourMatMSG.gameObject.SetActive(true);

            calculateTime = true;

            if (currentCalculatedTime > 1.5f)
            {
                runningIntroDone = true;
                calculateTime = false;
            }
        }

        if (!runningIntroDone) return;

        if (!leftTapsDone)
        {
            StartLeftTapsPart();
        }

        if (waitForNextPart)
        {
            if (currentCalculatedTime > 1.5f)
            {
                waitForNextPart = false;
                calculateTime = false;
                currentCalculatedTime = 0;

                StartNextTapsPart();
            }
            else
            {
                return;
            }
        }

        if (leftTapsDone && rightTapsDone && jumpsDone && !simpleTutorialDone)
        {
            if (currentCalculatedTime > 0f)
            { // 2.5 wait is original
                calculateTime = false;
                simpleTutorialDone = true;
                StartPauseFlow();
            }
        }

        if (pauseFlowStarted)
        {
            if ((currentCalculatedTime > 2f && currentCalculatedTime < 2.5f) && !tryItBoxParent.activeSelf)
            {
                calculateTime = false;
                DisplayTryItBox();
                return;
            }

            if (currentCalculatedTime > 2.5f && tryItBoxParent.activeSelf)
            {
                StartResumeFlow();
            }
        }

        if (resumeFlowStarted)
        {
            if ((currentCalculatedTime > 2f && currentCalculatedTime < 2.5f) && !tryItBoxParent.activeSelf)
            {
                calculateTime = false;
                DisplayTryItBox();
                return;
            }

            if (currentCalculatedTime > 2.5f && tryItBoxParent.activeSelf)
            {
                HideTryItBox();

                resumeTitle.gameObject.SetActive(false);
                resumeStatement.gameObject.SetActive(false);

                FindObjectOfType<ModelEventActivator>().SetZTransformToZero();

                DisableMatActionListener();
                TurnOnAwesomePart();
            }

            if (currentCalculatedTime > 5f)
            {
                resumeFlowStarted = false;
                StartTapAndJumpInfoFlow();
            }
        }

        if (tapsAndJumpInfoFlowStarted)
        {
            if (currentCalculatedTime > 2.5f && currentCalculatedTime < 3f)
            {
                highlightGameMenu.gameObject.SetActive(true);
            }
            else if (currentCalculatedTime > 3f && currentCalculatedTime < 7f)
            {
                toSelectJumpOnMat.gameObject.SetActive(true);
            }
            else if (currentCalculatedTime > 10f)
            {
                tapsAndJumpInfoFlowStarted = true;
                calculateTime = false;

                StartFinalTutorial();
            }
        }

        if (finalMessageDisplayStarted)
        {
            if (currentCalculatedTime > 3f && currentCalculatedTime < 7f)
            {
                TurnOffAwesomePart();

                trainingDoneTitle.gameObject.SetActive(true);
                goodHeadStart.gameObject.SetActive(true);
                letsStartPlaying.gameObject.SetActive(true);

                if (!headStartAudioPlayed)
                {
                    headStartAudioPlayed = true;
                    threeDModelManager.StartRMFinalPart();
                    PlayInstructionAudio(thisIsAGoodHeadStart);
                }
            }
            else if (currentCalculatedTime > 15f)
            {
                EndMatTutorial();
            }
        }
    }

    private void CalculateTime()
    {
        if (calculateTime)
        {
            currentCalculatedTime += Time.deltaTime;
        }
        else
        {
            currentCalculatedTime = 0;
        }
    }

    // step 2
    private void StartRunningIntro()
    {
        if (calculateTime) return;

        TurnOffEverything();

        calculateTime = true;

        newMatInputController.HideTextButtons();
        newMatInputController.HideMainMat();
        letsLearnToUseYourMatMSG.gameObject.SetActive(true);

        // apply main animation here
        threeDModelManager.Display3DModel();
        threeDModelManager.Display3DMat();
        threeDModelManager.EnableModelManagerAnimator();
        threeDModelManager.ApplyWalkingOverride();

        runningManMat.gameObject.SetActive(true);
    }

    // step 3
    private void StartLeftTapsPart()
    {
        if (userInteractionStarted) return;
        skipButton.SetActive(false);

        userInteractionStarted = true;

        tapLeft3Times.GetComponent<Animator>().enabled = false;
        tapRight3Times.GetComponent<Animator>().enabled = false;
        jump3Times.GetComponent<Animator>().enabled = false;

        tapLeft3Times.transform.GetChild(1).gameObject.SetActive(false);
        tapRight3Times.transform.GetChild(1).gameObject.SetActive(false);
        jump3Times.transform.GetChild(1).gameObject.SetActive(false);

        tapLeft3Times.gameObject.SetActive(true);
        threeDModelManager.ApplyLeftTapOverride();

        PlayInstructionAudio(doLefttaps);

        DisplayThreeBoxes();
        EnableMatActionListener();
    }

    // step 4
    private void StartRightTapsPart()
    {
        tapRight3Times.gameObject.SetActive(true);
        threeDModelManager.ApplyRightTapOverride();

        PlayInstructionAudio(doRightTaps);

        ResetBoxes();
        EnableMatActionListener();
    }

    // step 4
    private void StartJumpsPart()
    {
        tapRight3Times.transform.GetChild(1).gameObject.SetActive(true);
        tapRight3Times.GetComponent<Animator>().enabled = true;

        jump3Times.gameObject.SetActive(true);
        threeDModelManager.ApplyJumpOverride();

        PlayInstructionAudio(doJumps);

        ResetBoxes();
        EnableMatActionListener();
    }

    // Step 5
    private void EndTapsPart()
    {
        DisableMatActionListener();

        tapLeft3Times.gameObject.SetActive(false);
        tapRight3Times.gameObject.SetActive(false);
        jump3Times.gameObject.SetActive(false);
        letsLearnToUseYourMatMSG.gameObject.SetActive(false);

        threeDModelManager.ApplyMainIdleOverride();

        userInteractionStarted = false;
        calculateTime = true;

        HideThreeBoxes();

        threeDModelManager.ActivatePausePart();

        TurnOnAwesomePart();
    }

    private void TurnOnAwesomePart()
    {
        awesomeJob.gameObject.SetActive(true);
        youGotThatRight.gameObject.SetActive(true);
    }

    private void TurnOffAwesomePart()
    {
        awesomeJob.gameObject.SetActive(false);
        youGotThatRight.gameObject.SetActive(false);
    }

    // step 6 - pause part
    private void StartPauseFlow()
    {
        pauseFlowStarted = true;
        calculateTime = false;
        currentCalculatedTime = 0f;

        TurnOffAwesomePart();

        //threeDModelManager.ActivatePausePart();

        pauseStatement.gameObject.SetActive(true);
        pauseTitle.gameObject.SetActive(true);

        PlayInstructionAudio(pauseGame);

        calculateTime = true;
    }

    // step 7 - resume part
    private void StartResumeFlow()
    {
        pauseFlowStarted = false;
        resumeFlowStarted = true;

        calculateTime = false;
        currentCalculatedTime = 0f;

        ResetTryItBox();

        pauseStatement.gameObject.SetActive(false);
        pauseTitle.gameObject.SetActive(false);

        resumeTitle.gameObject.SetActive(true);
        resumeStatement.gameObject.SetActive(true);

        PlayInstructionAudio(toResumeStepOnTheMatAndJump);
    }

    // step 8 - info panel
    private void StartTapAndJumpInfoFlow()
    {
        calculateTime = false;
        currentCalculatedTime = 0f;

        TurnOffAwesomePart();

        //navigatingWithYourMatMSG.gameObject.SetActive(true);

        tapsAndJumpInfoFlowStarted = true;
        calculateTime = true;
    }

    // step 9 - Final Tutorial
    private void StartFinalTutorial()
    {
        navigatingWithYourMatMSG.gameObject.SetActive(false);
        highlightGameMenu.gameObject.SetActive(false);
        toSelectJumpOnMat.gameObject.SetActive(false);

        finalTutStarted = true;

        DisplayFinalTutElements();
        EnableMatActionListener();
    }

    // step 10 - training complete
    private void ShowFinalMessages()
    {
        DisableMatActionListener();

        finalTutStarted = false;
        finalMessageDisplayStarted = true;
        tapsAndJumpInfoFlowStarted = false;

        threeDModelManager.ApplyFistPumpOverride();

        HideFinalTutElements();

        PlayInstructionAudio(congratsChamp);

        awesomeJob.gameObject.SetActive(true);
        youGotThatRight.gameObject.SetActive(true);

        calculateTime = true;
    }

    // next part to invoke right taps and jump part following tutorial forward
    private void ProcessTapsPart()
    {
        if (!leftTapsDone && totalLeftTaps > 2)
        {
            leftTapsDone = true;

            tapLeft3Times.transform.GetChild(1).gameObject.SetActive(true);
            tapLeft3Times.GetComponent<Animator>().enabled = true;

            StartWaitPart();
            return;
        }

        if (!rightTapsDone && totalRightTaps > 2)
        {
            rightTapsDone = true;

            tapRight3Times.transform.GetChild(1).gameObject.SetActive(true);
            tapRight3Times.GetComponent<Animator>().enabled = true;

            StartWaitPart();
            return;
        }

        if (!jumpsDone && totalJumps > 2)
        {
            jumpsDone = true;

            jump3Times.transform.GetChild(1).gameObject.SetActive(true);
            jump3Times.GetComponent<Animator>().enabled = true;

            StartWaitPart();
            return;
        }
    }

    private void StartWaitPart()
    {
        DisableMatActionListener();

        waitForNextPart = true;
        calculateTime = true;
    }

    private void StartNextTapsPart()
    {
        if (leftTapsDone && !rightTapsDone && !jumpsDone)
        {
            StartRightTapsPart();
            return;
        }

        if (leftTapsDone && rightTapsDone && !jumpsDone)
        {
            StartJumpsPart();
            return;
        }

        if (leftTapsDone && rightTapsDone && jumpsDone)
        {
            EndTapsPart();
            return;
        }
    }

    // manage check mark boxes
    private void DisplayThreeBoxes()
    {
        threeBoxesParent.gameObject.SetActive(true);
    }

    private void HideThreeBoxes()
    {
        threeBoxesParent.gameObject.SetActive(false);
    }

    private void ResetBoxes()
    {
        threeBoxesParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = Color.white;
        threeBoxesParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.white;
        threeBoxesParent.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().color = Color.white;
    }

    private void MarkFirstBox()
    {
        PlaySound(checkMarkSound);
        threeBoxesParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = yipliGreen;
    }

    private void MarkSecondBox()
    {
        PlaySound(checkMarkSound);
        threeBoxesParent.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = yipliGreen;
    }

    private void MarkThirdBox()
    {
        PlaySound(checkMarkSound);
        threeBoxesParent.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().color = yipliGreen;
    }

    private void ProcessBoxCheckMarks(int taps)
    {
        switch (taps)
        {
            case 1:
                MarkFirstBox();
                break;

            case 2:
                MarkSecondBox();
                break;

            case 3:
                MarkThirdBox();
                ProcessTapsPart();
                break;

            default:
                break;
        }
    }

    private void ShakeBoxes()
    {
        for (int i = 0; i < threeBoxesParent.transform.childCount; i++)
        {
            if (threeBoxesParent.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().color == yipliGreen) continue;

            PlaySound(errorSound);
            threeBoxesParent.transform.GetChild(i).GetComponent<DOTweenAnimation>().DOPlay();
        }
    }

    // try it box
    private void DisplayTryItBox()
    {
        EnableMatActionListener();
        tryItBoxParent.gameObject.SetActive(true);
    }

    private void HideTryItBox()
    {
        tryItBoxParent.gameObject.SetActive(false);
    }

    private void MarkTryItBox()
    {
        PlaySound(checkMarkSound);
        tryItBoxParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = yipliGreen;
    }

    private void ResetTryItBox()
    {
        tryItBoxParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = Color.white;
    }

    private void ShakeTryItBox()
    {
        PlaySound(errorSound);
        tryItBoxParent.transform.GetChild(0).GetComponent<DOTweenAnimation>().DOPlay();
    }

    // final tutorial management
    private void DisplayFinalTutElements()
    {
        finalTutParent.gameObject.SetActive(true);

        finalTutParent.transform.GetChild(2).transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
        finalTutParent.transform.GetChild(2).transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(false);
        finalTutParent.transform.GetChild(2).transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(false);

        PlayInstructionAudio(letsPractise);

        MakeAllCircleChildrenNormal();

        MarkCircleAsRequiredToSelect(finalTutParent.transform.GetChild(2).transform.GetChild(2).gameObject);
        ftStatement.text = "Select";
        ftStatement.transform.GetChild(0).GetComponent<Image>().sprite = yipliHubS;
        requiredCHildElement = 2;

        currentCircleChildActive = 0;
        TraverseThroughAllCirclesLikeButtons();
    }

    private void MakeAllCircleChildrenNormal()
    {
        for (int i = 0; i < finalTutParent.transform.GetChild(2).transform.childCount; i++)
        {
            finalTutParent.transform.GetChild(2).transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
            //finalTutParent.transform.GetChild(2).transform.GetChild(i).transform.GetChild(3).gameObject.SetActive(false);
        }
    }

    private void HideFinalTutElements()
    {
        finalTutParent.gameObject.SetActive(false);
    }

    private void MarkCircleAsRequiredToSelect(GameObject childCirle)
    {
        //childCirle.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void TraverseThroughAllCirclesLikeButtons()
    {
        for (int i = 0; i < finalTutParent.transform.GetChild(2).transform.childCount; i++)
        {
            if (i == currentCircleChildActive)
            {
                finalTutParent.transform.GetChild(2).transform.GetChild(i).transform.localScale = new Vector3(1.3f, 1.3f, 1f);
                finalTutParent.transform.GetChild(2).transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);

                // check mark management
                // if (finalTutParent.transform.GetChild(2).transform.GetChild(currentCircleChildActive).transform.GetChild(2).gameObject.activeSelf) {
                //     finalTutParent.transform.GetChild(2).transform.GetChild(currentCircleChildActive).transform.GetChild(2).gameObject.SetActive(true);
                // }

            }
            else
            {
                finalTutParent.transform.GetChild(2).transform.GetChild(i).transform.localScale = new Vector3(1f, 1f, 1f);
                finalTutParent.transform.GetChild(2).transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
                finalTutParent.transform.GetChild(2).transform.GetChild(i).transform.GetChild(1).GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void ProcessFinalTutJumpAction()
    {
        if (currentCircleChildActive == requiredCHildElement)
        {
            // switchPlayer, playButton, yipliHub
            switch (requiredCHildElement)
            {
                case 0:
                    ftStatement.transform.GetChild(0).GetComponent<Animator>().SetTrigger("switchPlayer");
                    break;

                case 1:
                    ftStatement.transform.GetChild(0).GetComponent<Animator>().SetTrigger("playButton");
                    break;

                case 2:
                    ftStatement.transform.GetChild(0).GetComponent<Animator>().SetTrigger("yipliHub");
                    break;
            }
        }
        else
        {
            threeDModelManager.ApplyHeadNodOverride();
            PlaySound(errorSound);
            ShakeCircles();
        }
    }

    private void SelectCircleChild()
    {
        /*if (currentCircleChildActive == requiredCHildElement) {
            finalTutParent.transform.GetChild(2).transform.GetChild(currentCircleChildActive).transform.GetChild(1).GetComponent<Image>().color = Color.white;
            //finalTutParent.transform.GetChild(2).transform.GetChild(currentCircleChildActive).transform.GetChild(2).GetComponent<Image>().color = yipliRedNoS;
            finalTutParent.transform.GetChild(2).transform.GetChild(currentCircleChildActive).transform.GetChild(2).gameObject.SetActive(true);
            
            threeDModelManager.ApplyFistPumpOverride();
            PlaySound(checkMarkSound);

            // switchPlayer, playButton, yipliHub

            ProgressFinalTutorial();
            ftStatement.transform.GetChild(0).GetComponent<Animator>().SetTrigger("");
        } else {
            threeDModelManager.ApplyHeadNodOverride();
            PlaySound(errorSound);
            ShakeCircles();
        } */

        finalTutParent.transform.GetChild(2).transform.GetChild(currentCircleChildActive).transform.GetChild(1).GetComponent<Image>().color = Color.white;
        //finalTutParent.transform.GetChild(2).transform.GetChild(currentCircleChildActive).transform.GetChild(2).GetComponent<Image>().color = yipliRedNoS;
        finalTutParent.transform.GetChild(2).transform.GetChild(currentCircleChildActive).transform.GetChild(2).gameObject.SetActive(true);

        threeDModelManager.ApplyFistPumpOverride();
        PlaySound(checkMarkSound);

        ProgressFinalTutorial();
        //ftStatement.transform.GetChild(0).GetComponent<Animator>().SetTrigger("");
    }

    public void GoForNextTaskInFT()
    {
        SelectCircleChild();
    }

    private void ProgressFinalTutorial()
    {
        switch (requiredCHildElement)
        {
            case 0:
                MakeAllCircleChildrenNormal();

                ftStatement.text = "Again select";
                ftStatement.transform.GetChild(0).GetComponent<Image>().sprite = playFromMM_S;

                MarkCircleAsRequiredToSelect(finalTutParent.transform.GetChild(2).transform.GetChild(1).gameObject);
                requiredCHildElement = 1;
                TraverseThroughAllCirclesLikeButtons();
                break;

            case 1:
                ShowFinalMessages();
                break;

            case 2:
                MakeAllCircleChildrenNormal();

                ftStatement.text = "Now select";
                ftStatement.transform.GetChild(0).GetComponent<Image>().sprite = switchPlayerS;

                MarkCircleAsRequiredToSelect(finalTutParent.transform.GetChild(2).transform.GetChild(0).gameObject);
                requiredCHildElement = 0;
                TraverseThroughAllCirclesLikeButtons();
                break;
        }
    }

    private int GetNextCircleChild()
    {
        switch (currentCircleChildActive)
        {
            case 0:
                return 1;

            case 1:
                return 2;

            case 2:
                return 0;

            default:
                return 0;
        }
    }

    private int GetPreviousCircleChild()
    {
        switch (currentCircleChildActive)
        {
            case 0:
                return 2;

            case 1:
                return 0;

            case 2:
                return 1;

            default:
                return 0;
        }
    }

    private void ShakeCircles()
    {
        for (int i = 0; i < finalTutParent.transform.GetChild(2).transform.childCount; i++)
        {
            if (i == requiredCHildElement) continue;

            finalTutParent.transform.GetChild(2).transform.GetChild(i).GetComponent<DOTweenAnimation>().DOPlay();
        }
    }

    // controller part
    private void ManageMatActionsForTutorial()
    {
        //if (!currentYipliConfig.onlyMatPlayMode) return;

        string fmActionData = InitBLE.GetFMResponse();
        Debug.Log("stut : Json Data from Fmdriver in matinput : " + fmActionData);

        FmDriverResponseInfo singlePlayerResponse = null;

        Debug.LogError("stut : getting single player response");
        try
        {
            Debug.LogError("stut : from try");
            singlePlayerResponse = JsonUtility.FromJson<FmDriverResponseInfo>(fmActionData);
        }
        catch (System.Exception e)
        {
            Debug.LogError("stut : singlePlayerResponse is having problem : " + e.Message);
        }

        Debug.LogError("stut : single player responce might be null returning");
        if (singlePlayerResponse == null) return;

        Debug.LogError("stut : single player response is not null");

        Debug.LogError("stut : currentYipliConfig.oldFMResponseCount : " + currentYipliConfig.oldFMResponseCount);
        Debug.LogError("stut : singlePlayerResponse.count : " + singlePlayerResponse.count);

        if (currentYipliConfig.oldFMResponseCount != singlePlayerResponse.count)
        {
            Debug.LogError("stut : from if");
            PlayerSession.Instance.currentYipliConfig.oldFMResponseCount = singlePlayerResponse.count;

            Debug.LogError("stut : next line is to detect action");
            DetectedAction = ActionAndGameInfoManager.GetActionEnumFromActionID(singlePlayerResponse.playerdata[0].fmresponse.action_id);
            Debug.LogError("stut : action is detected" + DetectedAction);

            switch (DetectedAction)
            {
                // UI input executions
                case YipliUtils.PlayerActions.LEFT_TAP:
                    ManagePlayerActions();
                    break;

                case YipliUtils.PlayerActions.RIGHT_TAP:
                    ManagePlayerActions();
                    break;

                case YipliUtils.PlayerActions.JUMP:
                    ManagePlayerActions();
                    break;

                case YipliUtils.PlayerActions.PAUSE:
                    ManagePlayerActions();
                    break;

                default:
                    Debug.LogError("stut : Wrong Action is detected : " + DetectedAction.ToString());
                    break;
            }
        }
    }

    private void GetMatTutorialKeyboardInputs()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DetectedAction = YipliUtils.PlayerActions.LEFT_TAP;
            ManagePlayerActions();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DetectedAction = YipliUtils.PlayerActions.RIGHT_TAP;
            ManagePlayerActions();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DetectedAction = YipliUtils.PlayerActions.JUMP;
            ManagePlayerActions();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DetectedAction = YipliUtils.PlayerActions.PAUSE;
            ManagePlayerActions();
        }
    }

    private void ManagePlayerActions()
    {

        //if (phoneHolderPanel.activeSelf)
        //{
        //    Debug.LogError("new tut : from ManagePlayerActions if");
        //    if (DetectedAction == YipliUtils.PlayerActions.JUMP)
        //    {
        //        Debug.LogError("new tut : from ManagePlayerActions if if ");
        //        StartTutrorialOnJump();
        //    }
        //    return;
        //}

        Debug.LogError("stut : connection status is : " + YipliHelper.GetMatConnectionStatus());
        Debug.LogError("stut : current cluster id is : " + YipliHelper.GetGameClusterId());
        Debug.LogError("stut : only mat play mode is : " + currentYipliConfig.onlyMatPlayMode);
        Debug.LogError("stut : Detected action is : " + DetectedAction.ToString());

        if (!startIntroDone)
        {
            //if (DetectedAction == YipliUtils.PlayerActions.LEFT_TAP) {
            //    //EndMatTutorial(); // uncomment to allow skip from mat
            //    return;
            //} else
            if (DetectedAction == YipliUtils.PlayerActions.JUMP)
            {
                startIntroDone = true;
                ManagePlayerActions();
                DisableMatActionListener();
            }
            else
            {
                return;
            }
        }

        if (!leftTapsDone)
        {

            if (DetectedAction != YipliUtils.PlayerActions.LEFT_TAP)
            {
                ShakeBoxes();
                return;
            }

            totalLeftTaps++;
            ProcessBoxCheckMarks(totalLeftTaps);
        }

        if (!rightTapsDone)
        {
            if (!leftTapsDone) return;

            if (DetectedAction != YipliUtils.PlayerActions.RIGHT_TAP)
            {
                ShakeBoxes();
                return;
            }

            totalRightTaps++;
            ProcessBoxCheckMarks(totalRightTaps);
        }

        if (!jumpsDone)
        {
            if (!rightTapsDone) return;

            if (DetectedAction != YipliUtils.PlayerActions.JUMP)
            {
                ShakeBoxes();
                return;
            }

            totalJumps++;
            ProcessBoxCheckMarks(totalJumps);
        }

        if (pauseFlowStarted)
        {
            if (DetectedAction != YipliUtils.PlayerActions.PAUSE)
            {
                ShakeTryItBox();
                return;
            }

            calculateTime = true;

            MarkTryItBox();

            threeDModelManager.ResetAllTriggers();
            threeDModelManager.ActivateResumePart();
        }

        if (resumeFlowStarted)
        {
            if (DetectedAction != YipliUtils.PlayerActions.JUMP)
            {
                ShakeTryItBox();
                return;
            }

            calculateTime = true;

            MarkTryItBox();

            threeDModelManager.ResetAllTriggers();

            threeDModelManager.SetMainChracterAndMatToProperPostions();
            //threeDModelManager.ResetAllRotationManagerTriggers();
            threeDModelManager.ApplyMainIdleOverride();
        }

        if (finalTutStarted)
        {
            if (DetectedAction == YipliUtils.PlayerActions.LEFT_TAP)
            {
                currentCircleChildActive = GetPreviousCircleChild();
                TraverseThroughAllCirclesLikeButtons();
            }
            else if (DetectedAction == YipliUtils.PlayerActions.RIGHT_TAP)
            {
                currentCircleChildActive = GetNextCircleChild();
                TraverseThroughAllCirclesLikeButtons();
            }
            else if (DetectedAction == YipliUtils.PlayerActions.JUMP)
            {
                //SelectCircleChild();
                ProcessFinalTutJumpAction();
            }
        }
    }

    public void ResetTutorial()
    {
        leftTapsDone = false;
        rightTapsDone = false;
        jumpsDone = false;
        runningIntroDone = false;
        startIntroDone = false;
        calculateTime = false;
        tutorialStarted = false;
        userInteractionStarted = false;
        pauseFlowStarted = false;
        resumeFlowStarted = false;
        tapsAndJumpInfoFlowStarted = false;
        simpleTutorialDone = false;
        finalTutStarted = false;
        finalMessageDisplayStarted = false;
        waitForNextPart = false;

        // int values
        totalLeftTaps = 0;
        totalRightTaps = 0;
        totalJumps = 0;
        currentCircleChildActive = 0;
        requiredCHildElement = -1;

        // float time variables
        currentCalculatedTime = 0;
    }

    // sound management
    private void PlaySound(AudioClip clip)
    {
        tutorialAudioSource.clip = clip;
        tutorialAudioSource.Play();
    }

    // button functions
    public void SkipTutorialButton()
    {
        DetectedAction = YipliUtils.PlayerActions.LEFT_TAP;
        ManagePlayerActions();
    }

    public void ContinueToTutorialButton()
    {
        DetectedAction = YipliUtils.PlayerActions.RIGHT_TAP;
        ManagePlayerActions();
    }

    // Enable Disable mat controls
    public void EnableMatActionListener()
    {
        listenToMatActions = true;
    }

    public void DisableMatActionListener()
    {
        listenToMatActions = false;
    }

    // Tutorial Instructions Audio Management
    public void PlayInstructionAudio(AudioClip clip)
    {
        tutorialAudioSource.clip = clip;
        tutorialAudioSource.Play();
    }

    public void FinalClapsSound()
    {
        PlayInstructionAudio(finalClaps);
    }

    // phone holder panel manager
    [Header("phone holder panel management")]
    [SerializeField] GameObject phoneHolderPanel = null;
    [SerializeField] GameObject phoneAnimationOBJ = null;
    [SerializeField] GameObject stickAnimationOBJ = null;
    [SerializeField] GameObject pcAnimationOBJ = null;

    private void TurnOffAllPHAnimationOBJs()
    {
        phoneAnimationOBJ.SetActive(false);
        stickAnimationOBJ.SetActive(false);
        pcAnimationOBJ.SetActive(false);
        phoneHolderPanel.SetActive(false);
    }

    private void TurnOnPHAnimationOBJs()
    {
        Debug.LogError("new tut : from TurnOnPHAnimationOBJs");
        phoneAnimationOBJ.SetActive(true);

#if UNITY_STANDALONE_WIN
        pcAnimationOBJ.SetActive(true);
#else
        if (currentYipliConfig.isDeviceAndroidTV)
        {
            stickAnimationOBJ.SetActive(true);
        }
        else
        {
            Debug.LogError("new tut : from TurnOnPHAnimationOBJs else else");
            phoneAnimationOBJ.SetActive(true);
        }
#endif
    }

    private void StartTutrorialOnJump()
    {
        Debug.LogError("new tut : from StartTutrorialOnJump");
        phoneAnimationOBJ.SetActive(false);
        StartMatTutorial();
    }

    public void QuitFromTutorial()
    {
        Application.Quit();
    }
}