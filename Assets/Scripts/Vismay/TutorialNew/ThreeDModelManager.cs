using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDModelManager : MonoBehaviour
{
    // required variables
    [Header("3d Character")]
    [SerializeField] private GameObject mainCharacter = null;
    [SerializeField] private GameObject modelManager = null;
    [SerializeField] private Animator mainCharacterAnimator = null;
    [SerializeField] private GameObject threeDMat = null;
    [SerializeField] private GameObject rotationManger = null;

    //[SerializeField] private GameObject ResumeCharacter = null;

    [Header("All Override controllers")]
    [SerializeField] private AnimatorOverrideController leftTapController = null;
    [SerializeField] private AnimatorOverrideController rightTapController = null;
    [SerializeField] private AnimatorOverrideController jumpController = null;
    [SerializeField] private AnimatorOverrideController idleController = null;
    [SerializeField] private AnimatorOverrideController walkingController = null;
    [SerializeField] private AnimatorOverrideController wavingController = null;
    [SerializeField] private AnimatorOverrideController headNodController = null;
    [SerializeField] private AnimatorOverrideController fistPumpController = null;
    [SerializeField] private AnimatorOverrideController pauseController = null;
    [SerializeField] private AnimatorOverrideController resumeController = null;
    [SerializeField] private AnimatorOverrideController danceController = null;

    [Header("All Required model avatars")]
    [SerializeField] private Avatar taps3DModelAvatar = null;
    [SerializeField] private Avatar jump3DModelAvatar = null;
    [SerializeField] private Avatar pause3DModelAvatar = null;
    [SerializeField] private Avatar resume3DModelAvatar = null;
    [SerializeField] private Avatar victory3DModelAvatar = null;
    [SerializeField] private Avatar headShake3DModelAvatar = null;
    [SerializeField] private Avatar walking3DModelAvatar = null;
    [SerializeField] private Avatar waving3DModelAvatar = null;
    [SerializeField] private Avatar idle3DModelAvatar = null;
    [SerializeField] private Avatar dance3DModelAvatar = null;

    void Start() {
        DisableModelManagerAnimator();
        Hide3DModel();
        Hide3DMat();
        
        rotationManger.GetComponent<Animator>().enabled = false;
        //ResumeCharacter.SetActive(false);
    }

    public void ApplyLeftTapOverride() {
        ApplyTapsAvatar();
        mainCharacterAnimator.runtimeAnimatorController = leftTapController;
    }

    public void ApplyRightTapOverride() {
        ApplyTapsAvatar();
        mainCharacterAnimator.runtimeAnimatorController = rightTapController;
    }

    public void ApplyJumpOverride() {
        ApplyJumpAvatar();
        mainCharacterAnimator.runtimeAnimatorController = jumpController;
    }

    public void ApplyWalkingOverride() {
        ApplyWalkingAvatar();
        mainCharacterAnimator.runtimeAnimatorController = walkingController;
    }

    public void ApplyWavingOverride() {
        ApplyWavingAvatar();
        mainCharacterAnimator.runtimeAnimatorController = wavingController;
    }

    public void ApplyMainIdleOverride() {
        ApplyIdleAvatar();
        mainCharacterAnimator.runtimeAnimatorController = idleController;
    }

    public void ApplyHeadNodOverride() {
        ApplyHeadShakeAvatar();
        mainCharacterAnimator.runtimeAnimatorController = headNodController;
    }

    public void ApplyFistPumpOverride() {
        ApplyVictoryAvatar();
        mainCharacterAnimator.runtimeAnimatorController = fistPumpController;
    }

    public void ApplyResumeOverride() {
        ApplyResumeAvatar();
        mainCharacterAnimator.runtimeAnimatorController = resumeController;
    }

    public void ApplyDanceOverride() {
        ApplyDanceAvatar();
        mainCharacterAnimator.runtimeAnimatorController = danceController;
    }

    public void Display3DModel() {
        mainCharacter.SetActive(true);
    }

    public void Hide3DModel() {
        mainCharacter.SetActive(false);
    }

    public void Display3DMat() {
        threeDMat.SetActive(true);
    }

    public void Hide3DMat() {
        threeDMat.SetActive(false);
    }

    public void EnableModelManagerAnimator() {
        modelManager.GetComponent<Animator>().enabled = true;
    }

    public void DisableModelManagerAnimator() {
        modelManager.GetComponent<Animator>().enabled = false;
    }

    // pausePart, resumePart

    public void ActivatePausePart() {
        EnableModelManagerAnimator();
        //modelManager.GetComponent<Animator>().SetBool("pausePart", true);

        rotationManger.GetComponent<Animator>().enabled = true;
        rotationManger.GetComponent<Animator>().SetTrigger("pauseTo120");

        //mainCharacter.transform.localRotation = Quaternion.Euler(mainCharacter.transform.localRotation.x, 90f, mainCharacter.transform.localRotation.z);
        
        //threeDMat.transform.localRotation = Quaternion.Euler(threeDMat.transform.localRotation.x, 90f, threeDMat.transform.localRotation.z);
        //threeDMat.transform.localPosition = new Vector3(-2.0f, threeDMat.transform.localPosition.y, threeDMat.transform.localPosition.z);

        // ApplyPauseAvatar();
        // mainCharacterAnimator.runtimeAnimatorController = pauseController;
    }

    public void ApplyPauseAnimator() {
        ApplyPauseAvatar();
        mainCharacterAnimator.runtimeAnimatorController = pauseController;
    }

    public void ActivateResumePart() {
        EnableModelManagerAnimator();
        //modelManager.GetComponent<Animator>().SetBool("resumePart", true);

        //rotationManger.transform.GetChild(0).transform.localRotation = Quaternion.Euler(rotationManger.transform.localRotation.x, 120f, rotationManger.transform.localRotation.z);
        mainCharacter.transform.localPosition = new Vector3(0.3f, mainCharacter.transform.localPosition.y, -0.3f);
        
        //threeDMat.transform.localRotation = Quaternion.Euler(threeDMat.transform.localRotation.x, 90f, threeDMat.transform.localRotation.z);
        threeDMat.transform.localPosition = new Vector3(-2.0f, threeDMat.transform.localPosition.y, threeDMat.transform.localPosition.z);

        ApplyResumeAvatar();
        mainCharacterAnimator.runtimeAnimatorController = resumeController;
    }

    public void SetMainChracterAndMatToProperPostions() {
        //rotationManger.transform.GetChild(0).transform.localRotation = Quaternion.Euler(rotationManger.transform.localRotation.x, 120f, rotationManger.transform.localRotation.z);
        rotationManger.GetComponent<Animator>().SetTrigger("120ToForward180");

        mainCharacter.transform.localPosition = new Vector3(0f, mainCharacter.transform.localPosition.y, 0f);
        
        // threeDMat.transform.localRotation = Quaternion.Euler(threeDMat.transform.localRotation.x, 0f, threeDMat.transform.localRotation.z);
        threeDMat.transform.localPosition = new Vector3(-2.2f, threeDMat.transform.localPosition.y, threeDMat.transform.localPosition.z);
    }

    public void ChangeMainCharacterToPausePos() {
        mainCharacter.transform.localPosition = new Vector3(0.3f, mainCharacter.transform.localPosition.y, -0.5f);
    }

    public void ChangeMainCharacterToResumePos() {
        mainCharacter.transform.localPosition = new Vector3(0.3f, mainCharacter.transform.localPosition.y, 0.3f);
    }

    public void ResetAllRotationManagerTriggers() {
        rotationManger.GetComponent<Animator>().ResetTrigger("pausePart");
        rotationManger.GetComponent<Animator>().ResetTrigger("120ToForward180");

        rotationManger.GetComponent<Animator>().enabled = false;
    }

    public void ResetAllTriggers() {
        modelManager.GetComponent<Animator>().SetBool("pausePart", false);
        modelManager.GetComponent<Animator>().SetBool("resumePart", false);

        DisableModelManagerAnimator();
    }

    // Avatar management
    public void ApplyTapsAvatar() {
        mainCharacterAnimator.avatar = taps3DModelAvatar;
    }

    public void ApplyJumpAvatar() {
        mainCharacterAnimator.avatar = jump3DModelAvatar;
    }

    public void ApplyPauseAvatar() {
        mainCharacterAnimator.avatar = pause3DModelAvatar;
    }

    public void ApplyResumeAvatar() {
        mainCharacterAnimator.avatar = resume3DModelAvatar;
    }

    public void ApplyVictoryAvatar() {
        mainCharacterAnimator.avatar = victory3DModelAvatar;
    }

    public void ApplyHeadShakeAvatar() {
        mainCharacterAnimator.avatar = headShake3DModelAvatar;
    }

    public void ApplyWalkingAvatar() {
        mainCharacterAnimator.avatar = walking3DModelAvatar;
    }

    public void ApplyWavingAvatar() {
        mainCharacterAnimator.avatar = waving3DModelAvatar;
    }

    public void ApplyIdleAvatar() {
        mainCharacterAnimator.avatar = idle3DModelAvatar;
    }

    public void RemoveAvatar() {
        mainCharacterAnimator.avatar = null;
    }

    public void ApplyDanceAvatar() {
        mainCharacterAnimator.avatar = dance3DModelAvatar;
    }

    public void TurnModelManager180() {
        modelManager.GetComponent<Animator>().SetTrigger("turn180back");
    }

    public void StartRMFinalPart() {
        rotationManger.GetComponent<Animator>().SetTrigger("FinalPart");
    }

    // character management
    // public void EnableResumeCharatcer() {
    //     mainCharacter.SetActive(false);
    //     ResumeCharacter.SetActive(true);
    // }

    // public void DisableResumeCharatcer() {
    //     ResumeCharacter.SetActive(false);
    //     mainCharacter.SetActive(true);
    // }
}
