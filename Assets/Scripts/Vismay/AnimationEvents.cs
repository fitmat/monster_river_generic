using UnityEngine;
using TMPro;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI actionNameText;

    private TutorialManagerGL tm;

    private void Start()
    {
        tm = FindObjectOfType<TutorialManagerGL>();
    }

    public void ChangeToLeftTap()
    {
        actionNameText.text = "Left";
    }

    public void ChangeToRightTap()
    {
        actionNameText.text = "Right";
    }

    public void PlayTapsSFX()
    {
        tm.PlayAnimationTaps();
    }
}
