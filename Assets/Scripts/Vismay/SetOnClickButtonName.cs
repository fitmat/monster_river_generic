using UnityEngine;

public class SetOnClickButtonName : MonoBehaviour
{
    public void SetPlayerNameInMatInputControllerScript()
    {
        FindObjectOfType<MatInputController>().CurrentPlayerName = gameObject.name;
    }
}
