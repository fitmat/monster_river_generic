using UnityEngine;

public class TroubleShootGL : MonoBehaviour
{
    // required varibales
    public YipliConfig currentYipliConfig;

    void Start()
    {
        TroubleShootGameLib();
    }

    public void TroubleShootGameLib()
    {
        if (currentYipliConfig.troubleshootingPOSTDone) return;

        if (currentYipliConfig.gameId == null) return;

        if (YipliHelper.GetGameClusterId() != 0) return;
        if (!YipliHelper.checkInternetConnection()) return;

        if (FindObjectOfType<MatSelection>() == null) return;
        if (FindObjectOfType<PlayerSelection>() == null) return;

        // activate driver troubleshoot module here

        currentYipliConfig.troubleshootingPOSTDone = true;
    }
}
