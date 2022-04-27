using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelEventActivator : MonoBehaviour
{
    // required variables
    [SerializeField] private ThreeDModelManager threeDModelManager = null;
    [SerializeField] private GameObject confetiParent = null;

    void Start() {
        confetiParent.SetActive(false);
    }

    public void ActivateWaveAnimator() {
        threeDModelManager.ApplyWavingOverride();
    }

    public void ActivateIdleAnimator() {
        threeDModelManager.ApplyMainIdleOverride();
    }

    public void ActivateWalkingAnimator() {
        threeDModelManager.ApplyWalkingOverride();
    }

    public void ActivateJumpAnimator() {
        threeDModelManager.ApplyJumpOverride();
    }

    public void ActivateDanceAnimator() {
        FindObjectOfType<SecondTutorialManager>().FinalClapsSound();
        confetiParent.SetActive(true);
        threeDModelManager.ApplyDanceOverride();
    }

    public void SetZTransformToZero() {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    public void ApplyPauseAnimator() {
        threeDModelManager.ApplyPauseAnimator();
    }

    public void DisableConfeties() {
        confetiParent.SetActive(false);
    }
}
