using UnityEngine;
using UnityEngine.UI;
using YipliFMDriverCommunication;
using DG.Tweening;
using System.Collections;
using TMPro;

public class NewMatInputController : MonoBehaviour
{
    // required variables
    [Header("Mat Elelments")]
    [SerializeField] private Image matLeftButton = null;
    [SerializeField] private Image matRightButton = null;
    [SerializeField] private Image matCentreButton = null;
    [SerializeField] private Image matFullButton = null;
    [SerializeField] private Canvas matCanvasComponent = null;

    [SerializeField] private GameObject matParentObj = null;

    [Header("Vector for positions")]
    [SerializeField] Vector3 playerSelectionPosition;
    [SerializeField] Vector3 tutorialPosition;
    [SerializeField] Vector3 switchPlayerPosition;

    [Header("Chevrons")]
    [SerializeField] GameObject chevronParent = null;

    [Header("Text Buttons")]
    [SerializeField] GameObject textButtonsParent = null;

    [Header("Colors")]
    [SerializeField] private Color yipliRed;
    [SerializeField] private Color yipliBlue;
    [SerializeField] private Color originalButtonColor;

    [Header("Legs part")]
    [SerializeField] private GameObject legsParent = null;

    [Header("Required script objects")]
    [SerializeField] private SecondTutorialManager secondTutorialManager = null;
    [SerializeField] private MatInputController matInputController = null;

    public void DisplayMainMat() {
        matParentObj.SetActive(true);
    }

    public void HideMainMat() {
        matParentObj.SetActive(false);
    }

    void Start() {
        matLeftButton.GetComponent<Animator>().enabled = false;
        matRightButton.GetComponent<Animator>().enabled = false;

        //matFullButton.gameObject.SetActive(false);

        //HideTextButtons();
        //HideChevrons();
    }

    public void EnableMatLeftButtonAnimator() {
        // matLeftButton.GetComponent<Animator>().enabled = true;
        legsParent.GetComponent<Animator>().SetTrigger("lefttap");
    }

    public void EnableMatRightButtonAnimator() {
        // matRightButton.GetComponent<Animator>().enabled = true;
        legsParent.GetComponent<Animator>().SetTrigger("righttap");
    }

    public void EnableMatParentButtonAnimator() {
        matParentObj.GetComponent<Animator>().enabled = true;
    }

    public void DisableMatParentButtonAnimator() {
        matParentObj.GetComponent<Animator>().enabled = false;;
    }

    public bool GetMatDisplayStatus() {
        return matParentObj.activeSelf;
    }

    public void DisableAnimators()
    {
        matLeftButton.GetComponent<Animator>().enabled = false;
        matRightButton.GetComponent<Animator>().enabled = false;
        matParentObj.GetComponent<Animator>().enabled = false;

        KeepLeftNadRightButtonColorToOriginal();
        //matCentreButton.color = originalButtonColor;
    }

    public void KeepLeftNadRightButtonColorToOriginal()
    {
        matLeftButton.color = originalButtonColor;
        matRightButton.color = originalButtonColor;
    }

    public void SetMatPlayerSelectionPosition() {
        matParentObj.transform.localPosition = playerSelectionPosition;
    }

    public void SetMatTutorialPosition() {
        matParentObj.transform.localPosition = tutorialPosition;

        textButtonsParent.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Skip";
    }

    public void SetMatSwitchPlayerPosition() {
        matParentObj.transform.localPosition = switchPlayerPosition;
        matParentObj.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
    }

    public void SetMatToNormalScale() {
        matParentObj.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void DisplayChevrons() {
        matFullButton.gameObject.SetActive(true);
        chevronParent.SetActive(true);
    }

    public void HideChevrons() {
        matFullButton.gameObject.SetActive(false);
        chevronParent.SetActive(false);
    }

    public void DisplayTextButtons() {
        textButtonsParent.SetActive(true);
    }

    public void HideTextButtons() {
        textButtonsParent.SetActive(false);
    }

    public void UpdateCenterButtonColor() {
        matCentreButton.color = yipliRed;
    }

    public void UpdateCenterButtonWithOriginalColor() {
        matCentreButton.color = originalButtonColor;
    }

    public void DisplayMatForSwitchPlayerPanel() {
        textButtonsParent.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Change";

        HideLegs();
        DisplayTextButtons();

        SetMatSwitchPlayerPosition();
    }

    public void MakeSortLayerZero() {
        matCanvasComponent.sortingOrder = 0;
    }

    public void MakeSortLayerTen() {
        matCanvasComponent.sortingOrder = 10;
    }

    public void SkipReselectButton() {
        if (matInputController.IsTutorialRunning) {
            secondTutorialManager.SkipTutorialButton();
        } else {
            matInputController.ManualLeftButton();
        }
    }

    public void ContinueButton() {
        if (matInputController.IsTutorialRunning) {
            secondTutorialManager.ContinueToTutorialButton();
        } else {
            matInputController.ManualRightButton();
        }
    }
    
    // legs management
    public void DisplayLegs() {
        legsParent.SetActive(true);
    }

    public void HideLegs() {
        matFullButton.gameObject.SetActive(false);
        legsParent.SetActive(false);
    }
}
