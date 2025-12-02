using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public Button StartRestartButton;
    [SerializeField] public Button ResumeButton;
    [SerializeField] public Button ExitButton;

    [SerializeField] private TMP_Text StartRestartButtonText;

    public void SetStartedMenu()
    {
        StartRestartButtonText.text = "Restart";
    }

    public void SetNotStartedMenu()
    {
        StartRestartButtonText.text = "Start";
    }
}