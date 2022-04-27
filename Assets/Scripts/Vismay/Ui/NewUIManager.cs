using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewUIManager : MonoBehaviour
{
    // required variables
    [Header("UI Elements")]
    [SerializeField] private Button mainCommonButton = null;
    [SerializeField] private TextMeshProUGUI commonButtonText = null;
    [SerializeField] private Sprite errorSprite = null;
    [SerializeField] private Sprite regularSprite = null;

    // current yipli config
    [Header("Scriptables requirements")]
    [SerializeField] YipliConfig currentyipliConfig = null;

    [Header("Required Script objects")]
    [SerializeField] private PlayerSelection playerSelection = null;
    [SerializeField] private MatSelection matSelection = null;

    [Header("Required Script objects")]
    [SerializeField] private Color yipliRed;
    [SerializeField] private Color yipliMarine;
    [SerializeField] private Color yipliBubble;

    public string currentPanelTag = string.Empty;
    private bool currentIsMainTanenceModeOn = false;

    /* All Panels and Buttons Tags */

    //Panels----------
    const string playerSelectionPanel = "PlayerSelectionPanel";
    const string noInternetPanel = "NoInternetPanel";
    const string noMatPanel = "NoMatPanel"; // guest user panel
    const string launchFromYipliAppPanel = "LaunchFromYipliAppPanel";
    const string maintanencePanel = "MaintanencePanel";
    const string noMatConnectionPanel = "NoMatConnectionPanel";
    const string phoneHolderTutorialPanel = "PhoneHolderTutorialPanel";
    const string minimum2Player = "Minimum2Player";

    //Buttons---------

    const string mainCommonButtonTag = "mainCommonButton";

    // void Start() {
    //     TurnOffMainCommonButton();
    // }

    // panels with buttons
    // noInternetPanel, noMatPanel(Guest User panel), maintanencePanel, noMatConnectionPanel, phoneHolderTutorialPanel, minimum2Player

    public void UpdateButtonDisplay(string panelTag, bool isMainTanenceModeOn = false) {
        switch(panelTag) {
            case playerSelectionPanel:
                currentPanelTag = playerSelectionPanel;
                break;

            case noInternetPanel:
                currentPanelTag = noInternetPanel;

                commonButtonText.text = "Try Again";
                ChangeToErrorSprite();
                //TurnOnMainCommonButton();
                TurnMainButtonScaleToOne();
                break;

            case noMatPanel:
                currentPanelTag = noMatPanel;

                commonButtonText.text = "Get Mat"; // check if we can use BUY as button text // Minimum2Player
                //TurnOnMainCommonButton();
                TurnMainButtonScaleToOne();
                break;

            case launchFromYipliAppPanel:
                currentPanelTag = launchFromYipliAppPanel;
                break;

            case maintanencePanel:
                currentPanelTag = maintanencePanel;

                currentIsMainTanenceModeOn = isMainTanenceModeOn;

                if (isMainTanenceModeOn) {
                    commonButtonText.text = "Quit";
                    ChangeToErrorSprite();
                } else {
                    commonButtonText.text = "Update";
                }

                //TurnOnMainCommonButton();
                TurnMainButtonScaleToOne();
                break;

            case noMatConnectionPanel:
                currentPanelTag = noMatConnectionPanel;

                commonButtonText.text = "Recheck";
                ChangeToErrorSprite();
                //TurnOnMainCommonButton();
                TurnMainButtonScaleToOne();
                break;

            case phoneHolderTutorialPanel:
                currentPanelTag = phoneHolderTutorialPanel;

                commonButtonText.text = "Jump";
                //TurnOnMainCommonButton();
                TurnMainButtonScaleToZero();
                break;

            case minimum2Player:
                currentPanelTag = minimum2Player;

                commonButtonText.text = "Create";
                //TurnOnMainCommonButton();
                TurnMainButtonScaleToOne();
                break;

            default:
                break;
        }
    }

    public void MainButtonFucntion() {
        switch(currentPanelTag) {
            case playerSelectionPanel:
                break;

            case noInternetPanel:
                playerSelection.TryAgainInternetConnection();
                break;

            case noMatPanel:
                if (currentyipliConfig.getMatUrlIn == null || currentyipliConfig.getMatUrlUS == null) {
                    if (System.Globalization.RegionInfo.CurrentRegion.ToString().Equals("IN", System.StringComparison.OrdinalIgnoreCase)) {
                        Application.OpenURL(currentyipliConfig.getMatUrlIn);
                    } else {
                        Application.OpenURL(currentyipliConfig.getMatUrlUS);
                    }
                } else {
                    if (System.Globalization.RegionInfo.CurrentRegion.ToString().Equals("IN", System.StringComparison.OrdinalIgnoreCase)) {
                        Application.OpenURL(ProductMessages.GetMatUrlIn);
                    } else {
                        Application.OpenURL(ProductMessages.GetMatUrlUS);
                    }
                }
                break;

            case launchFromYipliAppPanel:
                break;

            case maintanencePanel:
                if (currentIsMainTanenceModeOn) {
                    // quit application.
                    Application.Quit();
                } else {
                    // update game
                    playerSelection.OnUpdateGameClick();
                }
                break;

            case noMatConnectionPanel:
                matSelection.ReCheckMatConnection();
                break;

            case phoneHolderTutorialPanel:
                TurnMainButtonScaleToOne();
                playerSelection.OnJumpOnMat();
                break;

            case minimum2Player:
                playerSelection.OnGoToYipliPress();
                break;

            default:
                break;
        }
    }

    public void TurnOnMainCommonButton() {
        mainCommonButton.gameObject.SetActive(true);
    }

    public void TurnOffMainCommonButton() {
        ChangeToRegularSprite();
        mainCommonButton.gameObject.SetActive(false);
    }

    public void TurnMainButtonScaleToZero() {
        mainCommonButton.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    public void TurnMainButtonScaleToOne() {
        mainCommonButton.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void ChangeToErrorSprite() {
        mainCommonButton.GetComponent<Image>().sprite = errorSprite;
        mainCommonButton.transform.GetChild(0).GetComponent<Image>().sprite = errorSprite;
        mainCommonButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = yipliMarine;
    }

    private void ChangeToRegularSprite() {
        mainCommonButton.GetComponent<Image>().sprite = regularSprite;
        mainCommonButton.transform.GetChild(0).GetComponent<Image>().sprite = regularSprite;
        mainCommonButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.white;
    }
}
