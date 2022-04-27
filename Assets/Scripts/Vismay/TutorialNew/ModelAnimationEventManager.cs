using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAnimationEventManager : MonoBehaviour
{
    [SerializeField] ThreeDModelManager threeDModelManager = null;
    [SerializeField] SecondTutorialManager secondTutorialManager = null;

    public void ChangeToJumpOverride() {
        threeDModelManager.ChangeMainCharacterToResumePos();
        threeDModelManager.ApplyJumpOverride();
    }

    public void ChangeIdleOverride() {
        threeDModelManager.ChangeMainCharacterToPausePos();
        threeDModelManager.ApplyMainIdleOverride();
    }

    public int totalActionCount = 0;

    float currentCalculatedTime = 0f;
    bool calculateTime = false;

    void Update() {
        // if (calculateTime) {
        //     currentCalculatedTime += Time.deltaTime;

        //     if (currentCalculatedTime > 3f) {
        //         calculateTime = false;
        //         currentCalculatedTime = 0f;
        //         totalActionCount = 0;

        //         if (!secondTutorialManager.leftTapsDone) {
        //             threeDModelManager.ApplyLeftTapOverride();
        //         } else if (!secondTutorialManager.rightTapsDone) {
        //             threeDModelManager.ApplyRightTapOverride();
        //         } else if (!secondTutorialManager.jumpsDone) {
        //             threeDModelManager.ApplyJumpOverride();
        //         }
        //     }
        // }
    }

    public void ManageActionsDelay() {
        //Debug.LogError("From ManageActionsDelay()");

        // if (calculateTime) return;

        // if (totalActionCount > 3) {
        //     calculateTime = true;
        //     currentCalculatedTime = 0f;

        //     threeDModelManager.ApplyMainIdleOverride();
        // } else {    
        //     totalActionCount++;
        // }
    }
}
