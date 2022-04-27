using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctionActivator : MonoBehaviour
{
    [SerializeField] MatInputController matInputController = null;
    [SerializeField] SecondTutorialManager secondTutorialManager = null;

    public void UpdatePlayerSelectionInfo() {
        matInputController.StartScrolling();
    }

    public void GoForNextTaskInFT() {
        secondTutorialManager.GoForNextTaskInFT();
    }
}
