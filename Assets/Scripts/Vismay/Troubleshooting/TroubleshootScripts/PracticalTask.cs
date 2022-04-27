using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PracticalTask : MonoBehaviour
{
    enum Sectiuons
    {
        Far, // far mat
        Near, // near mat
        Out // out of the mat
    }

    // required variables
    [Header("Aniamtion objects")]
    [SerializeField] GameObject legsParent;

    [Header("mat pexel requirements")]
    [SerializeField] GameObject matPanel;
    [SerializeField] GameObject cellWhitePrefab;
    [SerializeField] GameObject cellGreenPrefab;

    [Header("Aniamtion placements")]
    [SerializeField] Vector3[] legsPositions;

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI instructionText;

    [Header("Mat Pixels")]
    [SerializeField] Image[] thausandPixels;

    private TroubleShootSystem tss;

    int totalActivePixelsInFarSection = 0;
    int totalActivePixelsInNearSection = 0;
    int totalJumpingJackDone = 0;

    public bool checkForPosition = false;
    public bool askUserToDoJJ = false;
    public bool timerOver = false;
    public bool stepOneDone = false;
    public bool stepTwoDone = false;
    public bool stepThreeDone = false;
    public bool stepFourDone = false;

    Sectiuons playersCurrentStandSection = Sectiuons.Out;

    private List<string> fmResponseList = null;

    public int TotalActivePixelsInFarSection { get => totalActivePixelsInFarSection; set => totalActivePixelsInFarSection = value; }
    public int TotalActivePixelsInNearSection { get => totalActivePixelsInNearSection; set => totalActivePixelsInNearSection = value; }
    public bool CheckForPosition { get => checkForPosition; set => checkForPosition = value; }
    public bool AskUserToDoJJ { get => askUserToDoJJ; set => askUserToDoJJ = value; }
    public bool TimerOver { get => timerOver; set => timerOver = value; }
    public int TotalJumpingJackDone { get => totalJumpingJackDone; set => totalJumpingJackDone = value; }
    public bool StepOneDone { get => stepOneDone; set => stepOneDone = value; }
    public bool StepThreeDone { get => stepThreeDone; set => stepThreeDone = value; }
    public bool StepTwoDone { get => stepTwoDone; set => stepTwoDone = value; }
    public bool StepFourDone { get => stepFourDone; set => stepFourDone = value; }
    private Sectiuons PlayersCurrentStandSection { get => playersCurrentStandSection; set => playersCurrentStandSection = value; }

    private void Awake()
    {
        tss = FindObjectOfType<TroubleShootSystem>();
    }

    private void Start()
    {
        //TurnOfAllPixels();

        DeactivateLegsAnimationObjects();
    }

    public void ManagePracticalTaskStepOne()
    {
        fmResponseList = new List<string>();

        // step 1
        instructionText.text = "Stand on the LEFT section of Mat as shown";
        legsParent.transform.GetChild(0).gameObject.SetActive(true);

        StartCoroutine(DeactivateAnimationObject(2f, legsParent.transform.GetChild(0).gameObject));
    }

    public void ManagePracticalTaskStepTwo()
    {
        // step 2
        instructionText.text = "Do Jumping jack";

        AskUserToDoJJ = false;

        // activate jj animation here
        StartCoroutine(TimerClock(5f)); // provide time here
    }

    public void ManagePracticalTaskStepThree()
    {
        // step 3
        instructionText.text = "Stand on the Right section of Mat as shown";
        legsParent.transform.GetChild(1).gameObject.SetActive(true);

        StartCoroutine(DeactivateAnimationObject(2f, legsParent.transform.GetChild(1).gameObject));
    }
    public void ManagePracticalTaskStepFour()
    {
        // step 4
        tss.CheckMatActions = false;

        // save all responses to file
        tss.FlowInfo += "PracticalDone->";

        string desc = "Practical task is performed successfully, check data for MAT Cracks.";

        FmResponseFile.GenerateFilesAndUpload(fmResponseList, tss.FlowInfo, tss.TroubleshootManager.CurrentAlgorithmID, tss.CurrentYipliConfig, desc, tss.TroubleshootManager.GetTroubleShootScriptableJson());

        // generate Ticket
        Debug.LogError("Ticket is generated");
        StepFourDone = true;

        ResetPracticalTaskFlags();
        tss.ResetTroubleShooter();
        tss.TurnOnMessageBoxPanel("Your problem Ticket is generated.");
    }


    public void ManagePracticaltask(string fmData, string fmResponse, string extraAction = "NoAction")
    {
        fmResponseList.Add(fmResponse);

        char[] fmDataArray = fmData.ToCharArray();

        // check if JJ is received
        if (extraAction != "NoAction")
        {
            TotalJumpingJackDone += 1;
        }

        // check player position
        if (CheckForPosition)
        {
            MakeMatSectionDecesion(fmDataArray);
            Debug.LogError("player's current position is : " + playersCurrentStandSection.ToString());
            return;
        }

        // return is player is not on the mat
        if (PlayersCurrentStandSection == Sectiuons.Out)
        {
            Debug.LogError("player is still our of the Mat, returning");
            return;
        }

        // ask user to do JJ
        if (AskUserToDoJJ)
        {
            ManagePracticalTaskStepTwo();
            return;
        }

        // until timer runs return
        if (!TimerOver) return;

        // step 2
        if ((TimerOver || TotalJumpingJackDone > 2) && StepOneDone && !StepTwoDone && !StepThreeDone && !StepFourDone)
        {
            TimerOver = true;
            StepTwoDone = true;
            ManagePracticalTaskStepThree();
        }

        // step 2 again after step 3
        if ((TimerOver || TotalJumpingJackDone > 2) && StepOneDone && StepTwoDone && StepThreeDone && !StepFourDone)
        {
            TimerOver = true;
            ManagePracticalTaskStepFour();
        }

        // do something based on sections

        /*
        // code for player position representgation on mat with green cells
        for (int i = 0; i < thausandPixels.Length; i++)
        {
            if (char.GetNumericValue(fmDataArray[i]) == 1)
            {
                thausandPixels[i].color = Color.green;
            }
            else
            {
                thausandPixels[i].color = Color.white;
            }
        }
        */
    }

    private void MakeMatSectionDecesion(char[] fmDataArray)
    {
        for (int i = 0; i < 500; i++)
        {
            if (char.GetNumericValue(fmDataArray[i]) == 1)
            {
                TotalActivePixelsInFarSection += 1;
            }
        }

        for (int i = 500; i < 1000; i++)
        {
            if (char.GetNumericValue(fmDataArray[i]) == 1)
            {
                TotalActivePixelsInNearSection += 1;
            }
        }

       // player is in Near section
        if (TotalActivePixelsInNearSection > 5)
        {
            PlayersCurrentStandSection = Sectiuons.Near;
            CheckForPosition = false;
            AskUserToDoJJ = true;
        }
        
        else if (TotalActivePixelsInFarSection > 5)
            {
            // player is in far section
                PlayersCurrentStandSection = Sectiuons.Far;
                CheckForPosition = false;
                AskUserToDoJJ = true;
            
        }
        

       /* else if (TotalActivePixelsInFarSection != 0 && TotalActivePixelsInNearSection != 0 && 
            ( (TotalActivePixelsInFarSection + TotalActivePixelsInNearSection) > 18 && 
              (TotalActivePixelsInFarSection + TotalActivePixelsInNearSection) < 32)  )
        {
            if (TotalActivePixelsInNearSection > TotalActivePixelsInFarSection)
            {
                // player is in Near section
                PlayersCurrentStandSection = Sectiuons.Near;
            }
            else
            {
                // player is in far section
                PlayersCurrentStandSection = Sectiuons.Far;
            }

            CheckForPosition = false;
            AskUserToDoJJ = true;
        }*/
        else
        {
            PlayersCurrentStandSection = Sectiuons.Out;
        }
    }

    private void DeactivateLegsAnimationObjects()
    {
        for (int i = 0; i < legsParent.transform.childCount; i++)
        {
            legsParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private IEnumerator DeactivateAnimationObject(float waitTime, GameObject objectToDeactivate)
    {
        yield return new WaitForSecondsRealtime(waitTime);

        objectToDeactivate.SetActive(false);
        matPanel.SetActive(true);

        // start checking the mat actions
        CheckForPosition = true;
        tss.CheckMatActions = true;

        // set steps flags
        if (!StepOneDone && !StepThreeDone)
        {
            StepOneDone = true;
        }
        else if (StepOneDone && !StepThreeDone)
        {
            StepThreeDone = true;
        }
    }

    private IEnumerator TimerClock(float waitTime)
    {
        TimerOver = false;
        yield return new WaitForSecondsRealtime(waitTime);
        TimerOver = true;
    }

    private void TurnOfAllPixels()
    {
        for (int i = 0; i < matPanel.transform.childCount; i++)
        {
            matPanel.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void ResetPracticalTaskFlags()
    {
        StepOneDone = false;
        StepTwoDone = false;
        StepThreeDone = false;
        StepFourDone = false;
    }
}
