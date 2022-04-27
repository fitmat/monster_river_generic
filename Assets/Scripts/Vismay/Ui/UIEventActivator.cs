using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventActivator : MonoBehaviour
{
    // required variables
    [SerializeField] private NewMatInputController newMatInputController = null;

    public void DisableAllButtonsAnimators() {
        newMatInputController.DisableAnimators();
    }
}
