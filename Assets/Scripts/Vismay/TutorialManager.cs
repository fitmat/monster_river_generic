using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YipliFMDriverCommunication;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    // required variables
    [Header("Interactive tutorial components")]
    [SerializeField] List<Button> tuorialButtons;
    [SerializeField] Button continueButton;
    [SerializeField] Button currentB;

    [SerializeField] TextMeshProUGUI instructionOne;
    [SerializeField] TextMeshProUGUI instructionTwo;
    [SerializeField] TextMeshProUGUI finalInstruction;

    [SerializeField] TextMeshProUGUI leftTaps;
    [SerializeField] TextMeshProUGUI rightTaps;
    [SerializeField] TextMeshProUGUI buttonClicks;

    [SerializeField] MatInputController matInputController;

    [SerializeField] RawImage buttonOneImg;
    [SerializeField] RawImage buttonTwoImg;
    [SerializeField] RawImage buttonThreeImg;

    [Header("NonInteractive tutorial components")]
    [SerializeField] TextMeshProUGUI instructionsText;

    [SerializeField] RawImage standAnimationImg;
    [SerializeField] RawImage leftTapAnimationImg;
    [SerializeField] RawImage rightTapAnimationImg;

    [Header("Tutorial Panels")]
    [SerializeField] GameObject interactiveTutPanel;
    [SerializeField] GameObject nonInteractiveTutPanel;

    const string LEFT = "left";
    const string RIGHT = "right";
    const string ENTER = "enter";

    int currentButtonIndex = 0;
    int totalLeftTaps = 0;
    int totalRightTaps = 0;

    YipliUtils.PlayerActions detectedAction;

    bool tappingsDone = false;

    bool buttonOneClicked = false;
    bool buttonTwoClicked = false;
    bool buttonThreeClicked = false;

    private string[] leftTapsText = {
        "Perfect Left Tap",
        "That's a Left Tap",
        "Good Tap",
        "Left Tap done nicely"
    };

    private string[] rightTapsText = {
        "Great Right Tap",
        "That's a magnificient Right Tap",
        "Good Tap",
        "Right Tap done perfectly"
    };

    public YipliUtils.PlayerActions DetectedAction { get => detectedAction; set => detectedAction = value; }
    public string[] LeftTapsText { get => leftTapsText; set => leftTapsText = value; }
    public string[] RightTapsText { get => rightTapsText; set => rightTapsText = value; }

    private void Start()
    {
        TurnOffInteractiveTutorialChildren();
    }

    private void TurnOffInteractiveTutorialChildren()
    {
        leftTaps.transform.localScale = new Vector3(0f, 0f, 0f);

        for (int i = 0; i < leftTaps.transform.childCount; i++)
        {
            leftTaps.transform.GetChild(i).transform.localScale = new Vector3(0f, 0f, 0f);
        }

        rightTaps.transform.localScale = new Vector3(0f, 0f, 0f);

        for (int i = 0; i < rightTaps.transform.childCount; i++)
        {
            rightTaps.transform.GetChild(i).transform.localScale = new Vector3(0f, 0f, 0f);
        }

        instructionOne.text = "";
        instructionTwo.text = "";

        leftTaps.gameObject.SetActive(false);
        rightTaps.gameObject.SetActive(false);

        foreach (Button b in tuorialButtons)
        {
            b.transform.localScale = new Vector3(0f, 0f, 0f);
            b.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (matInputController.IsTutorialRunning)
        {
            GetMatUIKeyboardInputs();
            ManageMatActions();
        }
    }

    public void ActivateTutorial()
    {
        StopCoroutine(ActivateNITutorial());
        StartCoroutine(ActivateNITutorial());
    }

    private IEnumerator ActivateNITutorial()
    {
        //start part
        interactiveTutPanel.SetActive(false);
        leftTapAnimationImg.gameObject.SetActive(false);
        rightTapAnimationImg.gameObject.SetActive(false);

        standAnimationImg.gameObject.SetActive(true);
        nonInteractiveTutPanel.SetActive(true);

        instructionsText.text = "Stand comfortably on the MAT";

        // second part
        instructionsText.text = "Use Left/Right Taps to Navigate";

        standAnimationImg.gameObject.SetActive(false);

        leftTapAnimationImg.gameObject.SetActive(true);
        rightTapAnimationImg.gameObject.SetActive(false);

        // wait for tap animations to finish 2 times. 1 is 9 seconds
        yield return new WaitForSecondsRealtime(18f);

        // final part
        nonInteractiveTutPanel.SetActive(false);

        StartInteractiveTutorialCoroutine();
    }

    private void StartInteractiveTutorialCoroutine()
    {
        StopCoroutine(ActivateNITutorial());
        StartCoroutine(AnimateInteractiveTutorialIntro());
    }

    private IEnumerator AnimateInteractiveTutorialIntro()
    {
        interactiveTutPanel.SetActive(true);

        //essentials
        buttonClicks.gameObject.SetActive(false);

        instructionOne.text = "Let's try Tapping now";
        instructionOne.GetComponent<Animator>().enabled = true;
        instructionTwo.text = "";

        leftTaps.gameObject.SetActive(true);
        rightTaps.gameObject.SetActive(true);
        buttonClicks.gameObject.SetActive(true);

        while (leftTaps.transform.localScale.x < 1)
        {
            leftTaps.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            yield return new WaitForSecondsRealtime(0.1f);
        }

        for (int i = 0; i < leftTaps.transform.childCount; i++)
        {
            while (leftTaps.transform.GetChild(i).transform.localScale.x < 1)
            {
                leftTaps.transform.GetChild(i).transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                yield return new WaitForSecondsRealtime(0.05f);
            }
        }

        while (rightTaps.transform.localScale.x < 1)
        {
            rightTaps.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            yield return new WaitForSecondsRealtime(0.05f);
        }

        for (int i = 0; i < rightTaps.transform.childCount; i++)
        {
            while (rightTaps.transform.GetChild(i).transform.localScale.x < 1)
            {
                rightTaps.transform.GetChild(i).transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                yield return new WaitForSecondsRealtime(0.05f);
            }
        }

        foreach (Button b in tuorialButtons)
        {
            while (b.transform.localScale.x < 1)
            {
                b.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                yield return new WaitForSecondsRealtime(0.05f);
            }

            b.gameObject.SetActive(true);
        }

        ActivateITutorial();
    }

    public void ActivateITutorial()
    {
        matInputController.IsTutorialRunning = true;

        currentButtonIndex = 1;
        ManageCurrentButton();

        finalInstruction.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        buttonClicks.gameObject.SetActive(false);
    }

    private void GetMatUIKeyboardInputs()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ProcessMatInputs(LEFT);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ProcessMatInputs(RIGHT);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ProcessMatInputs(ENTER);
        }
    }

    private void ManageMatActions()
    {
        string fmActionData = InitBLE.GetFMResponse();
        Debug.Log("Json Data from Fmdriver : " + fmActionData);

        FmDriverResponseInfo singlePlayerResponse = JsonUtility.FromJson<FmDriverResponseInfo>(fmActionData);

        if (singlePlayerResponse == null) return;

        if (PlayerSession.Instance.currentYipliConfig.oldFMResponseCount < singlePlayerResponse.count)
        {
            PlayerSession.Instance.currentYipliConfig.oldFMResponseCount = singlePlayerResponse.count;

            DetectedAction = ActionAndGameInfoManager.GetActionEnumFromActionID(singlePlayerResponse.playerdata[0].fmresponse.action_id);

            switch (DetectedAction)
            {
                // UI input executions
                case YipliUtils.PlayerActions.LEFT:
                    ProcessMatInputs(LEFT);
                    break;

                case YipliUtils.PlayerActions.RIGHT:
                    ProcessMatInputs(RIGHT);
                    break;

                case YipliUtils.PlayerActions.ENTER:
                    ProcessMatInputs(ENTER);
                    break;

                default:
                    Debug.LogError("Wrong Action is detected : " + DetectedAction.ToString());
                    break;
            }
        }
    }

    private void ManageCurrentButton()
    {
        for (int i = 0; i < tuorialButtons.Count; i++)
        {
            if (i == currentButtonIndex)
            {
                // animate button
                tuorialButtons[i].GetComponent<Animator>().enabled = true;
                tuorialButtons[i].transform.GetChild(0).gameObject.SetActive(true);
                currentB = tuorialButtons[i];
            }
            else
            {
                // do nothing
                tuorialButtons[i].transform.localScale = new Vector3(1f, 1f, 1f);
                tuorialButtons[i].GetComponent<Animator>().enabled = false;
                tuorialButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void ProcessMatInputs(string matInput)
    {
        switch (matInput)
        {
            case LEFT:
                currentButtonIndex = GetPreviousButton();
                ManageCurrentButton();
                ProgressTutorial(LEFT);
                break;

            case RIGHT:
                currentButtonIndex = GetNextButton();
                ManageCurrentButton();
                ProgressTutorial(RIGHT);
                break;

            case ENTER:
                ProgressTutorial(ENTER);
                break;

            default:
                Debug.Log("Wrong Input");
                break;
        }
    }

    private int GetNextButton()
    {
        if ((currentButtonIndex + 1) == tuorialButtons.Count)
        {
            return 0;
        }
        else
        {
            return currentButtonIndex + 1;
        }
    }

    private int GetPreviousButton()
    {
        if (currentButtonIndex == 0)
        {
            return tuorialButtons.Count - 1;
        }
        else
        {
            return currentButtonIndex - 1;
        }
    }

    private void ProgressTutorial(string providedAction)
    {
        if((providedAction == LEFT || providedAction == RIGHT) && !tappingsDone)
        {
            if (providedAction == LEFT)
            {
                totalLeftTaps++;
                instructionOne.text = "";
                instructionOne.GetComponent<Animator>().enabled = false;
                //instructionTwo.text = LeftTapsText[Random.Range(0, LeftTapsText.Length)];

                ProgressCheckMarks(totalLeftTaps, leftTaps.gameObject);
            }
            else
            {
                totalRightTaps++;
                instructionOne.text = "";
                instructionOne.GetComponent<Animator>().enabled = false;
                //instructionTwo.text = RightTapsText[Random.Range(0, RightTapsText.Length)];

                ProgressCheckMarks(totalRightTaps, rightTaps.gameObject);
            }

            if (totalLeftTaps >= 3 && totalRightTaps >= 3)
            {
                StartCoroutine(TeachJump());
            }
        }
        else if (providedAction == ENTER && tappingsDone)
        {
            ClickButton();

            if (buttonOneClicked && buttonTwoClicked && buttonThreeClicked)
            {
                instructionOne.text = "";
                instructionOne.GetComponent<Animator>().enabled = false;
                instructionTwo.text = "";

                foreach (Button b in tuorialButtons)
                {
                    b.gameObject.SetActive(false);
                }

                leftTaps.gameObject.SetActive(false);
                rightTaps.gameObject.SetActive(false);
                buttonClicks.gameObject.SetActive(false);

                currentB = continueButton;

                finalInstruction.gameObject.SetActive(true);
                continueButton.gameObject.SetActive(true);
            }
        }
    }

    public void ClickButton()
    {
        currentB.onClick.Invoke();
    }

    public void ContinueButton()
    {
        matInputController.IsTutorialRunning = false;
        //currentB.onClick.Invoke();
    }

    private void ProgressCheckMarks(int caseNumber, GameObject effectThisObject)
    {
        switch (caseNumber)
        {
            case 1:
                StartCoroutine(FillImage(effectThisObject.transform.GetChild(0).GetComponent<Image>(), effectThisObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>()));

                /*
                effectThisObject.transform.GetChild(0).GetComponent<RawImage>().color = Color.green;
                effectThisObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
                effectThisObject.transform.GetChild(0).gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                progressBarI.fillAmount += 0.06f;
                */
                break;

            case 2:
                StartCoroutine(FillImage(effectThisObject.transform.GetChild(1).GetComponent<Image>(), effectThisObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>()));

                /*
                effectThisObject.transform.GetChild(1).GetComponent<RawImage>().color = Color.green;
                effectThisObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
                effectThisObject.transform.GetChild(1).transform.localScale = new Vector3(1f, 1f, 1f);
                progressBarI.fillAmount += 0.06f;
                */
                break;

            case 3:
                StartCoroutine(FillImage(effectThisObject.transform.GetChild(2).GetComponent<Image>(), effectThisObject.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>()));

                /*
                effectThisObject.transform.GetChild(2).GetComponent<RawImage>().color = Color.green;
                effectThisObject.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
                effectThisObject.transform.GetChild(2).gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                progressBarI.fillAmount += 0.06f;
                */
                break;

            default:
                Debug.LogError(effectThisObject.name + " taps number is not 1,2,3 : number is : " + caseNumber);
                break;
        }
    }

    private IEnumerator TeachJump()
    {
        foreach (Button b in tuorialButtons)
        {
            b.gameObject.SetActive(false);
        }

        instructionOne.text = "Now Let's try selecting the buttons";
        instructionOne.GetComponent<Animator>().enabled = true;
        instructionTwo.text = "";
        yield return new WaitForSecondsRealtime(2f);

        instructionOne.text = "jump to select the button";
        instructionOne.GetComponent<Animator>().enabled = true;
        instructionTwo.text = "";
        yield return new WaitForSecondsRealtime(2f);

        instructionOne.GetComponent<Animator>().enabled = false;

        foreach (Button b in tuorialButtons)
        {
            b.gameObject.SetActive(true);
            b.GetComponentInChildren<TextMeshProUGUI>().text = "Selecte Me";
        }
        ManageCurrentButton();

        buttonClicks.gameObject.SetActive(true);

        tappingsDone = true;
    }

    public void ButtonOneClick()
    {
        instructionOne.text = "";
        //instructionTwo.text = "Button 1 Pressed";

        if (!buttonOneClicked)
        {
            ProgressCheckMarks(1, buttonClicks.gameObject);
        }

        buttonOneImg.color = Color.green;
        buttonOneImg.transform.parent.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Selected";
        buttonOneClicked = true;
    }

    public void ButtonTwoClick()
    {
        instructionOne.text = "";
        //instructionTwo.text = "Button 2 Pressed";

        if (!buttonTwoClicked)
        {
            ProgressCheckMarks(2, buttonClicks.gameObject);
        }

        buttonTwoImg.color = Color.green;
        buttonTwoImg.transform.parent.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Selected";
        buttonTwoClicked = true;
    }

    public void ButtonThreeClick()
    {
        instructionOne.text = "";
        //instructionTwo.text = "Button 3 Pressed";
        
        if (!buttonThreeClicked)
        {
            ProgressCheckMarks(3, buttonClicks.gameObject);
        }

        buttonThreeImg.color = Color.green;
        buttonThreeImg.transform.parent.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Selected";
        buttonThreeClicked = true;
    }

    public void StartDirectInteractiveTutorial()
    {
        StopCoroutine(ActivateNITutorial());

        // final part
        nonInteractiveTutPanel.SetActive(false);
        interactiveTutPanel.SetActive(true);

        ActivateITutorial();
    }

    private IEnumerator FillImage(Image squreBox, Image rightArrow)
    {
        rightArrow.fillAmount = 0;

        while (rightArrow.fillAmount < 1f)
        {
            rightArrow.fillAmount += 0.1f;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        squreBox.color = Color.green;
    }
}
