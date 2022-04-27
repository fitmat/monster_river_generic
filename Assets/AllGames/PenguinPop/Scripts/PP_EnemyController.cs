using UnityEngine;

public class PP_EnemyController : MonoBehaviour
{
    public bool ignorePlayer1, ignorePlayer2;

    private void OnEnable()
    {
        ignorePlayer1 = false;
        ignorePlayer2 = false;
    }
}
